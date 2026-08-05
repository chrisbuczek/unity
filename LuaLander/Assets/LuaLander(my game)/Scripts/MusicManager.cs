using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    private const int VOLUME_MAX = 5;

    private static float musicTime;
    private static int volume = VOLUME_MAX; // static so it persists across scenes, same reasoning as musicTime

    private AudioSource musicAudioSource;

    private void Awake()
    {
        Instance = this;

        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.time = musicTime;
        ApplyVolume();
    }

    private void Update()
    {
        // we track musicTime on every update, so when new scene loads (MainMenu or GameOver), we don't play from beginning
        musicTime = musicAudioSource.time;
    }

    public void VolumeUp()
    {
        volume = Mathf.Min(volume + 1, VOLUME_MAX);
        ApplyVolume();
    }

    public void VolumeDown()
    {
        volume = Mathf.Max(volume - 1, 0);
        ApplyVolume();
    }

    public int GetVolume()
    {
        return volume;
    }

    private void ApplyVolume()
    {
        musicAudioSource.volume = (float)volume / VOLUME_MAX;
    }
}
