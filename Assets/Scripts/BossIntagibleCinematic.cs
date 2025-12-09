using System.Collections;
using UnityEngine;

public class BossIntangibleCinematic : MonoBehaviour
{
    [Header("GameObjects References")]
    public Animator heroAnimator;
    public Animator minotaureAnimator;
    public Animator rocherAnimator;

    [Header("Hero Animations")]
    public AnimationClip heroIdleClip;
    public AnimationClip heroRunAttackClip;
    public AnimationClip heroPropulsionClip;
    public AnimationClip heroRelevementClip;
    public AnimationClip heroCoupeCordeClip;
    public AnimationClip heroDeathClip;

    [Header("Minotaure Animations")]
    public AnimationClip minotaureIdleClip;
    public AnimationClip minotaureAttaqueClip;
    public AnimationClip minotaureRireClip;
    public AnimationClip minotaureEcrasementClip;

    [Header("Rocher Animations")]
    public AnimationClip rocherEboulementClip;

    [Header("Hero Durations (seconds)")]
    public float heroIdleDuration = 2f;
    public float heroRunAttackDuration = 2.5f;
    public float heroPropulsionDuration = 1.5f;
    public float heroRelevementDuration = 1.5f;
    public float heroCoupeCordeDuration = 1f;
    public float heroDeathDuration = 3f;

    [Header("Minotaure Durations (seconds)")]
    public float minotaureIdleDuration = 2f;
    public float minotaureAttaqueDuration = 2f;
    public float minotaureRireDuration = 2f;
    public float minotaureEcrasementDuration = 2f;

    [Header("Rocher Durations (seconds)")]
    public float rocherEboulementDuration = 2f;

    [Header("Settings")]
    public bool playOnStart = true;
    public bool useCustomDurations = true;
    public bool lockPositions = true;

    private Vector3 heroStartPosition;
    private Vector3 minotaureStartPosition;
    private Vector3 rocherStartPosition;

    void Start()
    {
        SaveInitialPositions();

        if (playOnStart)
        {
            StartCinematic();
        }
    }

    void SaveInitialPositions()
    {
        if (heroAnimator != null)
            heroStartPosition = heroAnimator.transform.position;

        if (minotaureAnimator != null)
            minotaureStartPosition = minotaureAnimator.transform.position;

        if (rocherAnimator != null)
            rocherStartPosition = rocherAnimator.transform.position;
    }

    public void StartCinematic()
    {
        StartCoroutine(PlayCinematicSequence());
    }

    IEnumerator PlayCinematicSequence()
    {
        Debug.Log("Cinématique Boss Intangible - Début");

        yield return PlayAnimationAndWait(heroAnimator, heroIdleClip, "Hero Idle", heroStartPosition, heroIdleDuration);
        yield return PlayAnimationAndWait(minotaureAnimator, minotaureIdleClip, "Minotaure Idle", minotaureStartPosition, minotaureIdleDuration);
        yield return PlayAnimationAndWait(heroAnimator, heroRunAttackClip, "Hero Run Attack", heroStartPosition, heroRunAttackDuration);
        yield return PlayAnimationAndWait(minotaureAnimator, minotaureAttaqueClip, "Minotaure Attaque", minotaureStartPosition, minotaureAttaqueDuration);
        yield return PlayAnimationAndWait(heroAnimator, heroPropulsionClip, "Hero Propulsion", heroStartPosition, heroPropulsionDuration);
        yield return PlayAnimationAndWait(minotaureAnimator, minotaureRireClip, "Minotaure Rire", minotaureStartPosition, minotaureRireDuration);
        yield return PlayAnimationAndWait(heroAnimator, heroRelevementClip, "Hero Relèvement", heroStartPosition, heroRelevementDuration);
        yield return PlayAnimationAndWait(heroAnimator, heroCoupeCordeClip, "Hero Coupe Corde", heroStartPosition, heroCoupeCordeDuration);
        yield return PlayAnimationAndWait(rocherAnimator, rocherEboulementClip, "Éboulement", rocherStartPosition, rocherEboulementDuration);
        yield return PlayAnimationAndWait(minotaureAnimator, minotaureEcrasementClip, "Minotaure Écrasement", minotaureStartPosition, minotaureEcrasementDuration);
        yield return PlayAnimationAndWait(heroAnimator, heroDeathClip, "Hero Death", heroStartPosition, heroDeathDuration);

        Debug.Log("Cinématique Boss Intangible - Fin");
    }

    IEnumerator PlayAnimationAndWait(Animator animator, AnimationClip clip, string debugName, Vector3 lockedPosition, float customDuration)
    {
        if (animator == null || clip == null)
        {
            Debug.LogWarning($"Animation ou Animator manquant pour: {debugName}");
            yield break;
        }

        Debug.Log($"▶ {debugName}");

        animator.Play(clip.name);

        float duration = useCustomDurations ? customDuration : clip.length;
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

