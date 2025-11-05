using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float runSpeed = 3f;

    [Header("Destruction Settings")]
    [SerializeField] private float destroyDelay = 2f;

    [Header("Animation")]
    private Animator animator;
    
    private bool isRunning = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        if (player == null)
        {
            player = GameObject.Find("Idle_0")?.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            RunAway();
        }
        else
        {
            StopRunning();
        }
    }

    void RunAway()
    {
        if (!isRunning)
        {
            isRunning = true;
            animator.Play("Enemy_Walk");
        }

        Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;
        transform.Translate(directionAwayFromPlayer * runSpeed * Time.deltaTime, Space.World);
        
        if (directionAwayFromPlayer.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void StopRunning()
    {
        if (isRunning)
        {
            isRunning = false;
            animator.Play("Enemy_IDLE");
        }
    }

    void OnBecameInvisible()
    {
        if (isRunning)
        {
            Destroy(gameObject, destroyDelay);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}