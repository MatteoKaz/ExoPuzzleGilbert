using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Clips")]
    public AudioClip musicA; // Instrumentale A (niveaux 1 à 4)
    public AudioClip musicB; // Version plus épique / avec choeur (niveau 5)
    public AudioClip musicC; // Musique des niveaux 6+ (autre ambiance)

    [Header("Réglages")]
    public float crossfadeDuration = 3f;
    public float baseVolume = 1f;

    private AudioSource sourceMain;
    private AudioSource sourceAlt;

    // 1 = A, 2 = B, 3 = C
    private int currentPhase = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Création des deux AudioSources pour les fondus
        sourceMain = gameObject.AddComponent<AudioSource>();
        sourceAlt  = gameObject.AddComponent<AudioSource>();

        sourceMain.loop = true;
        sourceAlt.loop  = true;

        sourceMain.playOnAwake = false;
        sourceAlt.playOnAwake  = false;

        sourceMain.volume = baseVolume;
        sourceAlt.volume  = 0f;

        // On écoute le changement de scène
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Lancer la musique en fonction de la scène actuelle
        Scene scene = SceneManager.GetActiveScene();
        UpdateMusicForScene(scene.buildIndex);
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMusicForScene(scene.buildIndex);
    }

    /// <summary>
    /// Choisit la bonne musique selon le numéro de niveau.
    /// À adapter si tes buildIndex ne correspondent pas.
    /// </summary>
    private void UpdateMusicForScene(int buildIndex)
    {
        // ⚠️ Adapte ces conditions si ton Level 1 n'est pas buildIndex 1.
        // Ici on suppose :
        //   - Niveaux 1 à 4  -> musique A
        //   - Niveau 5       -> musique B
        //   - Niveaux 6 et + -> musique C

        if (buildIndex >= 1 && buildIndex <= 4)
        {
            // Niveaux 1 -> 4 : A
            if (currentPhase != 1)
            {
                PlayOrCrossfadeTo(musicA, 1);
            }
        }
        else if (buildIndex == 5)
        {
            // Niveau 5 : crossfade vers B
            if (currentPhase != 2)
            {
                PlayOrCrossfadeTo(musicB, 2);
            }
        }
        else if (buildIndex >= 6)
        {
            // Niveaux 6+ : musique C
            if (currentPhase != 3)
            {
                PlayOrCrossfadeTo(musicC, 3);
            }
        }
    }

    private void PlayOrCrossfadeTo(AudioClip targetClip, int targetPhase)
    {
        if (targetClip == null)
        {
            Debug.LogWarning("[MusicManager] Aucun clip assigné pour cette phase.");
            return;
        }

        // Si rien ne joue encore, on démarre directement
        if (!sourceMain.isPlaying && !sourceAlt.isPlaying)
        {
            sourceMain.clip = targetClip;
            sourceMain.volume = baseVolume;
            sourceMain.Play();
            currentPhase = targetPhase;
            return;
        }

        // Sinon : crossfade de sourceMain vers targetClip sur sourceAlt
        StartCoroutine(CrossfadeRoutine(targetClip, targetPhase, crossfadeDuration));
    }

    private IEnumerator CrossfadeRoutine(AudioClip targetClip, int targetPhase, float duration)
    {
        // sourceAlt prépare la prochaine musique
        sourceAlt.clip = targetClip;
        sourceAlt.volume = 0f;
        sourceAlt.Play();

        float time = 0f;
        float startVolMain = sourceMain.volume;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            sourceMain.volume = Mathf.Lerp(startVolMain, 0f, t);
            sourceAlt.volume  = Mathf.Lerp(0f, baseVolume, t);

            yield return null;
        }

        // On termine proprement
        sourceMain.Stop();
        sourceMain.volume = baseVolume;

        // On swap les sources : la nouvelle devient la principale
        AudioSource temp = sourceMain;
        sourceMain = sourceAlt;
        sourceAlt  = temp;

        currentPhase = targetPhase;
    }
}
