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
    [SerializeField] AudioSource DeathSound;
    private bool isDying = false;
    private Coroutine deathRoutine;
    private Collider col;
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

        }
        yield return new WaitForSeconds(0.75f);
        {
            isDying = false;
            _PM._isRevive = false;
            deathRoutine = null;
        }
        
    }
}
