using System.Collections;
using UnityEngine;

[System.Serializable]
public class AnimationStep
{
    public string stepName;
    public Animator targetAnimator;
    public AnimationClip clip;
    public float duration = 1f;
    [Tooltip("Délai avant de jouer cette étape")]
    public float startDelay = 0f;
    [Tooltip("Cette animation se joue en parallèle avec la suivante")]
    public bool playInParallel = false;
}

public class BossIntangibleCinematic : MonoBehaviour
{
    [Header("Animation Sequence")]
    [Tooltip("Configurez l'ordre et les paramètres des animations ici")]
    public AnimationStep[] animationSequence;

    [Header("GameObjects References")]
    public Animator heroAnimator;
    public Animator minotaureAnimator;
    public Animator rocherAnimator;
    public Animator cordeAnimator;

    [Header("Settings")]
    public bool playOnStart = true;
    public bool lockPositions = true;

    private Vector3 heroStartPosition;
    private Vector3 minotaureStartPosition;
    private Vector3 rocherStartPosition;
    private Vector3 cordeStartPosition;

    void Start()
    {
        SaveInitialPositions();
        DisableAnimatorsAtStart();

        if (playOnStart)
        {
            StartCinematic();
        }
    }

    void DisableAnimatorsAtStart()
    {
        if (heroAnimator != null)
            heroAnimator.enabled = false;

        if (minotaureAnimator != null)
            minotaureAnimator.enabled = false;

        if (rocherAnimator != null)
            rocherAnimator.enabled = false;

        if (cordeAnimator != null)
            cordeAnimator.enabled = false;
    }

    void SaveInitialPositions()
    {
        if (heroAnimator != null)
            heroStartPosition = heroAnimator.transform.position;

        if (minotaureAnimator != null)
            minotaureStartPosition = minotaureAnimator.transform.position;

        if (rocherAnimator != null)
            rocherStartPosition = rocherAnimator.transform.position;

        if (cordeAnimator != null)
            cordeStartPosition = cordeAnimator.transform.position;
    }

    public void StartCinematic()
    {
        StartCoroutine(PlayCinematicSequence());
    }

    IEnumerator PlayCinematicSequence()
    {
        Debug.Log("=== Cinématique Boss Intangible - Début ===");

        for (int i = 0; i < animationSequence.Length; i++)
        {
            AnimationStep step = animationSequence[i];

            if (step.targetAnimator == null || step.clip == null)
            {
                Debug.LogWarning($"⚠️ Étape {i} ({step.stepName}) invalide - Animator ou Clip manquant");
                continue;
            }

            Debug.Log($"PHASE {i + 1} : {step.stepName}");

            if (step.startDelay > 0)
            {
                yield return new WaitForSeconds(step.startDelay);
            }

            Vector3 lockedPosition = GetLockedPosition(step.targetAnimator);

            if (step.playInParallel && i < animationSequence.Length - 1)
            {
                StartCoroutine(PlayAnimationAndWait(step.targetAnimator, step.clip, step.stepName, lockedPosition, step.duration));
            }
            else
            {
                yield return PlayAnimationAndWait(step.targetAnimator, step.clip, step.stepName, lockedPosition, step.duration);
            }
        }

        Debug.Log("=== Cinématique Boss Intangible - Fin ===");
    }

    Vector3 GetLockedPosition(Animator animator)
    {
        if (!lockPositions || animator == null)
            return Vector3.zero;

        if (animator == heroAnimator)
            return heroStartPosition;
        else if (animator == minotaureAnimator)
            return minotaureStartPosition;
        else if (animator == rocherAnimator)
            return rocherStartPosition;
        else if (animator == cordeAnimator)
            return cordeStartPosition;

        return animator.transform.position;
    }

    IEnumerator PlayAnimationAndWait(Animator animator, AnimationClip clip, string debugName, Vector3 lockedPosition, float duration)
    {
        if (animator == null || clip == null)
        {
            Debug.LogWarning($"⚠️ Animation ou Animator manquant pour: {debugName}");
            yield break;
        }

        animator.enabled = true;
        Debug.Log($"▶ {debugName}");
        animator.Play(clip.name, 0, 0f);

        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (lockPositions)
            {
                animator.transform.position = lockedPosition;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
