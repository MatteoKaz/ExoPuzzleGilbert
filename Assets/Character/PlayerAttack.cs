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
    [SerializeField] private AudioSource SwordSound;
    [SerializeField] private AudioSource EnnemyHit;
    [SerializeField] private AudioSource Reaction1;
    [SerializeField] private AudioSource Reaction2;
    
    [Header("Enemy Hit Cry Sounds")]
    [SerializeField] private AudioClip enemyHitCry1;
    [SerializeField] private AudioClip enemyHitCry2;
    [SerializeField] private AudioClip enemyHitCry3;
    [SerializeField] private AudioSource enemyCryAudioSource;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        PM = gameObject.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        _animator.SetBool("Attack", _Attack);
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
            SwordSound.Play();
            
            Reaction1.Play();
            
            yield return new WaitForSeconds(0.75f);
            EnnemyHit.Play();
            
            PlayRandomEnemyHitCry();

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
            enemyCryAudioSource.PlayOneShot(selectedClip);
        }
    }
}
