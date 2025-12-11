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

    [Header("Scene Music Mapping")]
    [SerializeField] private List<SceneMusic> sceneMusicMap = new List<SceneMusic>();

    [Header("Settings")]
    [SerializeField] private float crossfadeDuration = 2f;

    private AudioSource sourceA;
    private AudioSource sourceB;
    private AudioSource currentSource;
    private AudioSource nextSource;
    private bool isCrossfading = false;
    private string currentSceneName = "";

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

        sourceA.loop = true;
        sourceB.loop = true;
        sourceA.volume = 0f;
        sourceB.volume = 0f;

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

    private void PlayMusic(AudioClip newClip)
    {
        if (currentSource.clip == newClip && currentSource.isPlaying)
        {
            return;
        }

        if (!currentSource.isPlaying)
        {
            currentSource.clip = newClip;
            currentSource.volume = 1f;
            currentSource.Play();
        }
        else
        {
            if (!isCrossfading)
            {
                StartCoroutine(CrossfadeToClip(newClip));
            }
        }
    }

    private IEnumerator CrossfadeToClip(AudioClip newClip)
    {
        isCrossfading = true;

        nextSource.clip = newClip;
        nextSource.Play();

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

        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            currentSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / crossfadeDuration);
            yield return null;
        }

        currentSource.volume = 0f;
        currentSource.Stop();
    }
}
