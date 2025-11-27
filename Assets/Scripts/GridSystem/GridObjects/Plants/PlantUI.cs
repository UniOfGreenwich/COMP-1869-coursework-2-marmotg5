using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlantUI : MonoBehaviour
{
    [Header("UI Naming")]
    [SerializeField] string healthPrefix = "Health: ";
    [SerializeField] string waterPrefix = "Water: ";
	[SerializeField] string growingRemainingPrefix = "Remaining: ";

	[Header("UI Texts")]
    [SerializeField] TextMeshProUGUI plantNameText;
    [SerializeField] TextMeshProUGUI plantHealthText;
    [SerializeField] TextMeshProUGUI plantWaterLevelText;
    [SerializeField] TextMeshProUGUI plantTimeLeftText;

    [Header("UI Images")]
    [SerializeField] Image plantHealthBarImage;
    [SerializeField] Image plantWaterBarImage;
    [SerializeField] Image plantTimeLeftImage;

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

    public void BindUIButtons(PlantObject plantObject)
    {
        if (plantObject == null) return;

        clearPestsButton.onClick.AddListener(plantObject.ClearPestDamage);
        waterPlantButton.onClick.AddListener(() => plantObject.WaterPlant(plantObject.GetPlantWaterGainAmount()));
        harvestPlantButton.onClick.AddListener(plantObject.HarvestPlant);
    }

    public void UnbindUIButtons(PlantObject plantObject)
    {
		if (plantObject == null) return;

		clearPestsButton.onClick.RemoveListener(plantObject.ClearPestDamage);
		waterPlantButton.onClick.RemoveListener(() => plantObject.WaterPlant(plantObject.GetPlantWaterGainAmount()));
        harvestPlantButton.onClick.RemoveListener(plantObject.HarvestPlant);

	}

    // Updates the data on the UI based on the plant object passed in the function
    public void UpdatePlantUIData(PlantObject plantObject)
    {
		if (plantObject == null) return;

		plantNameText.text = plantObject.GetPlantName();


        UpdatePlantHealthBar(plantObject);
        UpdatePlantWaterBar(plantObject);
        UpdatePlantTimeLeftBar(plantObject);
    }

    void UpdatePlantHealthBar(PlantObject plantObject)
    {
        float progressImageBarValue = (float)plantObject.GetPlantHealth() / (float)plantObject.GetPlantData().maxHealth;
        print("plant health : " + progressImageBarValue);
        plantHealthText.text = healthPrefix + plantObject.GetPlantHealth();
        plantHealthBarImage.fillAmount = progressImageBarValue;
    }

    void UpdatePlantWaterBar(PlantObject plantObject)
    {
        float progressImageBarValue = (plantObject.GetPlantWaterLevel() / PlantObject.MAX_WATER_LEVEL);
        plantWaterLevelText.text = waterPrefix + plantObject.GetPlantWaterLevel().ToString("F0") + "%";
        plantWaterBarImage.fillAmount = progressImageBarValue;
    }

    void UpdatePlantTimeLeftBar(PlantObject plantObject)
    {
        // Update the timer left for the plant to fully grow
        float plantTimeLeft = plantObject.GetPlantData().requiredGrowingTime - plantObject.GetPlantCurrentGrowingTime();
        float progressImageBarValue = plantObject.GetPlantCurrentGrowingTime() / plantObject.GetPlantData().requiredGrowingTime;

        plantTimeLeftText.text = growingRemainingPrefix + Math.Round(plantTimeLeft).ToString() + "s";
        plantTimeLeftImage.fillAmount = progressImageBarValue;
    }
}
