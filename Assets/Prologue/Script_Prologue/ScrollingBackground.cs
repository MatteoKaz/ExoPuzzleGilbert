using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float scrollSpeed = 2f;
    public Vector3 scrollDirection = new Vector3(-1, 0, 0);
    public float resetDistance = 10f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position += scrollDirection * scrollSpeed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= resetDistance)
        {
            transform.position = startPosition;
        }
    }
}
