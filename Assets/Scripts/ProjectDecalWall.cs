using UnityEngine;

public class ProjectDecalWall : MonoBehaviour
{
    [SerializeField] public GameObject ToFollow;
    void Update()
    {
            Vector3 newPosition = new Vector3(18.93f,transform.position.y,transform.position.z);
            transform.position = newPosition;
    }
}
