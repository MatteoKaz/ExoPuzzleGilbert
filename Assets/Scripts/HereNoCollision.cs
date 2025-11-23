using UnityEngine;

public class HereNoCollision : MonoBehaviour
{
    public CapsuleCollider capsulePlayer;
    public GameObject player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<PlayerMovement>() != null)
        {
            capsulePlayer = collider.gameObject.GetComponent<CapsuleCollider>();
            player = collider.gameObject;
            player.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            return;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<PlayerMovement>() != null)
        {
            player = collider.gameObject;
            player.layer = LayerMask.NameToLayer("ShadowPlayer");
        }
        else
        {
            return;
        }
    }*/
}
