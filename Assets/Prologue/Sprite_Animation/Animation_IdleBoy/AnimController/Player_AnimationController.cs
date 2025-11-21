using UnityEngine;

public class Player_AnimationController : MonoBehaviour
{
    private Animator animator;

    [Header("Auto Start Settings")]
    [SerializeField] private float delayBeforeRun = 0.5f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Invoke(nameof(StartRunAnimation), delayBeforeRun);
    }

    private void StartRunAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsRunning", true);
        }
    }
}
