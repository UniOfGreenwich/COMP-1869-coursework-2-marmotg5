using UnityEngine;

public class GrowthVFX : MonoBehaviour
{
    private ParticleSystem fullyGrownVFX;

    void Start()
    {
        fullyGrownVFX = GetComponentInChildren<ParticleSystem>();
    }

    
}
