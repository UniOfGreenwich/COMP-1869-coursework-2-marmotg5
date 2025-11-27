using UnityEngine;

public class WaterPlantsPotion : ThrowablePotion
{
    protected override void ApplyPotionEffect(PlantObject plantObject)
    {
        plantObject.WaterPlant(100.0f); // Water it to the maximum
    }
}
