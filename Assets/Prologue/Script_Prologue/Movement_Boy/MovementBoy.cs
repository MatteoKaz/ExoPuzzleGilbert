using UnityEngine;

public class AutoRunner : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool useLocalDirection = true;
    [SerializeField] private Vector3 moveDirection = Vector3.right;

    [Header("Start Delay")]
    [SerializeField] private float startDelay = 0.5f;

    [Header("Optional - Stop After Duration")]
    [SerializeField] private float runDuration = 0f;

    private bool isRunning = false;
    private float runTimer = 0f;

    private void Start()
    {
        Invoke(nameof(StartRunning), startDelay);
    }

    private void Update()
    {
        if (!isRunning) return;

        Vector3 movement;

        if (useLocalDirection)
        {
            movement = transform.TransformDirection(moveDirection.normalized) * moveSpeed * Time.deltaTime;
        }
        else
        {
            movement = moveDirection.normalized * moveSpeed * Time.deltaTime;
        }

        transform.position += movement;

        if (runDuration > 0)
        {
            runTimer += Time.deltaTime;
            if (runTimer >= runDuration)
            {
                StopRunning();
            }
        }
    }

    private void StartRunning()
    {
        isRunning = true;
    }

    public void StopRunning()
    {
        isRunning = false;
    }
}

