using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Death Settings")]
    [SerializeField] private string deathAnimationName = "Enemy_Death";
    [SerializeField] private float destroyDelay = 2f;

    private Animator animator;
    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KillEnemy();
        }
    }

    public void KillEnemy()
    {
        if (isDead) return;

        isDead = true;

        EnemyMovement movement = GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }

        if (animator != null)
        {
            animator.Play(deathAnimationName);
        }

        Destroy(gameObject, destroyDelay);
    }
}
