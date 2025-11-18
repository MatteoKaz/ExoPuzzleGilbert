using UnityEngine;

public class Vent : MonoBehaviour
{
    public PlayerMovement movePlayer;
    public float oldmovePlayer;
    public float vitesseDePoussee = 0.25f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("touchee");
            
            if (hit.transform.gameObject.GetComponent<PlayerMovement>() != null) 
            {
                movePlayer = hit.transform.gameObject.GetComponent<PlayerMovement>();
                if (movePlayer.speed < vitesseDePoussee)
                {
                    oldmovePlayer = movePlayer.speed;
                    movePlayer.speed = vitesseDePoussee;
                }
            }
            else
            {
                if (movePlayer != null)
                {
                    movePlayer.speed = oldmovePlayer;
                    movePlayer = null;
                }
                
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("pas touchee");

            if (movePlayer != null)
            {
                movePlayer.speed = oldmovePlayer;
                movePlayer = null;
            }
        }
    }
}
