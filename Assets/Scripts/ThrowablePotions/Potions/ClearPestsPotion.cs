using UnityEngine;

public class ClearPestsPotion : ThrowablePotion
{
    protected override void ApplyPotionEffect(PlantObject plantObject)
    {
        plantObject.ClearPestDamage();
    }
}
