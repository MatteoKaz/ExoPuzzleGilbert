using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] public GameObject toFollow;
    private Vector3 dir;
    void Start()
    {
        dir = transform.forward * 2;
    }

    void Update()
    {
        transform.position = toFollow.transform.position + dir;
    }
}
