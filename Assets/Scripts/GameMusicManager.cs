using UnityEngine;
using System.Collections;

public class GameMusicManager : MonoBehaviour
{
    public static GameMusicManager Instance;

    [System.Serializable]
    public class MusicTrack
    {
        public string trackName;
        public AudioSource audioSource;
    }

    [Header("Music Tracks")]
    public MusicTrack[] tracks;

    [Header("Settings")]
    public float fadeDuration = 2f;

    private int currentTrackIndex = -1;
    private bool isCrossfading = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        foreach (var track in tracks)
        {
            if (track.audioSource != null)
            {
                track.audioSource.loop = true;
                track.audioSource.volume = 0f;
            }
        }

        if (tracks.Length > 0)
        {
            PlayTrack(0, true);
        }
    }

    public void SwitchToTrack(string trackName)
    {
        int targetIndex = -1;
        for (int i = 0; i < tracks.Length; i++)
        {
            if (tracks[i].trackName == trackName)
            {
                targetIndex = i;
                break;
            }
        }

        if (targetIndex == -1)
        {
            Debug.LogWarning($"Track '{trackName}' not found in GameMusicManager.");
            return;
        }

        if (targetIndex == currentTrackIndex)
        {
            return;
        }

        if (!isCrossfading)
        {
            StartCoroutine(CrossfadeToTrack(targetIndex));
        }
    }

    public void FadeOutCurrent()
    {
        if (currentTrackIndex >= 0 && !isCrossfading)
        {
            StartCoroutine(FadeOutTrack(currentTrackIndex));
        }
    }

    private void PlayTrack(int index, bool immediate = false)
    {
        if (index < 0 || index >= tracks.Length)
            return;

        currentTrackIndex = index;
        var track = tracks[index];

        if (track.audioSource != null)
        {
            if (!track.audioSource.isPlaying)
            {
                track.audioSource.Play();
            }

            if (immediate)
            {
                track.audioSource.volume = 1f;
            }
        }
    }

    private IEnumerator CrossfadeToTrack(int targetIndex)
    {
        isCrossfading = true;

        int previousIndex = currentTrackIndex;
        AudioSource previousSource = previousIndex >= 0 ? tracks[previousIndex].audioSource : null;
        AudioSource targetSource = tracks[targetIndex].audioSource;

        if (targetSource != null && !targetSource.isPlaying)
        {
            targetSource.Play();
        }

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            if (previousSource != null)
            {
                previousSource.volume = 1f - t;
            }

            if (targetSource != null)
            {
                targetSource.volume = t;
            }

            yield return null;
        }

        if (previousSource != null)
        {
            previousSource.volume = 0f;
            previousSource.Stop();
        }

        if (targetSource != null)
        {
            targetSource.volume = 1f;
        }

        currentTrackIndex = targetIndex;
        isCrossfading = false;
    }

    private IEnumerator FadeOutTrack(int index)
    {
        isCrossfading = true;

        AudioSource source = tracks[index].audioSource;
        if (source == null)
        {
            isCrossfading = false;
            yield break;
        }

        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            source.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        source.volume = 0f;
        source.Stop();
        currentTrackIndex = -1;
        isCrossfading = false;
    }
}
