using UnityEngine;

public class HoriRebond : MonoBehaviour
{
    [Header("Bounce Settings")]
    public float bounceForce = 5f;
    public LayerMask bounceableLayers;
    public bool debugMode = true;

    private Rigidbody rb;
    private PlayerMovement playerMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();

        if (bounceableLayers == 0)
        {
            bounceableLayers = LayerMask.GetMask("Cube", "Default");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & bounceableLayers) != 0)
        {
            ApplyHorizontalBounce(collision);
        }
    }

    void ApplyHorizontalBounce(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;

        float horizontalDot = Mathf.Abs(Vector3.Dot(normal, Vector3.forward));

        if (horizontalDot > 0.5f)
        {
            Vector3 bounceDirection = Vector3.Reflect(Vector3.forward * Mathf.Sign(rb.linearVelocity.z), normal);

            bounceDirection.y = 0;
            bounceDirection.Normalize();

            float currentSpeed = Mathf.Abs(rb.linearVelocity.z);
            Vector3 bounceVelocity = bounceDirection * bounceForce;

            if (playerMovement != null)
            {
              /*  playerMovement.recentCollision = true;
                playerMovement.collisionTimer = playerMovement.collisionPreserveTime;*/
            }

            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, bounceVelocity.z);

            if (debugMode)
            {
                Debug.Log($"Rebond horizontal appliqué! Force: {bounceVelocity.z}, Direction: {bounceDirection}");
            }
        }
    }
}
