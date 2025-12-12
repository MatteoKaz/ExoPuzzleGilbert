using UnityEngine;
using UnityEngine.ProBuilder;

public class HoriRebond : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceForce = 5f;
    public LayerMask bounceableLayers;
    public LayerMask moreBounceLayers;
    public bool debugMode = true;

    [Header("Deceleration Settings")]
    public bool useCustomDeceleration = true;
    public float customDecelerationRate = 2f;
    public float bounceDecelerationMultiplier = 1.5f;
    public float decelerationDuration = 2f;
    public float minVelocityThreshold = 0.1f;

    private Rigidbody rb;
    private PlayerMovement playerMovement;
    private float decelerationTimer = 0f;
    public Vector3 normal;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();

        if (bounceableLayers == 0)
        {
            bounceableLayers = LayerMask.GetMask("Cube", "Default", "Slope");
        }
    }

    public void SetDeceleration()
    {
        decelerationTimer = 0f;
    }


    private void FixedUpdate()
    {
        if (useCustomDeceleration && decelerationTimer > 0f)
        {
            decelerationTimer -= Time.fixedDeltaTime;
        }
    }

    private void Update()
    {
    }

    void OnCollisionEnter(Collision collision)
    {
        if (enabled == true)
        {
            if (((1 << collision.gameObject.layer) & moreBounceLayers) != 0)
            {
                decelerationTimer = decelerationDuration;
            }
            if (((1 << collision.gameObject.layer) & bounceableLayers) != 0)
            {
                ApplyBounce(collision);
            }
        }
    }

    void ApplyBounce(Collision collision)
    {
        if (collision.contactCount > 1)
        {
            Vector3 averageNormal = Vector3.zero;
            foreach (ContactPoint contact in collision.contacts)
            {
                averageNormal += contact.normal;
            }
            normal = (averageNormal / collision.contacts.Length).normalized;
        }
        else if (collision.contactCount == 0)
        {
            Debug.Log("Aucun point de col ????");
        }
        else
        {
            normal = collision.GetContact(0).normal;
        }
        
        Vector3 currentVelocity = rb.linearVelocity;

        

        Vector3 horizontalVelocity = new Vector3(0, currentVelocity.y, currentVelocity.z);
        Vector3 bounceDirection;

        if (decelerationTimer <= 0)
        {
            decelerationTimer = decelerationDuration;
        }

       // if (horizontalVelocity.magnitude > 0.1f)
     //   {
        //    bounceDirection = Vector3.Reflect(horizontalVelocity.normalized, normal);
      //  }
    //    else
       // {
            bounceDirection = new Vector3(0, normal.y, normal.z);
      //  }

        bounceDirection.x = 0;
        bounceDirection.Normalize();

        Vector3 newVelocity = bounceDirection * bounceForce;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, newVelocity.y, newVelocity.z)*(decelerationTimer/(decelerationDuration/2));


        if (debugMode)
        {
            Debug.DrawRay(transform.position, bounceDirection * 2f, Color.magenta, 1f);
           // Debug.Log($"Other Collider : {collision.gameObject.name} vitesse : {horizontalVelocity.magnitude:F2}, direction: {bounceDirection}, nouvelle velocite: {rb.linearVelocity}");
        }
    }



   /* void ApplyDeceleration()
    {
        Vector3 currentVelocity = rb.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(0, currentVelocity.y, currentVelocity.z);

        if (horizontalVelocity.magnitude > minVelocityThreshold)
        {
            float effectiveDeceleration = customDecelerationRate * bounceDecelerationMultiplier;
            Vector3 deceleration = -horizontalVelocity.normalized * effectiveDeceleration * Time.fixedDeltaTime;
            Debug.Log(deceleration);
            Vector3 newHorizontalVelocity = horizontalVelocity + deceleration;


            rb.linearVelocity = new Vector3(currentVelocity.x, newHorizontalVelocity.y, newHorizontalVelocity.z);
        }
        else
        {
            if (!playerMovement.notOnGround)
            {
                rb.linearVelocity = new Vector3(currentVelocity.x, 0, 0);
                decelerationTimer = 0f;
                Debug.Log("STOP");
            }
        }
    }*/
}
