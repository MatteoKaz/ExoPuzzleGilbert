using UnityEngine;
using System.Collections;
using System.Globalization;

public class Death : MonoBehaviour
{
    private Vector3 RespawnLoc;
    [SerializeField] public GameObject Character;
    private float _deathTimer = 0.5f;
    private SpriteRenderer _sprite;
    private Animator _animator;
    private PlayerMovement _PM;
    
    [Header("Death Sounds")]
    [SerializeField] private AudioClip deathSound1;
    [SerializeField] private AudioClip deathSound2;
    [SerializeField] private AudioClip deathSound3;
    [SerializeField] private AudioSource deathAudioSource;
    
    [Header("Respawn Sound")]
    [SerializeField] private AudioClip respawnSound;
    
    private bool isDying = false;
    private Coroutine deathRoutine;
    private Collider col;

    void Start()
    {
        if (Character != null)
        {
            RespawnLoc = Character.transform.position;
            RespawnLoc = new Vector3(RespawnLoc.x, RespawnLoc.y, RespawnLoc.z);
        }
    }

    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Character != null)
        {
            if (isDying) return;
            if (other.gameObject.GetComponent<PlayerMovement>() != null)
            {
                _PM = other.gameObject.GetComponent<PlayerMovement>();
                isDying = true;
                Dead();
            }
        }
    }

    public void Dead()
    {
        if (deathRoutine == null)
        {
            deathRoutine = StartCoroutine(ShadowTimerControl());
        }
    }

    private void PlayRandomDeathSound()
    {
        if (deathAudioSource == null) return;
        
        AudioClip[] deathClips = { deathSound1, deathSound2, deathSound3 };
        AudioClip selectedClip = deathClips[Random.Range(0, deathClips.Length)];
        
        if (selectedClip != null)
        {
            deathAudioSource.PlayOneShot(selectedClip);
        }
    }

    private void PlayRespawnSound()
    {
        if (deathAudioSource != null && respawnSound != null)
        {
            deathAudioSource.PlayOneShot(respawnSound);
        }
    }

    private IEnumerator ShadowTimerControl()
    {
        if (Character != null)
        {
            _PM._isdead = true;
            _sprite = Character.gameObject.GetComponent<SpriteRenderer>();
            _PM.speed = 0;
            _PM.rb.isKinematic = true;
            col = Character.GetComponent<CapsuleCollider>();
            col.enabled = false;

            PlayRandomDeathSound();
        }

        yield return new WaitForSeconds(1.25f);
        if (Character != null)
        {
            _sprite.enabled = false;
            _PM._isdead = false;
            if (deathAudioSource != null)
                deathAudioSource.Stop();
            Character.gameObject.GetComponent<PlayerMovement>().speed = 0f;
        }
        yield return new WaitForSeconds(0.25f);

        if (Character != null)
        {
            _PM.rb.constraints = RigidbodyConstraints.None;
            Character.transform.position = RespawnLoc;

            _PM.rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
        }
        yield return new WaitForSeconds(0.25f);
        if (Character != null)
        {
            col.enabled = true;
            _PM.rb.isKinematic = false;
            Debug.Log("tp");
            _sprite.enabled = true;
            
            PlayRespawnSound();
        }
        yield return new WaitForSeconds(0.75f);
        {
            isDying = false;
            _PM._isRevive = false;
            deathRoutine = null;
        }
    }
}
