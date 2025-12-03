using UnityEngine;

public class PrologueMusicManager : MonoBehaviour
{
    public static PrologueMusicManager Instance;

    [Header("Audio Sources")]
    public AudioSource prologueMusic;   // Musique 1
    public AudioSource mainMusic;       // Musique 2

    [Header("Réglages de fondu")]
    public float fadeDuration = 2f;

    private bool isCrossfading = false;
    private bool hasSwitched = false;
    private float crossfadeTimer = 0f;

    private void Awake()
    {
        // Singleton : 1 seul manager dans toute la game
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
        if (prologueMusic == null || mainMusic == null)
        {
            Debug.LogError("PrologueMusicManager : merci d'assigner prologueMusic et mainMusic.");
            enabled = false;
            return;
        }

        prologueMusic.loop = true;
        mainMusic.loop = true;

        if (!hasSwitched)
        {
            prologueMusic.volume = 1f;
            mainMusic.volume = 0f;

            prologueMusic.Play();
            mainMusic.Play();
        }
        else
        {
            // Si la musique avait déjà switché et qu'on revient depuis une autre scène
            prologueMusic.volume = 0f;
            mainMusic.volume = 1f;

            prologueMusic.Play();
            mainMusic.Play();
        }
    }

    private void Update()
    {
        if (isCrossfading)
        {
            HandleCrossfade();
        }
    }

    public void TriggerSwitchToMain()
    {
        if (hasSwitched || isCrossfading)
            return;

        isCrossfading = true;
        crossfadeTimer = 0f;

        // Pour repartir du début si voulu :
        // mainMusic.time = 0f;
    }

    private void HandleCrossfade()
    {
        crossfadeTimer += Time.deltaTime;
        float t = Mathf.Clamp01(crossfadeTimer / fadeDuration);

        prologueMusic.volume = 1f - t;
        mainMusic.volume = t;

        if (t >= 1f)
        {
            isCrossfading = false;
            hasSwitched = true;

            prologueMusic.volume = 0f;
            mainMusic.volume = 1f;

            prologueMusic.Stop();
        }
    }
}
