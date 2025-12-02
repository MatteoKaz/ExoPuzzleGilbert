using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics.HapticsUtility;

public class PlayerMovement : MonoBehaviour
{
    // private Rigidbody rb;
    public Rigidbody rb;
    // private Vector3 moveDirection;
    public float speed = 0f;
    public float stopSpeed = 1f;
    //  private Vector3 Direction;
    private Animator _animator;
    public bool _isdead;
    public bool _isRevive;
    [SerializeField] AudioSource FootstepSound;
    public bool Footstep = false;

    private Vector3 velocity;
    public float gravity = -9.81f;
    public float gravityValue = 0.9f;
    public float verticalSpeed = 0f;
    public float starterAxisX;

    public float zMin = -Mathf.Infinity;
    public float zMax = Mathf.Infinity;

    public bool impulsionEnCours;
    public RaycastHit RaycastHit;
    public Vector3 SlopeNormal;
    public float SlopeAngle;
    public float maxSlopeAngle;
    public float playerheight;
    public LayerMask groundMask;
    [Header("Pente Settings")]
    public float rayDistance = 0.1f;
    private Vector3 moveDir = Vector3.back; // direction auto en -Z
    public bool Onslope;
    private Vector3 baseDirection = Vector3.back; // -Z direction
    public float forwardOffset = 0.3f;
    public float downwardOffset = 0.5f;
    public float tamere;
    public float speedOriginal;
    public Vector3 mouvementPente;
    public float yPente;
    public bool notOnGround;
    public float slopeMultiplier = 2f;
    public Vector3 v;
    public float forceHelper = 2.72f;
    public float maxHorizontalSpeed = 0.10f; // vitesse max
    public float maxVerticalSpeed = 0.10f;
    public float Offsetvalue = 0.5f;
    [Header("Step Settings")]
    public float stepHeight = 0.3f;      // hauteur des petits rebords
    public float stepLow = 0.3f;
    public float stepSmooth = 0.05f;     // puissance du step-up (vertical)
    public float stepCheckDistance = 0.4f; // rayon de détection devant
    public float stepCooldown = 0.1f;        // time between two steps

    private float stepTimer = 0f;
    public float stepMinHeight = 0.05f;        // lowest ray height
    public float stepMaxHeight = 0.35f;        // highest ray height
    public int stepVerticalRays = 3;           // number of vertical samples

    public float stepMinDistance = 0.15f;      // closest forward check
    public float stepMaxDistance = 0.45f;      // farthest forward check
    public int stepForwardRays = 3;            // number of forward samples
    public LayerMask mask;
    [Header("StepOnSlop Settings")]
    public float stepMinDistanceSlope = 0.15f;
    public float stepMaxDistanceSlope = 0.45f;
    public float stepMinHeightSlope = 0.05f;        // lowest ray height
    public float stepMaxHeightSlope = 0.35f;
    public float stepSmoothSlope = 2f;
    public float controlGravity =1f;

    public bool stucked = false;
    public LayerMask groundLayer;
    public LayerMask climbLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = 0f;
        rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _animator.SetFloat("Speed", -speed);
        starterAxisX = transform.position.x;
        mask = ~LayerMask.GetMask("Slope");
        mask += ~LayerMask.GetMask("Traversable");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IsGrounded();
       // CheckGroud();
        if (rb != null)
        {
            if (_animator != null)
            {
                _animator.SetFloat("Speed", -speed);
                _animator.SetBool("Dead", _isdead);
                _animator.SetBool("Revive", _isRevive);
            }
            
            v = rb.linearVelocity;
            




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
           // rb.useGravity = true;
        }
       

