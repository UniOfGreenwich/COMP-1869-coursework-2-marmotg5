using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlantUI : MonoBehaviour
{
    PlantObject plantObject = null;

    [Header("UI Naming")]
    [SerializeField] string healthPrefix = "Health: ";
    [SerializeField] string waterPrefix = "Water: ";

    [SerializeField] TextMeshProUGUI plantHealthText;
    [SerializeField] TextMeshProUGUI plantWaterLevelText;

    [Header("UI Buttons")]
    [SerializeField] Button clearPestsButton;
    [SerializeField] Button waterPlantButton;
    [SerializeField] Button harvestPlantButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Updates the data on the UI based on the plant object passed in the function
    public void UpdatePlantUIData(PlantObject plantObject)
    {
        plantHealthText.text = healthPrefix + plantObject.GetPlantHealth();

        // MAKE AN ENTIRE FUNCTION THAT ALSO BINDS BUTTON CLICK EVENTS AND ALSO UPDATE PLANT UI DATA
        // MAKE AN ENTIRE FUNCTION THAT ALSO BINDS BUTTON CLICK EVENTS AND ALSO UPDATE PLANT UI DATA
        // MAKE AN ENTIRE FUNCTION THAT ALSO BINDS BUTTON CLICK EVENTS AND ALSO UPDATE PLANT UI DATA
        // MAKE AN ENTIRE FUNCTION THAT ALSO BINDS BUTTON CLICK EVENTS AND ALSO UPDATE PLANT UI DATA
        // MAKE AN ENTIRE FUNCTION THAT ALSO BINDS BUTTON CLICK EVENTS AND ALSO UPDATE PLANT UI DATA
        // MAKE AN ENTIRE FUNCTION THAT ALSO BINDS BUTTON CLICK EVENTS AND ALSO UPDATE PLANT UI DATA
        // MAKE AN ENTIRE FUNCTION THAT ALSO BINDS BUTTON CLICK EVENTS AND ALSO UPDATE PLANT UI DATA
        // MAKE AN ENTIRE FUNCTION THAT ALSO BINDS BUTTON CLICK EVENTS AND ALSO UPDATE PLANT UI DATA
        // MAKE AN ENTIRE FUNCTION THAT ALSO BINDS BUTTON CLICK EVENTS AND ALSO UPDATE PLANT UI DATA
        // MAKE AN ENTIRE FUNCTION THAT ALSO BINDS BUTTON CLICK EVENTS AND ALSO UPDATE PLANT UI DATA

        plantWaterLevelText.text = waterPrefix + plantObject.GetPlantWaterLevel().ToString("F0") + "%";
    }

    public void DestroyUI()
    {
        Destroy(gameObject);
    }
}
