using UnityEngine;

public class LanderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem centerThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;


    private void Update()
    {
        // leftThrusterParticleSystem.emission.enabled = false; emission has no set option
        ParticleSystem.EmissionModule emissionModule = leftThrusterParticleSystem.emission;
        emissionModule.enabled = false;
    }
}
