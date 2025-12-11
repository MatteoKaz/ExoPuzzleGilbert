using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public static MusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("MusicManager");
                instance = go.AddComponent<MusicManager>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    [System.Serializable]
    public class LinkedMusicPair
    {
        public AudioClip mainTrack;
        public AudioClip linkedTrack;
    }

    [Header("Scene Music Mapping")]
    [SerializeField] private List<SceneMusic> sceneMusicMap = new List<SceneMusic>();

    [Header("Linked Music Pairs")]
    [Tooltip("Paires de musiques qui jouent en sync (la linkedTrack démarre en même temps que mainTrack mais à volume 0)")]
    [SerializeField] private List<LinkedMusicPair> linkedMusicPairs = new List<LinkedMusicPair>();

    [Header("Settings")]
    [SerializeField] private float crossfadeDuration = 2f;

    private AudioSource sourceA;
    private AudioSource sourceB;
    private AudioSource linkedSource;

    private AudioSource currentSource;
    private AudioSource nextSource;

    private bool isCrossfading = false;
    private string currentSceneName = "";
    private AudioClip currentLinkedClip = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        sourceA = gameObject.AddComponent<AudioSource>();
        sourceB = gameObject.AddComponent<AudioSource>();
        linkedSource = gameObject.AddComponent<AudioSource>();

        sourceA.loop = true;
        sourceB.loop = true;
        linkedSource.loop = true;

        sourceA.volume = 0f;
        sourceB.volume = 0f;
        linkedSource.volume = 0f;

        currentSource = sourceA;
        nextSource = sourceB;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == currentSceneName)
            return;

        currentSceneName = scene.name;

        AudioClip clipToPlay = GetMusicForScene(scene.name);

        if (clipToPlay != null)
        {
            PlayMusic(clipToPlay);
        }
    }

    private AudioClip GetMusicForScene(string sceneName)
    {
        foreach (SceneMusic sceneMusic in sceneMusicMap)
        {
            if (sceneMusic.sceneName == sceneName)
            {
                return sceneMusic.musicClip;
            }
        }
        return null;
    }

    private AudioClip GetLinkedTrack(AudioClip mainClip)
    {
        foreach (LinkedMusicPair pair in linkedMusicPairs)
        {
            if (pair.mainTrack == mainClip)
            {
                return pair.linkedTrack;
            }
            if (pair.linkedTrack == mainClip)
            {
                return pair.mainTrack;
            }
        }
        return null;
    }

    private void PlayMusic(AudioClip newClip)
    {
        if (currentSource.clip == newClip && currentSource.isPlaying)
        {
            return;
        }

        if (currentLinkedClip != null && currentLinkedClip == newClip && linkedSource.isPlaying)
        {
            if (!isCrossfading)
            {
                StartCoroutine(CrossfadeBetweenLinkedTracks());
            }
            return;
        }

        if (!currentSource.isPlaying)
        {
            currentSource.clip = newClip;
            currentSource.volume = 1f;
            currentSource.Play();

            AudioClip linkedClip = GetLinkedTrack(newClip);
            if (linkedClip != null)
            {
                linkedSource.clip = linkedClip;
                linkedSource.volume = 0f;
                linkedSource.Play();
                currentLinkedClip = linkedClip;
            }
            else
            {
                currentLinkedClip = null;
            }
        }
        else
        {
            if (!isCrossfading)
            {
                StartCoroutine(CrossfadeToClip(newClip));
            }
        }
    }

    private IEnumerator CrossfadeBetweenLinkedTracks()
    {
        isCrossfading = true;

        float elapsed = 0f;

        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / crossfadeDuration;

            currentSource.volume = Mathf.Lerp(1f, 0f, t);
            linkedSource.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        currentSource.volume = 0f;
        linkedSource.volume = 1f;

        AudioSource temp = currentSource;
        currentSource = linkedSource;
        linkedSource = temp;

        AudioClip tempClip = currentLinkedClip;
        currentLinkedClip = linkedSource.clip;

        isCrossfading = false;
    }

    private IEnumerator CrossfadeToClip(AudioClip newClip)
    {
        isCrossfading = true;

        if (linkedSource.isPlaying)
        {
            linkedSource.Stop();
            linkedSource.volume = 0f;
        }

        AudioClip newLinkedClip = GetLinkedTrack(newClip);

        nextSource.clip = newClip;
        nextSource.Play();

        if (newLinkedClip != null)
        {
            linkedSource.clip = newLinkedClip;
            linkedSource.volume = 0f;
            linkedSource.Play();
            currentLinkedClip = newLinkedClip;
        }
        else
        {
            currentLinkedClip = null;
        }

        float elapsed = 0f;

        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / crossfadeDuration;

            currentSource.volume = Mathf.Lerp(1f, 0f, t);
            nextSource.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        currentSource.volume = 0f;
        currentSource.Stop();
        nextSource.volume = 1f;

        AudioSource temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;

        isCrossfading = false;
    }

    public void StopMusic()
    {
        StartCoroutine(FadeOutCurrentMusic());
    }

    private IEnumerator FadeOutCurrentMusic()
    {
        float elapsed = 0f;
        float startVolume = currentSource.volume;
        float startLinkedVolume = linkedSource.volume;

        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / crossfadeDuration;

            currentSource.volume = Mathf.Lerp(startVolume, 0f, t);

            if (linkedSource.isPlaying)
            {
                linkedSource.volume = Mathf.Lerp(startLinkedVolume, 0f, t);
            }

            yield return null;
        }

        currentSource.volume = 0f;
        currentSource.Stop();

        if (linkedSource.isPlaying)
        {
            linkedSource.volume = 0f;
            linkedSource.Stop();
        }
    }
}
