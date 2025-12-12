using JetBrains.Annotations;
using UnityEngine;
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

    [Header("Enemy Hit Cry Sounds")]
    [SerializeField] private AudioClip enemyHitCry1;
    [SerializeField] private AudioClip enemyHitCry2;
    [SerializeField] private AudioClip enemyHitCry3;
    [SerializeField] private AudioSource enemyCryAudioSource;

    [Header("Volume Settings")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float swordSoundVolume = 1f;
    [SerializeField] private float ennemyHitVolume = 1f;
    [SerializeField] private float reaction1Volume = 1f;
    [SerializeField] private float reaction2Volume = 1f;
    [SerializeField] private float enemyCryVolume = 1f;

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

        AudioClip[] cryClips = { enemyHitCry1, enemyHitCry2, enemyHitCry3 };
        AudioClip selectedClip = cryClips[Random.Range(0, cryClips.Length)];

        if (selectedClip != null)
        {
            enemyCryAudioSource.PlayOneShot(selectedClip, masterVolume * enemyCryVolume);
        }
    }
}
