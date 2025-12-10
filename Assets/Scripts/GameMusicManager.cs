using UnityEngine;
using System.Collections;

public class GameMusicManager : MonoBehaviour
{
    private static GameMusicManager instance;
    public static GameMusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("GameMusicManager");
                instance = go.AddComponent<GameMusicManager>();
                DontDestroyOnLoad(go);
                Debug.Log("[GameMusicManager] Instance créée automatiquement.");
            }
            return instance;
        }
    }

    [System.Serializable]
    public class MusicTrack
    {
        public string trackName;
        public AudioClip audioClip;
    }

    [Header("Music Tracks")]
    public MusicTrack[] tracks;

    [Header("Settings")]
    public float fadeDuration = 2f;

    private AudioSource sourceMain;
    private AudioSource sourceAlt;
    private int currentTrackIndex = -1;
    private bool isCrossfading = false;
    private bool isInitialized = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("[GameMusicManager] Instance créée et persistante entre les scènes.");
    }

    private void Initialize()
    {
        if (isInitialized) return;

        sourceMain = gameObject.AddComponent<AudioSource>();
        sourceAlt = gameObject.AddComponent<AudioSource>();

        sourceMain.loop = true;
        sourceAlt.loop = true;
        sourceMain.volume = 0f;
        sourceAlt.volume = 0f;

        isInitialized = true;
        Debug.Log("[GameMusicManager] AudioSources initialisés.");
    }

    private void Start()
    {
        Initialize();

        if (tracks != null && tracks.Length > 0 && tracks[0].audioClip != null)
        {
            PlayTrack(0, true);
            Debug.Log($"[GameMusicManager] Première piste lancée : {tracks[0].trackName}");
        }
        else
        {
            Debug.LogWarning("[GameMusicManager] Aucune piste ou clip disponible au démarrage.");
        }
    }

    public void SwitchToTrack(string trackName)
    {
        Initialize();

        Debug.Log($"[GameMusicManager] Demande de changement vers la piste : '{trackName}' (piste actuelle index: {currentTrackIndex})");

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
            Debug.LogWarning($"[GameMusicManager] Piste '{trackName}' NON TROUVÉE. Pistes disponibles :");
            for (int i = 0; i < tracks.Length; i++)
            {
                Debug.LogWarning($"  - '{tracks[i].trackName}'");
            }
            return;
        }

        if (tracks[targetIndex].audioClip == null)
        {
            Debug.LogWarning($"[GameMusicManager] La piste '{trackName}' n'a pas de clip audio assigné.");
            return;
        }

        if (targetIndex == currentTrackIndex)
        {
            Debug.Log($"[GameMusicManager] La piste '{trackName}' est déjà en cours de lecture (index {currentTrackIndex}).");
            return;
        }

        Debug.Log($"[GameMusicManager] Changement de piste {currentTrackIndex} vers {targetIndex} ('{trackName}')");

        if (currentTrackIndex == -1)
        {
            PlayTrack(targetIndex, true);
            Debug.Log($"[GameMusicManager] Première piste démarrée directement : {trackName}");
        }
        else if (!isCrossfading)
        {
            StartCoroutine(CrossfadeToTrack(targetIndex));
        }
        else
        {
            Debug.LogWarning("[GameMusicManager] Crossfade déjà en cours, changement ignoré.");
        }
    }

    public void FadeOutCurrent()
    {
        if (currentTrackIndex >= 0 && !isCrossfading)
        {
            StartCoroutine(FadeOutCurrentTrack());
        }
    }

    private void PlayTrack(int index, bool immediate = false)
    {
        if (index < 0 || index >= tracks.Length || tracks[index].audioClip == null)
            return;

        sourceMain.clip = tracks[index].audioClip;
        sourceMain.Play();

        if (immediate)
        {
            sourceMain.volume = 1f;
        }

        currentTrackIndex = index;
        Debug.Log($"[GameMusicManager] Piste {index} ({tracks[index].trackName}) en lecture. CurrentTrackIndex = {currentTrackIndex}");
    }

    private IEnumerator CrossfadeToTrack(int targetIndex)
    {
        isCrossfading = true;
        Debug.Log($"[GameMusicManager] Début du crossfade vers piste {targetIndex} ({tracks[targetIndex].trackName})");

        AudioClip targetClip = tracks[targetIndex].audioClip;

        sourceAlt.clip = targetClip;
        sourceAlt.Play();

        Debug.Log($"[GameMusicManager] Démarrage de la nouvelle piste");

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;

            sourceMain.volume = 1f - t;
            sourceAlt.volume = t;

            yield return null;
        }

        sourceMain.volume = 0f;
        sourceMain.Stop();

        AudioSource temp = sourceMain;
        sourceMain = sourceAlt;
        sourceAlt = temp;

        Debug.Log($"[GameMusicManager] Ancienne piste arrêtée");

        currentTrackIndex = targetIndex;
        isCrossfading = false;

        Debug.Log($"[GameMusicManager] Crossfade terminé. Piste actuelle : {tracks[targetIndex].trackName} (index {currentTrackIndex})");
    }

    private IEnumerator FadeOutCurrentTrack()
    {
        isCrossfading = true;

        float startVolume = sourceMain.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            sourceMain.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        sourceMain.volume = 0f;
        sourceMain.Stop();
        currentTrackIndex = -1;
        isCrossfading = false;
    }
}
