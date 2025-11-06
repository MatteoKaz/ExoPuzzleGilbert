using UnityEngine;

public class Dead_Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private Animator enemyAnimator;
    [SerializeField] private string deathAnimationName = "Enemy_Death";

    [Header("Optional")]
    [SerializeField] private float destroyAfterSeconds = 3f;
    [SerializeField] private bool destroyAfterDeath = true;

    private bool isDead = false;

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            TriggerEnemyDeath();
        }
    }

    void TriggerEnemyDeath()
    {
        isDead = true;

        if (enemyAnimator != null)
        {
            enemyAnimator.Play(deathAnimationName);
        }

        if (destroyAfterDeath)
        {
            Destroy(enemyAnimator.gameObject, destroyAfterSeconds);
        }
    }
}
