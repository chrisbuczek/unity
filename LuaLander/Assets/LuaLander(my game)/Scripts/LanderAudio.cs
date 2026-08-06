using System;
using UnityEngine;

public class LanderAudio : MonoBehaviour
{
    [SerializeField] private AudioSource thrusterAudioSource;

    private Lander lander;
    // private bool isThrusterAudioPlaying = false; no need to that, there already is thrusterAudioSource.isPlaying;

    private void Awake()
    {
        // tighter/preferred choice when you're already on the same object, since it doesn't depend on the singleton being initialized first.
        lander = GetComponent<Lander>();
    }

    private void Start()
    {
        lander.OnBeforeForce += Lander_OnBeforeForce;
        lander.OnUpForce += Lander_OnUpForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnLeftForce += Lander_OnLeftForce;

        SoundManager.Instance.OnSoundVolumeChanged += SoundManager_OnSoundVolumeChanged;

        // thrusterAudioSource.enabled = false; this will always play from the start, that is why we change to Pause()
        thrusterAudioSource.Pause();
    }

    private void SoundManager_OnSoundVolumeChanged(object sender, EventArgs e)
    {
        thrusterAudioSource.volume = SoundManager.Instance.GetSoundVolumeNormalized();
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        // thrusterAudioSource.enabled = false;
        thrusterAudioSource.Pause();
    }

    private void Lander_OnUpForce(object sender, System.EventArgs e)
    {
        // thrusterAudioSource.enabled = true;
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        // thrusterAudioSource.enabled = true;
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        // thrusterAudioSource.enabled = true;
        if (!thrusterAudioSource.isPlaying)
        {
            thrusterAudioSource.Play();
        }
    }
}
