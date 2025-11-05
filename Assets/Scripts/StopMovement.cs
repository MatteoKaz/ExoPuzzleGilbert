using UnityEngine;

public class StopMovement : MonoBehaviour
{
    public Rigidbody rb;
    

    public void StopInertie()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
    } 
}
