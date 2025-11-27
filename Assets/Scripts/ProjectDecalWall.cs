using UnityEngine;

public class ProjectDecalWall : MonoBehaviour
{
    [SerializeField] public GameObject ToFollow;
    void Update()
    {
        if (ToFollow != null)
        {
            Vector3 positionToFollow = ToFollow.transform.position;
            Vector3 newPosition = new Vector3(18.93f,positionToFollow.y,positionToFollow.z);
        }
    }
}
