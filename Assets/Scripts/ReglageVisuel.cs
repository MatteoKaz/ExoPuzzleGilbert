using UnityEngine;

public class ReglageVisuel : MonoBehaviour
{
    private CharacterController controller;

    [Tooltip("Distance minimale au sol avant correction")]
    public float groundOffset = 0.05f;

    [Tooltip("Layer du sol")]
    public LayerMask groundMask;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void LateUpdate()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        if (Physics.SphereCast(origin, controller.radius, Vector3.down, out hit, controller.height / 2 + 0.5f, groundMask))
        {
            float distanceToGround = hit.distance - controller.height / 2;
            if (distanceToGround > groundOffset)
            {
                controller.Move(Vector3.down * (distanceToGround - groundOffset));
            }
        }
    }
}

