using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] public GameObject toFollow;
    private Vector3 dir;
    void Start()
    {
        dir = transform.forward;
    }

    void FixedUpdate()
    {
        transform.position = toFollow.transform.position;
    }
}