        {
            Vector3 slopeDir = AdjustDirectionToSlope(baseDirection);

            

            yPente = rb.linearVelocity.y;


            // Set velocity directly


            if (tamere > 18f && tamere < 70 && notOnGround == false)
            {
                
                Vector3 horizontalForce = new Vector3(slopeDir.x, slopeDir.y, slopeDir.z) * 4;
                rb.AddForce(horizontalForce * forceHelper, ForceMode.Acceleration);
                Vector3 horiz = new Vector3(v.x, 0f, speedOriginal);
                if (horiz.magnitude > maxHorizontalSpeed)
                {
                    horiz = horiz.normalized * maxHorizontalSpeed;
                    v.x = horiz.x;
                    v.z = horiz.z;
                    
                    ClimbOnSlope();

                }
                Offsetvalue = -0.028f;
                // 4. CLAMP VITESSE VERTICALE (évite de voler)
                if (v.y > maxVerticalSpeed)
                    v.y = maxVerticalSpeed;

                // appliquer le clamp
                rb.linearVelocity = v;
                if(speed ==0)
                {
                    rb.useGravity = false;
                    rb.AddForce(Physics.gravity * gravityValue*controlGravity, ForceMode.Acceleration);
                }
                else
                {
                    rb.useGravity = false;
                    rb.AddForce(Physics.gravity * 0.80f*controlGravity, ForceMode.Acceleration);
                    ClimbOnSlope();
                }
            }



            else
            {
                if(notOnGround ==false)
                {
                    //La suite est a mettre dans le build final.
                   // speed = speedOriginal;
                    //Debug.Log(speedOriginal);
                    rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, speed*stopSpeed);
                    Offsetvalue = 0.08f;
                    StepClimb();
                    rb.useGravity = false;
                    rb.AddForce(Physics.gravity * gravityValue*controlGravity, ForceMode.Acceleration);

                }
               else
                {
                    Offsetvalue = 0.08f;
                   
                }
               
                
            }
            if (tamere > 20f && tamere < 70 && slopeDir.y < 0 && notOnGround == false)
            {

                //Debug.Log("Descend");
                Vector3 horizontalForce = new Vector3(slopeDir.x, slopeDir.y, slopeDir.z) * 2.5f;
                if (speed == 0f)
                {
                    rb.AddForce(horizontalForce * -forceHelper * 4f, ForceMode.Acceleration);
                }
                else
                {
                    rb.AddForce(horizontalForce * -forceHelper, ForceMode.Acceleration);
                }
                  
                Vector3 horiz = new Vector3(v.x, 0f, speedOriginal);
                if (horiz.magnitude < maxHorizontalSpeed)
                {
                    horiz = horiz.normalized * maxHorizontalSpeed;
                    v.x = horiz.x;
                    v.z = horiz.z;
                    Offsetvalue = 0.0f;
                    rb.useGravity = true;
                    rb.AddForce(Physics.gravity * 1, ForceMode.Acceleration);

                }
                if (notOnGround == false)
                {
                    // 4. CLAMP VITESSE VERTICALE (évite de voler)
                    if (v.y < maxVerticalSpeed)
                        v.y = maxVerticalSpeed;

                    // appliquer le clamp
                    rb.linearVelocity = v;
                    rb.useGravity = true;
                    rb.AddForce(Physics.gravity * 1, ForceMode.Acceleration);
                }
            }   
            if (notOnGround != false)
            {
                
                
                rb.useGravity = false;
                //a remettre
                //speed = 0f;
                rb.AddForce(Physics.gravity * gravityValue * controlGravity, ForceMode.Acceleration);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z );

            }

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
            {
                if (hit.transform.gameObject.layer == 12)
                {
                    climbLayer += ~LayerMask.GetMask("Cube");
                    Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.magenta);
                }
                else
                {
                    climbLayer += LayerMask.GetMask("Cube");
                }
                
            }
            else
                {
                    climbLayer += LayerMask.GetMask("Cube");
                }


        }

        Vector3 AdjustDirectionToSlope(Vector3 dir)
        {
            Vector3 origin = new Vector3(transform.position.x, transform.position.y, transform.position.z - Offsetvalue);

            RaycastHit hit;

            if (Physics.Raycast(origin, Vector3.down, out hit, rayDistance))
            {
                
                Vector3 normal = hit.normal;
                Debug.DrawRay(origin, Vector3.down * rayDistance, Color.red);

                // project movement onto slope surface
                float slopeAngle = Vector3.Angle(normal, Vector3.up);
                Vector3 slopeDirection = Vector3.ProjectOnPlane(dir, normal).normalized;
            //    Debug.Log(slopeAngle);
                tamere = slopeAngle;

                return slopeDirection;
            }
            else
            {
                
            }

            // fallback if no ground hit
            return dir;

        }

    }


    public bool IsGrounded()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.19f, groundLayer);
        foreach (Collider collider in colliders)
        {
            //if (collider != null)
           // {
                notOnGround = false;
                return true;
           // }
        }
        notOnGround = true;
        return false;
        
    }
  /*  public void CheckGroud()
    {
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.28f))
        {
            
            notOnGround = false;
        }
        else
        {
            notOnGround = true;
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TraversableMur>())
        {
            stucked = true;
        }
        if (other.gameObject.GetComponent<TraversableBisControl>())
        {
            stopSpeed = 1f;
        }
        if (other.gameObject.GetComponent<HereNoCollision>())
        {
            gameObject.layer = LayerMask.NameToLayer("ShadowPlayerWithoutCube");
        }
        if (other.gameObject.GetComponent<Traversable>())
        {
            gameObject.layer = LayerMask.NameToLayer("ShadowPlayerTraversable");
        }



    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<TraversableMur>())
        {
            stucked = false;
            stopSpeed = 1f;
        }
        if (other.gameObject.GetComponent<TraversableBisControl>())
        {
            if (stucked)
            {
                stopSpeed = 0f;
            }
        }
        if (other.gameObject.GetComponent<HereNoCollision>())
        {
            gameObject.layer = LayerMask.NameToLayer("ShadowPlayer");
        }
        if (other.gameObject.GetComponent<Traversable>())
        {
            gameObject.layer = LayerMask.NameToLayer("ShadowPlayer");
        }

    }
    

    void StepClimb()
    {
        if (notOnGround == true)
            return;
        if (speed != 0)
        {
            // cooldown
            if (stepTimer > 0f && notOnGround == false)
            {
                stepTimer -= Time.deltaTime;
                return;
            }

            Vector3 origin = transform.position;
            Vector3 forward = transform.right;

            // vertical spacing
            float heightStep = 0f;
            if (stepVerticalRays > 1)
                heightStep = (stepMaxHeight - stepMinHeight) / (stepVerticalRays - 1);

            // forward spacing
            float distStep = 0f;
            if (stepForwardRays > 1)
                distStep = (stepMaxDistance - stepMinDistance) / (stepForwardRays - 1);

            bool canStep = false;
            if (notOnGround == false)
            {
                for (int h = 0; h < stepVerticalRays; h++)
                {
                    float yOffset = stepMinHeight + h * heightStep;
                    Vector3 lowOrigin = origin + Vector3.up * yOffset;

                    for (int d = 0; d < stepForwardRays; d++)
                    {
                        float dist = stepMinDistance + d * distStep;

                        // Low ray
                        Debug.DrawRay(lowOrigin, forward * dist, Color.green);
                        bool hitLow = Physics.Raycast(lowOrigin, forward, out RaycastHit hitLowInfo, dist, climbLayer);

                        if (hitLow && notOnGround == false)
                        {
                            // High ray
                            Vector3 highOrigin = origin + Vector3.up * stepMaxHeight;
                            Debug.DrawRay(highOrigin, forward * dist, Color.red);

                            bool hitHigh = Physics.Raycast(highOrigin, forward, dist);

                            // If top is clear -> step immediately
                            if (!hitHigh)
                            {
                                if (notOnGround == false)
                                {
                                    Vector3 dir = new Vector3(0, 0.75f, -0.15f);
                                    Debug.Log("je poussette ");
                                    rb.AddForce(dir * stepSmooth, ForceMode.Impulse);
                                    stepTimer = stepCooldown;
                                    return; // We are done. One hit only is enough.
                                }
                            }
                        }

                        if (canStep)
                            break;
                    }

                    if (canStep)
                    {
                        if (notOnGround == false)
                        {
                            Vector3 dir = new Vector3(0, 0.75f, -0.15f);
                            //Debug.Log("je pousse");
                            rb.AddForce(dir * stepSmooth, ForceMode.Impulse);
                            stepTimer = stepCooldown;
                        }

                    }
                }

            }

        }

    }

         void ClimbOnSlope()
         {
            if (notOnGround == true)
                return;
            if (speed != 0)
            {
                // cooldown
                if (stepTimer > 0f && notOnGround == false)
                {
                    stepTimer -= Time.deltaTime;
                    return;
                }

                Vector3 origin = transform.position;
                Vector3 forward = transform.right;

                // vertical spacing
                float heightStep = 0f;
                if (stepVerticalRays > 1)
                    heightStep = (stepMaxHeightSlope - stepMinHeightSlope) / (stepVerticalRays - 1);

                // forward spacing
                float distStep = 0f;
                if (stepForwardRays > 1)
                    distStep = (stepMaxDistanceSlope - stepMinDistanceSlope) / (stepForwardRays - 1);

                bool canStep = false;
                if (notOnGround == false)
                {
                    for (int h = 0; h < stepVerticalRays; h++)
                    {
                        float yOffset = stepMinHeightSlope + h * heightStep;
                        Vector3 lowOrigin = origin + Vector3.up * yOffset;

                        for (int d = 0; d < stepForwardRays; d++)
                        {
                            float dist = stepMinDistanceSlope + d * distStep;

                            // Low ray
                            Debug.DrawRay(lowOrigin, forward * dist, Color.green);
                            bool hitLow = Physics.Raycast(lowOrigin, forward, out RaycastHit hitLowInfo, dist, mask);

                            if (hitLow && notOnGround == false)
                            {
                                // High ray
                                Vector3 highOrigin = origin + Vector3.up * stepMaxHeightSlope;
                                Debug.DrawRay(highOrigin, forward * dist, Color.red);

                                bool hitHigh = Physics.Raycast(highOrigin, forward, dist, mask);

                                // If top is clear -> step immediately
                                if (!hitHigh)
                                {
                                    if (notOnGround == false)
                                    {
                                        Vector3 dir = new Vector3(0, 1, -0.15f);
                                        Debug.Log("je poussette ");
                                        rb.AddForce(dir * stepSmoothSlope, ForceMode.Impulse);
                                        stepTimer = stepCooldown;
                                        return; // We are done. One hit only is enough.
                                    }
                                }
                            }

                            if (canStep)
                                break;
                        }

                        if (canStep)
                        {
                            if (notOnGround == false)
                            {
                                Vector3 dir = new Vector3(0, 1, -0.15f);
                                //Debug.Log("je pousse");
                                rb.AddForce(dir * stepSmoothSlope, ForceMode.Impulse);
                                stepTimer = stepCooldown;
                            }

                        }
                    }

                }

            }
        }

            /*
            public void ForcePlayer()
            {
                bool impulsionEnCours = true;
                StartCoroutine(UpImpulse());
               // StartCoroutine(ThrowPlayer()); 
            }

            IEnumerator ThrowPlayer()
            {
                float throwDistance = 500f;
                Vector3 upVector = new Vector3(0, 0.0001f, 0);
                Vector3 throwVector;
                for (int i = 0; i <= throwDistance; i++)
                {
                    if (i <= throwDistance / 2)
                    {
                        throwVector = transform.forward + upVector;
                    }
                    else
                    {
                        throwVector = transform.forward - upVector;
                    }
                    controller.Move(throwVector);
                    if (!controller.isGrounded) yield return new WaitForFixedUpdate();
                }
                StopAllCoroutines();
            }

            IEnumerator UpImpulse()
            {
                float throwDistance = 250f;
                Vector3 upVector = new Vector3(0, 0.0001f, 0);
                Vector3 throwVector;
                for (int i = 0; i <= throwDistance; i++)
                {
                    if (i <= throwDistance / 2)
                    {
                        throwVector = transform.forward + upVector;
                    }
                    else
                    {
                        if (impulsionEnCours) 
                        { 

                        }
                        throwVector = transform.forward - upVector;
                    }
                    controller.Move(throwVector);
                    if (!controller.isGrounded) yield return new WaitForFixedUpdate();
                }

            }
            IEnumerator DownImpulse()
            {
                float throwDistance = 250f;
                Vector3 upVector = new Vector3(0, 0.0001f, 0);
                Vector3 throwVector;
                for (int i = 0; i <= throwDistance; i++)
                {
                    throwVector = transform.forward - upVector;

                    controller.Move(throwVector);
                    if (!controller.isGrounded) yield return new WaitForFixedUpdate();
                }

            }
            */
        
}
