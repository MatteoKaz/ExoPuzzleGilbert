using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;

public class PlayerMovement : MonoBehaviour
{
   // private Rigidbody rb;
    private CharacterController controller;
   // private Vector3 moveDirection;
    public float speed = 0f;
  //  private Vector3 Direction;
    private Animator _animator;
    public bool _isdead;
    public bool _isRevive;
    [SerializeField] AudioSource FootstepSound;
    public bool Footstep = false;

    private Vector3 velocity;       
    public float gravity = -9.81f;  
    public float verticalSpeed = 0f;
    public float starterAxisX;

    public float zMin = -Mathf.Infinity;
    public float zMax = Mathf.Infinity;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 0f;
        controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _animator.SetFloat("Speed", -speed);
        starterAxisX = transform.position.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (controller != null)
        {
            _animator.SetFloat("Speed",-speed);
            _animator.SetBool("Dead", _isdead);
            _animator.SetBool("Revive", _isRevive);
    
            
            if (controller.isGrounded)
            {
                verticalSpeed = -1f; 
            }
            else
            {
                verticalSpeed = Mathf.Clamp(verticalSpeed + gravity * Time.deltaTime, -1, 20); 
            }


            Vector3 move = new Vector3(0f, verticalSpeed, speed) * Time.deltaTime;

            float newZ = Mathf.Clamp(controller.transform.position.z + move.z, zMin, zMax);
            move.z = newZ - controller.transform.position.z;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hit, 0.1f))
            {
                move.z = 0;
            } // OUI CE PUTAIN DE CODE MARCHE ENFIN
            controller.Move(move);
        }

        if (speed != 0f)
        {
            if (Footstep == false)
            {
                Footstep = true;
                FootstepSound.Play();
            }
            
        }
        else
        {
            Footstep = false;
            FootstepSound.Stop();
        }

    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = starterAxisX; 
        transform.position = pos;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.GetComponent<HereNoCollision>()!= null)
        {
            gameObject.layer = LayerMask.NameToLayer("ShadowPlayerWithoutCube");
        }
        
           
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<HereNoCollision>() != null)
        {
            gameObject.layer = LayerMask.NameToLayer("ShadowPlayer");
        }
        
    }

}
