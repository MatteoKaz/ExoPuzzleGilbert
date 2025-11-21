using UnityEngine;

public class FollowPlateform : MonoBehaviour
{
    public Transform plateformTransform;
    public float rayDistance = 0.5f;
    private Rigidbody rb;
    public Vector3 localOffset;
    private bool onPlatform;
    public PlayerMovement pm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
            pm = GetComponent<PlayerMovement>();    
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (onPlatform)
        {
            if (pm.speed ==0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, plateformTransform.position.z + localOffset.z);
            }
           
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            if(hit.transform.gameObject != null && hit.transform.gameObject.CompareTag("Plateform"))
            {
                plateformTransform = hit.collider.transform;
                localOffset = transform.position - plateformTransform.position;
                onPlatform = true;
                onPlatform = true;
            }
        }
        else
        {
            onPlatform = false;
        }

    }
}

    
       
    

