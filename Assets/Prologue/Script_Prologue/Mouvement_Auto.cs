using UnityEngine;

public class MoveOnSingleAxis : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Axis Selection")]
    public bool moveOnX = true;
    public bool moveOnY = false;
    public bool moveOnZ = false;

    [Header("Direction")]
    public bool positiveDirection = true;

    private Vector3 lockedPosition;

    void Start()
    {
        lockedPosition = transform.position;
    }

    void Update()
    {
        Vector3 newPosition = transform.position;

        float direction = positiveDirection ? 1f : -1f;
        float movement = direction * moveSpeed * Time.deltaTime;

        if (moveOnX)
        {
            newPosition.x += movement;
            newPosition.y = lockedPosition.y;
            newPosition.z = lockedPosition.z;
        }
        else if (moveOnY)
        {
            newPosition.y += movement;
            newPosition.x = lockedPosition.x;
            newPosition.z = lockedPosition.z;
        }
        else if (moveOnZ)
        {
            newPosition.z += movement;
            newPosition.x = lockedPosition.x;
            newPosition.y = lockedPosition.y;
        }

        transform.position = newPosition;
    }
}
