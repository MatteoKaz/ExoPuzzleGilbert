using UnityEngine;

public class Vent : MonoBehaviour
{
    public PlayerMovement movePlayer;
    public float oldmovePlayer;
    public float vitesseDePoussee = 0.25f;
    public int direction = 1; //1 = gauche, 2 = haut, 3 = droite, 4 = bas
    [SerializeField] public Vector3 direct;
    [SerializeField] public float ventDistance = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (direction == 1)
        {
            direct = new Vector3(0f, 0f, 1f);
        }
        else if (direction == 2)
        {
            direct = new Vector3(0f, 1f, 0f);
        }
        else if (direction == 3)
        {
            direct = new Vector3(0f, 0f, -1f);
        }
        else if (direction == 4)
        {
            direct = new Vector3(0f, -1f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(direct), out hit, ventDistance))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(direct) * hit.distance, Color.yellow);
            
            
            if (hit.transform.gameObject.GetComponent<PlayerMovement>() != null) 
            {
                movePlayer = hit.transform.gameObject.GetComponent<PlayerMovement>();
                if (direction == 1)
                {
                    if (movePlayer.speed < vitesseDePoussee)
                    {
                        oldmovePlayer = movePlayer.speed;
                        movePlayer.speed = vitesseDePoussee;
                    }
                }
                else if (direction == 2)
                {
                    hit.transform.gameObject.GetComponent<Impulseur>().AddImpact(direct,3f);

                }
            }
            else
            {
                if (direction == 1)
                {
                    if (movePlayer != null)
                    {
                        movePlayer.speed = oldmovePlayer;
                        movePlayer = null;
                    }
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(direct) * ventDistance, Color.white);
            
            if (direction == 1)
            {
                if (movePlayer != null)
                {
                    movePlayer.speed = oldmovePlayer;
                    movePlayer = null;
                }
            }
            
        }
    }
}
