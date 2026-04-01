using System;
using UnityEngine;

public class LanderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftThrusterParticleSystem;
    [SerializeField] private ParticleSystem centerThrusterParticleSystem;
    [SerializeField] private ParticleSystem rightThrusterParticleSystem;
    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();
        //attach a listener to an event
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;
        SetEnableThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnableThrusterParticleSystem(centerThrusterParticleSystem,false);
        SetEnableThrusterParticleSystem(rightThrusterParticleSystem, false);
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(leftThrusterParticleSystem, false);
        SetEnableThrusterParticleSystem(centerThrusterParticleSystem, false);
        SetEnableThrusterParticleSystem(rightThrusterParticleSystem, false);
    }
    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(rightThrusterParticleSystem, true);
    }
    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(leftThrusterParticleSystem, true);
    }
    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        SetEnableThrusterParticleSystem(centerThrusterParticleSystem, true);
    }

    private void SetEnableThrusterParticleSystem(ParticleSystem particleSystem, bool enabled)
    {
        // leftThrusterParticleSystem.emission.enabled = false; emission has no set option
        // This would actually work at runtime (since the copy holds the reference). But the C# compiler blocks it with an error.
        // 1. .emission is a property that returns a struct (value type)
        // 2. You're immediately setting .enabled on that returned struct
        // 3. The compiler assumes that modifying a temporary struct copy is a mistake — the copy would be discarded right after
        // The compiler doesn't know that EmissionModule's setter has the special native trick. It applies a general safety rule: don't let developers modify temporary structs, because 99% of the time it's a bug
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        // It's not a runtime problem — it's the C# compiler being protective. The workaround (local variable) is just to make the compiler happy.
        emissionModule.enabled = enabled;
    }
}
