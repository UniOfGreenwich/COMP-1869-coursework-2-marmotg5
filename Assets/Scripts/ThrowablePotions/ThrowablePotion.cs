using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ThrowablePotion : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI timerText;

    [Header("Potion Explosion Particle Effect Settings")]
    [SerializeField] GameObject potionParticlePrefab;
    [SerializeField] Color potionParticleColor = Color.white;

    [Header("Potion Object Prefab")]
    [SerializeField, Tooltip("The actual object that gets spawned on top of the zone into the world")]
    GameObject potionObjectPrefab;

    GameObject potionObject = null;

    [Header("Potion Zone Prefab")]
    [SerializeField, Tooltip("The visual object that follows the drag of the player's input")]
    GameObject potionPlaceholderPrefab;
    GameObject potionPlaceholder = null;
    Vector3 potionThrownPosition = Vector3.zero;

    [Header("Potion Settings")]
    [SerializeField] float potionRadius = 15.0f;
    [SerializeField, Tooltip("How hight from the ground should the potion be dropped at")]
    float dropHeight = 3.0f;
    [SerializeField] float dropSpeed = 6.0f;
    [SerializeField] float potionUseCooldown = 5.0f; // In seconds

    float elapsedTimeSinceLastPotionUse = 0.0f; // Updated at runtime

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePotionCooldown();
        UpdateUIText();
        DropPotion(); // Only if it's spawned
    }

    void UpdatePotionCooldown()
    {
        // If we can't use the potion due to the cooldown timer, increase it
        if (!CanUsePotion())
        {
            elapsedTimeSinceLastPotionUse -= Time.deltaTime; // We want the timer to go countdown to 0
        }
    }

    void UpdateUIText()
    {
        if (timerText != null)
        {
            if (elapsedTimeSinceLastPotionUse <= 0.0f)
            {
                // If cooldown is over and potion is ready to use, remove text
                timerText.text = "";
            }
            else
            {
                // Display the cooldown text over the potion
                timerText.text = Mathf.Round(elapsedTimeSinceLastPotionUse).ToString();
            }
        }
    }

    void SpawnPotionObject()
    {
        if (potionObjectPrefab != null)
        {
            potionObject = Instantiate(potionObjectPrefab);
            potionObject.transform.position = potionThrownPosition + new Vector3(0.0f, dropHeight, 0.0f); // Set the spawn position of the new potion
        }
    }

    IEnumerator SpawnParticleEffect()
    {
        if (potionParticlePrefab != null && potionThrownPosition != Vector3.zero)
        {
            // Spawn the particle prefab and set it to the correct location
            GameObject spawnedParticleObject = Instantiate(potionParticlePrefab);
            spawnedParticleObject.transform.position = potionThrownPosition;

            // Get the particle system component so we can tweak it's values
            ParticleSystem spawnedParticleSystem = spawnedParticleObject.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule particleMain = spawnedParticleSystem.main;
            ParticleSystem.ShapeModule particleShape = spawnedParticleSystem.shape;

            particleMain.startColor = potionParticleColor;
            particleShape.radius = potionRadius / 2.0f;

            // While loop that will infinitely loop until the particle has finished playing
            while (spawnedParticleSystem.time < particleMain.duration || spawnedParticleSystem.isPlaying)
            {
                yield return new WaitForSeconds(Time.deltaTime);
            }

            // Delete the particle after it's done playing
            Destroy(spawnedParticleObject);
        }

        yield return null;
    }

    void DropPotion()
    {
        // Make sure we have a spawned potion and we know where it has been dropped at
        if (potionObject != null && potionThrownPosition != Vector3.zero)
        {
            potionObject.transform.position -= new Vector3(0.0f, dropSpeed * Time.deltaTime, 0.0f);
            
            // Check if the potion it's close to the original thrown position
            float distanceFromThrownPosition = Vector3.Distance(potionObject.transform.position, potionThrownPosition);
            if (distanceFromThrownPosition <= 0.25f)
            {
                // Potion has hit the ground, loop through all the objects within the potion radius
                Collider[] objectsNearPotion = Physics.OverlapSphere(potionThrownPosition, potionRadius);
                foreach (Collider objectCollider in objectsNearPotion)
                {
                    // If the object inside the radius of the potion is a detected plant
                    PlantObject plantObject = objectCollider.transform.gameObject.GetComponent<PlantObject>();
                    if (plantObject != null)
                    {
                        ApplyPotionEffect(plantObject);
                    }
                }

                // Spawn the VFX of the potion explosion
                StartCoroutine(SpawnParticleEffect());

                // Destroy the 3D potion model falling from the sky
                Destroy(potionObject);
                potionObject = null;
            }
        }
    }

    // Each potion should have it's own original effect
    protected abstract void ApplyPotionEffect(PlantObject plantObject);

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        // Ignore the drag event if the player is dragging their mouse on any UI element in the game
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // Ignore the function if there is already a spawned potion that is still being dropped
        if (potionObject != null) return;

        // Ignore the function if the potion is on a cooldown
        if (!CanUsePotion()) return;

        // Create a ray from the mouse screen position into a 3D world position
        Vector3 mouseScreenPosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);

        // Look for the ground tagged object in the world to make sure we are throwing the potion on the ground and not on top of other objects
        if (Physics.Raycast(ray, out RaycastHit hit, 999.9f, LayerMask.GetMask("Ground")))
        {
            potionThrownPosition = hit.point;

            // Create a visual 3D object in the world that shows the drop location and radius of the potion
            if (potionPlaceholder == null)
            {
                potionPlaceholder = Instantiate(potionPlaceholderPrefab);

                Vector3 newScale = new Vector3(potionRadius, potionPlaceholder.transform.localScale.y, potionRadius);
                potionPlaceholder.transform.localScale = newScale;
            }
            
            potionPlaceholder.transform.position = potionThrownPosition;
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        // Check if there player has selected where they want to drop the potion
        if (potionPlaceholder != null)
        {
            // Spawn the potion and it's effects
            SpawnPotionObject();
            elapsedTimeSinceLastPotionUse = potionUseCooldown; // Reset the cooldown for the potion

            // Remove the visual object from the world
            Destroy(potionPlaceholder);
            potionPlaceholder = null;
        }
    }

    bool CanUsePotion()
    {
        return (elapsedTimeSinceLastPotionUse <= 0.0f);
    }
}
