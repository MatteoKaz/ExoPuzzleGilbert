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
    public AnimationClip minotaureAttaqueClip;
    public AnimationClip minotaureRireClip;
    public AnimationClip minotaureEcrasementClip;

    [Header("Rocher Animations")]
    public AnimationClip rocherEboulementClip;

    [Header("Settings")]
    public bool playOnStart = true;
    public bool useClipDurations = true;
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

        yield return PlayAnimationAndWait(heroAnimator, heroIdleClip, "Hero Idle", heroStartPosition);
        yield return PlayAnimationAndWait(heroAnimator, heroRunAttackClip, "Hero Run Attack", heroStartPosition);
        yield return PlayAnimationAndWait(minotaureAnimator, minotaureAttaqueClip, "Minotaure Attaque", minotaureStartPosition);
        yield return PlayAnimationAndWait(heroAnimator, heroPropulsionClip, "Hero Propulsion", heroStartPosition);
        yield return PlayAnimationAndWait(minotaureAnimator, minotaureRireClip, "Minotaure Rire", minotaureStartPosition);
        yield return PlayAnimationAndWait(heroAnimator, heroRelevementClip, "Hero Relèvement", heroStartPosition);
        yield return PlayAnimationAndWait(heroAnimator, heroCoupeCordeClip, "Hero Coupe Corde", heroStartPosition);
        yield return PlayAnimationAndWait(rocherAnimator, rocherEboulementClip, "Éboulement", rocherStartPosition);
        yield return PlayAnimationAndWait(minotaureAnimator, minotaureEcrasementClip, "Minotaure Écrasement", minotaureStartPosition);
        yield return PlayAnimationAndWait(heroAnimator, heroDeathClip, "Hero Death", heroStartPosition);

        Debug.Log("Cinématique Boss Intangible - Fin");
    }

    IEnumerator PlayAnimationAndWait(Animator animator, AnimationClip clip, string debugName, Vector3 lockedPosition)
    {
        if (animator == null || clip == null)
        {
            Debug.LogWarning($"Animation ou Animator manquant pour: {debugName}");
            yield break;
        }

        Debug.Log($"▶ {debugName}");

        animator.Play(clip.name);

        float duration = useClipDurations ? clip.length : 1f;
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
