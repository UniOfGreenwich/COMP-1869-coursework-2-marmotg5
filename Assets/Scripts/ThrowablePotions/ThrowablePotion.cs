using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ThrowablePotion : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Throwable Potion Object Prefab")]
    [SerializeField, Tooltip("The actual object that gets spawned on top of the zone into the world")]
    GameObject potionObjectPrefab;
    [SerializeField, Tooltip("How hight from the ground should the potion be dropped at")]
    float dropHeight = 3.0f;
    [SerializeField]
    float dropSpeed = 2.5f;
    GameObject potionObject = null;

    [Header("Potion Zone Prefab")]
    [SerializeField, Tooltip("The visual object that follows the drag of the player's input")]
    GameObject potionPlaceholderPrefab;
    GameObject potionPlaceholder = null;
    Vector3 potionThrownPosition = Vector3.zero;

    [Header("Potion Settings")]
    [SerializeField] float potionRadius = 5.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DropPotion(); // Only if it's spawned
    }

    void SpawnPotionObject()
    {
        if (potionObjectPrefab != null)
        {
            potionObject = Instantiate(potionObjectPrefab);
            potionObject.transform.position = potionThrownPosition + new Vector3(0.0f, dropHeight, 0.0f); // Set the spawn position of the new potion
        }
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
                print("Potion hit ground");
                Destroy(potionObject);
                potionObject = null;
            }
        }
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        // Ignore the drag event if the player is dragging their mouse on any UI element in the game
        if (EventSystem.current.IsPointerOverGameObject()) return;

        // Ignore the function if there is already a spawned potion that is still being dropped
        if (potionObject != null) return;

        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI
        // ADD A SIMPLE SERIALIZEFIELD COOLDOWN THAT ALSO UPDATES ON THE UI

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

            // Remove the visual object from the world
            Destroy(potionPlaceholder);
            potionPlaceholder = null;
        }
    }
}
