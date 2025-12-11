using UnityEngine;

public class StopPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null)
        {
            PlayerMovement movement = other.GetComponent<PlayerMovement>();
            HoriRebond rebond = other.GetComponent<HoriRebond>();

            rebond.SetDeceleration();
            rebond.enabled = false;
            movement.rb.linearVelocity = Vector3.zero;
            movement.NewGravity();
            
        }
    }
}
