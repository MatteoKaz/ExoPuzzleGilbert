using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] public GameObject toFollow; 
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = toFollow.transform.position;
    }
}
