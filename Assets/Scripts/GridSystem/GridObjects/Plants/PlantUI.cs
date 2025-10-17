using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlantUI : MonoBehaviour
{
    [Header("UI Naming")]
    [SerializeField] string healthPrefix = "Health: ";
    [SerializeField] string waterPrefix = "Water: ";

    [SerializeField] TextMeshProUGUI plantHealthText;
    [SerializeField] TextMeshProUGUI plantWaterText;

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
}
