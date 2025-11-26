using UnityEngine;

public class RabbitRunZ : MonoBehaviour
{
    public float speed = 5f;
    private Animator animator;
    private bool isRunning = true;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator manquant sur le Lapin !");
        }
    }

    void Start()
    {
        animator.Play("Run");
    }

    void Update()
    {
        if (!isRunning) return;

        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Run") && stateInfo.normalizedTime >= 1f)
        {
            StopRunning();
        }
    }

    void StopRunning()
    {
        isRunning = false;
        speed = 0f;
        animator.speed = 0f;
    }
}
