using UnityEngine;
using System.Collections;
using System.Globalization;


public class Death : MonoBehaviour
{
    private Vector3 RespawnLoc;
    [SerializeField] private GameObject Character;
    private float _deathTimer = 0.5f;
    private SpriteRenderer _sprite;
    private Animator _animator;
    private PlayerMovement _PM;
    [SerializeField] AudioSource DeathSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Character != null)
        {
            RespawnLoc = Character.transform.position;
            RespawnLoc = new Vector3(RespawnLoc.x, RespawnLoc.y, RespawnLoc.z);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (Character != null)
        {
            if (other.gameObject.GetComponent<PlayerMovement>() != null)
            {
                _PM = other.gameObject.GetComponent<PlayerMovement>();
                Dead();
            }
        }
          
    }

    public void Dead()
    {
        StartCoroutine(ShadowTimerControl());

    }
        

    private IEnumerator ShadowTimerControl()
    {
        if (Character != null)
        {
            _PM._isdead = true;
            _sprite = Character.gameObject.GetComponent<SpriteRenderer>();
            _PM.speed = 0;
            DeathSound.Play();
        }

        yield return new WaitForSeconds(1.25f);
        if (Character != null)
        {
            _sprite.enabled = false;
            _PM._isdead = false;
            DeathSound.Stop();
            Character.gameObject.GetComponent<PlayerMovement>().speed = 0f;
        }
        yield return new WaitForSeconds(0.25f);

        if (Character != null)
        {
            Character.transform.position = RespawnLoc;
            _sprite.enabled = true;
        }
        yield return new WaitForSeconds(1f);
        if (Character != null)
        {
            _PM._isRevive = false;
        }
        
    }
}
