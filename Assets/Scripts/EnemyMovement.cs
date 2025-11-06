using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Detection Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float runSpeed = 3f;

    [Header("Destruction Settings")]
    [SerializeField] private float destroyAfterSeconds = 5f;

    [Header("Animation")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isRunning = false;
    private float runningTime = 0f;
    private float speed = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (player == null)
        {
            GameObject playerObj = GameObject.Find("Idle_0");
            if (playerObj == null) playerObj = GameObject.Find("Character");
            if (playerObj != null) player = playerObj.transform;
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

        if (rb != null)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, speed);
        }

        if (isRunning)
        {
            runningTime += Time.deltaTime;

            if (runningTime >= destroyAfterSeconds)
            {
                Destroy(gameObject);
            }
        }
    }

    void RunAway()
    {
        if (!isRunning)
        {
            isRunning = true;
            runningTime = 0f;
            animator.Play("Enemy_Walk");
            spriteRenderer.flipX = false;
        }

        float directionX = transform.position.x - player.position.x;

        if (directionX < 0)
        {
            speed = -runSpeed;
        }
        else
        {
            speed = runSpeed;
        }
    }

    void StopRunning()
    {
        if (isRunning)
        {
            isRunning = false;
            runningTime = 0f;
            animator.Play("Enemy_IDLE");
        }

        speed = 0f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

