using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public GameObject fixedPoint;
    public Vector3 fixedPosition;
    public Vector3 direction;
    public Quaternion newQuat;

    private void Start()
    {
        
    }


    void Update()
    {
        if (target == null) 
        { 
            return; 
        }

        //   transform.position = fixedPosition;
        //    transform.forward = target.forward;

        float currentZ = transform.eulerAngles.z;

        

        fixedPosition = fixedPoint.transform.position;
     //   direction = (fixedPosition - target.position).normalized;
        transform.LookAt(fixedPosition);
        Vector3 euler = transform.eulerAngles;
        euler.z = currentZ;
        transform.eulerAngles = euler;

    }

    //Faire plutôt raycast
}
