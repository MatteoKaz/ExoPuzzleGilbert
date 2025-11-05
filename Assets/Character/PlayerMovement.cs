using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 0f;
    private Vector3 Direction;
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
         speed = 0f;
        _animator = GetComponent<Animator>();
        _animator.SetFloat("Speed", -speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
        {
            _animator.SetFloat("Speed", -speed);
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, speed);
            
           
        }



    }
}
