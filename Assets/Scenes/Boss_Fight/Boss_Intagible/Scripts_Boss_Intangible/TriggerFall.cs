using UnityEngine;

public class TriggerFall : MonoBehaviour
{
    public Rigidbody2D cubeRigidBody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cubeRigidBody.gravityScale = 1; // Active la gravité → le cube tombe
        }
    }
}
