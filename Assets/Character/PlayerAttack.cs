using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    public Animator _animator;
    public bool _Attack = false;
    public bool HasAttack = false;
    public PlayerMovement PM;
    public bool _EnemyGO = false;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip SwordSound;
    [SerializeField] private AudioSource EnnemyHit;
    [SerializeField] private AudioSource Reaction1;
    [SerializeField] private AudioSource Reaction2;

    [Header("Enemy Hit Cry Sounds - Default (Prologue/Boss)")]
    [SerializeField] private AudioClip enemyHitCry1;
    [SerializeField] private AudioClip enemyHitCry2;
    [SerializeField] private AudioClip enemyHitCry3;

    [Header("Enemy Hit Cry Sounds - Act1")]
    [SerializeField] private AudioClip enemyHitCryAct1_1;
    [SerializeField] private AudioClip enemyHitCryAct1_2;
    [SerializeField] private AudioClip enemyHitCryAct1_3;

    [Header("Enemy Hit Cry Sounds - Act2")]
    [SerializeField] private AudioClip enemyHitCryAct2_1;
    [SerializeField] private AudioClip enemyHitCryAct2_2;
    [SerializeField] private AudioClip enemyHitCryAct2_3;

    [Header("Enemy Hit Cry Sounds - Act3")]
    [SerializeField] private AudioClip enemyHitCryAct3_1;
    [SerializeField] private AudioClip enemyHitCryAct3_2;
    [SerializeField] private AudioClip enemyHitCryAct3_3;

    [SerializeField] private AudioSource enemyCryAudioSource;

    [Header("Volume Settings")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float swordSoundVolume = 1f;
    [SerializeField] private float ennemyHitVolume = 1f;
    [SerializeField] private float reaction1Volume = 1f;
    [SerializeField] private float reaction2Volume = 1f;
    [SerializeField] private float enemyCryVolume = 1f;
    [SerializeField] private float enemyCryVolumeAct1 = 1f;
    [SerializeField] private float enemyCryVolumeAct2 = 1f;
    [SerializeField] private float enemyCryVolumeAct3 = 1f;

    private AudioSource audioSource;

    void Start()
    {
        _animator = GetComponent<Animator>();
        PM = gameObject.GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        _animator.SetBool("Attack", _Attack);
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = volume;
    }

    public void SetSwordSoundVolume(float volume)
    {
        swordSoundVolume = volume;
    }

    public void SetEnnemyHitVolume(float volume)
    {
        ennemyHitVolume = volume;
    }

    public void SetReaction1Volume(float volume)
    {
        reaction1Volume = volume;
    }

    public void SetReaction2Volume(float volume)
    {
        reaction2Volume = volume;
    }

    public void SetEnemyCryVolume(float volume)
    {
        enemyCryVolume = volume;
    }

    public void SetEnemyCryVolumeAct1(float volume)
    {
        enemyCryVolumeAct1 = volume;
    }

    public void SetEnemyCryVolumeAct2(float volume)
    {
        enemyCryVolumeAct2 = volume;
    }

    public void SetEnemyCryVolumeAct3(float volume)
    {
        enemyCryVolumeAct3 = volume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyMovement>() != null)
        {
            if (HasAttack == false)
            {
                Debug.Log("J'attaque");
                StartCoroutine(Anim());
            }
        }
    }

    private IEnumerator Anim()
    {
        if (HasAttack == false)
        {
            PM.speed = 0f;
            HasAttack = true;
            _Attack = true;

            if (SwordSound != null && audioSource != null)
                audioSource.PlayOneShot(SwordSound, masterVolume * swordSoundVolume);

            yield return new WaitForSeconds(0.25f);

            if (EnnemyHit != null && EnnemyHit.clip != null)
                EnnemyHit.PlayOneShot(EnnemyHit.clip, masterVolume * ennemyHitVolume);

            yield return new WaitForSeconds(0.1f);

            PlayRandomEnemyHitCry();

            yield return new WaitForSeconds(1.5f);

            if (Reaction1 != null && Reaction1.clip != null)
                Reaction1.PlayOneShot(Reaction1.clip, masterVolume * reaction1Volume);

            _Attack = false;
            yield return new WaitForSeconds(0.5f);
            PM.speed = -0.5f;
            _EnemyGO = true;
        }
    }

    private void PlayRandomEnemyHitCry()
    {
        if (enemyCryAudioSource == null) return;

        string currentSceneName = SceneManager.GetActiveScene().name;
        AudioClip[] cryClips;
        float actVolume;

        if (currentSceneName.StartsWith("Acte1"))
        {
            cryClips = new AudioClip[] { enemyHitCryAct1_1, enemyHitCryAct1_2, enemyHitCryAct1_3 };
            actVolume = enemyCryVolumeAct1;
        }
        else if (currentSceneName.StartsWith("Acte2"))
        {
            cryClips = new AudioClip[] { enemyHitCryAct2_1, enemyHitCryAct2_2, enemyHitCryAct2_3 };
            actVolume = enemyCryVolumeAct2;
        }
        else if (currentSceneName.StartsWith("Acte3"))
        {
            cryClips = new AudioClip[] { enemyHitCryAct3_1, enemyHitCryAct3_2, enemyHitCryAct3_3 };
            actVolume = enemyCryVolumeAct3;
        }
        else
        {
            cryClips = new AudioClip[] { enemyHitCry1, enemyHitCry2, enemyHitCry3 };
            actVolume = enemyCryVolume;
        }

        AudioClip selectedClip = cryClips[Random.Range(0, cryClips.Length)];

        if (selectedClip != null)
        {
            enemyCryAudioSource.PlayOneShot(selectedClip, masterVolume * actVolume);
        }
    }
}
