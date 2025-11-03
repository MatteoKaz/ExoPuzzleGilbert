using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public Transform door;
    public float openHeight = 3f;
    public float openSpeed = 2f;
    
    [Header("Player Tag")]
    public string playerTag = "Player";
    
    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;
    private bool isMoving = false;

    private void Start()
    {
        if (door != null)
        {
            closedPosition = door.position;
            openPosition = closedPosition + Vector3.up * openHeight;
        }
    }

    private void Update()
    {
        if (isMoving && door != null)
        {
            Vector3 targetPosition = isOpen ? openPosition : closedPosition;
            door.position = Vector3.MoveTowards(door.position, targetPosition, openSpeed * Time.deltaTime);
            
            if (Vector3.Distance(door.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isOpen = true;
            isMoving = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isOpen = false;
            isMoving = true;
        }
    }
}

