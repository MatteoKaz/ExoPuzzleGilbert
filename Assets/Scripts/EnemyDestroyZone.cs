using UnityEngine;

public class EnemyDestroyZone : MonoBehaviour
{
    [Header("Destruction Settings")]
    [SerializeField] private string enemyTag = "Untagged";
    [SerializeField] private bool destroyImmediately = true;
    [SerializeField] private float delayBeforeDestroy = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyMovement>() != null)
        {
            if (destroyImmediately)
            {
                Destroy(other.gameObject);
            }
            else
            {
                Destroy(other.gameObject, delayBeforeDestroy);
            }
        }
    }
}
