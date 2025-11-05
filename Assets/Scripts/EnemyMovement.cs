using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float runSpeed = 3f;

    [Header("Destruction Settings")]
    [SerializeField] private float destroyDelay = 5f;

    [Header("Animation")]
    private Animator animator;
    
    private bool isRunning = false;
    private bool isDestroying = false;

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

        float directionX = transform.position.x - player.position.x;
        
        if (directionX < 0)
        {
            transform.Translate(Vector3.left * runSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.right * runSpeed * Time.deltaTime);
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
        if (!isDestroying && isRunning)
        {
            StartCoroutine(DestroyAfterDelay());
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        isDestroying = true;
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
