using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] public Transform target;
    [SerializeField] public GameObject fixedPoint;
    public Vector3 fixedPosition;

    private void Start()
    {
        fixedPosition = fixedPoint.transform.position;
    }


    void Update()
    {
        if (target == null) 
        { 
            return; 
        }

        transform.position = fixedPosition;
        transform.forward = target.forward;
    }

    //Faire plutôt raycast
}
