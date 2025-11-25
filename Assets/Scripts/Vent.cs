using UnityEngine;

public class Vent : MonoBehaviour
{
    public PlayerMovement movePlayer;
    public float oldmovePlayer;
    public float vitesseDePoussee = 0.25f;
    public int direction = 1;
    [SerializeField] public Vector3 direct;
    [SerializeField] public Vector3 directPush;
    [SerializeField] public float ventDistance = 2f;
    private bool _bCummulation = false;
    public Rigidbody rb;
    
    [Header("Wind Sound")]
    [SerializeField] private AudioSource windAudioSource;
    [SerializeField] private AudioClip windSound;
    [SerializeField] private float windVolume = 0.5f;

    void Start()
    {
        if (direction == 1)
        {
            direct = new Vector3(0f, 0f, 1f);
            directPush = new Vector3(0f, 0.0f, 1f);
        }
        else if (direction == 2)
        {
            direct = new Vector3(0f, 1f, 0f);
            directPush = new Vector3(0f, 1f, 0f);
        }
        else if (direction == 3)
        {
            direct = new Vector3(0f, 0f, -1f);
            directPush = new Vector3(0f, 0.0001f, -1f);
        }
        else if (direction == 4)
        {
            direct = new Vector3(0f, -1f, 0f);
            directPush = new Vector3(0f, -1f, 0f);
        }
        
        SetupWindSound();
    }
    
    private void SetupWindSound()
    {
        if (windAudioSource != null && windSound != null)
        {
            windAudioSource.clip = windSound;
            windAudioSource.loop = true;
            windAudioSource.volume = windVolume;
            windAudioSource.Play();
        }
    }

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
                rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(directPush * vitesseDePoussee, ForceMode.Acceleration);
            }
            else
            {
                if (direction == 1 || direction == 3)
                {
                    if (movePlayer != null)
                    {
                        
                        movePlayer = null;
                        _bCummulation = false;
                    }
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(direct) * ventDistance, Color.white);
            
            if (direction == 1 || direction == 3)
            {
                if (movePlayer != null)
                {
                    
                    movePlayer = null;
                    _bCummulation = false;
                }
            }
        }
    }
}
