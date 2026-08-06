using System;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private const int MUSIC_VOLUME_MAX = 10;
    private static int musicVolume = 6;

    private static float musicTime;

    private AudioSource musicAudioSource;

    public event EventHandler OnMusicVolumeChanged;

    private void Awake()
    {
        Instance = this;

        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.time = musicTime;
    }

    private void Update()
    {
        // we track musicTime on every update, so when new scene loads (MainMenu or GameOver), we don't play from beginning
        musicTime = musicAudioSource.time;
    }

    public void ChangeMusicVolume()
    {
        musicVolume = (musicVolume + 1) % MUSIC_VOLUME_MAX;
        musicAudioSource.volume = GetMusicVolumeNormalized();
        OnMusicVolumeChanged?.Invoke(this, EventArgs.Empty); //we are not using this for now, but might be useful later
    }

    public int GetMusicVolume()
    {
        return musicVolume;
    }
    public float GetMusicVolumeNormalized()
    {
        return ((float)musicVolume) / MUSIC_VOLUME_MAX;
    }
}
