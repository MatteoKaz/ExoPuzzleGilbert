using System.Collections;
using UnityEngine;

public class BossIntangibleCinematic : MonoBehaviour
{
    [Header("GameObjects References")]
    public Animator heroAnimator;
    public Animator minotaureAnimator;
    public Animator rocherAnimator;
    public Animator cordeAnimator;

    [Header("Hero Animations")]
    public AnimationClip heroIdleClip;
    public AnimationClip heroRunAttackClip;
    public AnimationClip heroPropulsionClip;
    public AnimationClip heroRelevementClip;
    public AnimationClip heroCoupeCordeClip;
    public AnimationClip heroDeathClip;

    [Header("Minotaure Animations")]
    public AnimationClip minotaureIdleClip;
    public AnimationClip minotaureRireClip;
    public AnimationClip minotaureAttaqueClip;
    public AnimationClip minotaureEcrasementClip;

    [Header("Rocher Animations")]
    public AnimationClip rocherEboulementClip;

    [Header("Corde Animations")]
    public AnimationClip cordeTombeClip;

    [Header("Hero Durations (seconds)")]
    public float heroIdleDuration = 2f;
    public float heroRunAttackDuration = 2.5f;
    public float heroPropulsionDuration = 1.5f;
    public float heroRelevementDuration = 1.5f;
    public float heroCoupeCordeDuration = 1f;
    public float heroDeathDuration = 3f;

    [Header("Minotaure Durations (seconds)")]
    public float minotaureIdleDuration = 2f;
    public float minotaureRireDuration = 2f;
    public float minotaureAttaqueDuration = 2f;
    public float minotaureEcrasementDuration = 2f;

    [Header("Rocher Durations (seconds)")]
    public float rocherEboulementDuration = 2f;

    [Header("Corde Durations (seconds)")]
    public float cordeTombeDuration = 1f;

    [Header("Timing Settings")]
    [Tooltip("Délai avant que le rire commence pendant le Run Attack du héro")]
    public float rireStartDelay = 0.5f;

    [Tooltip("Délai avant que le Minotaure joue son animation d'écrasement après le début de l'éboulement")]
    public float ecrasementStartDelay = 0.5f;

    [Header("Settings")]
    public bool playOnStart = true;
    public bool useCustomDurations = true;
    public bool lockPositions = true;

    private Vector3 heroStartPosition;
    private Vector3 minotaureStartPosition;
    private Vector3 rocherStartPosition;
    private Vector3 cordeStartPosition;

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

        Debug.Log("PHASE 1 : Idle Hero");
        yield return PlayAnimationAndWait(heroAnimator, heroIdleClip, "Hero Idle", heroStartPosition, heroIdleDuration);

        Debug.Log("PHASE 2 : Idle Minotaure");
        yield return PlayAnimationAndWait(minotaureAnimator, minotaureIdleClip, "Minotaure Idle", minotaureStartPosition, minotaureIdleDuration);

        Debug.Log("PHASE 3 : Hero Run Attack + Minotaure Rire (EN PARALLÈLE)");
        StartCoroutine(PlayHeroRunAttack());
        yield return PlayMinotaureRireWithDelay();

        Debug.Log("PHASE 4 : Minotaure Attaque");
        yield return PlayAnimationAndWait(minotaureAnimator, minotaureAttaqueClip, "Minotaure Attaque", minotaureStartPosition, minotaureAttaqueDuration);

        Debug.Log("PHASE 5 : Hero Propulsion");
        yield return PlayAnimationAndWait(heroAnimator, heroPropulsionClip, "Hero Propulsion", heroStartPosition, heroPropulsionDuration);

        Debug.Log("PHASE 6 : Hero Relèvement");
        yield return PlayAnimationAndWait(heroAnimator, heroRelevementClip, "Hero Relèvement", heroStartPosition, heroRelevementDuration);

        Debug.Log("PHASE 7 : Hero Coupe Corde");
        yield return PlayAnimationAndWait(heroAnimator, heroCoupeCordeClip, "Hero Coupe Corde", heroStartPosition, heroCoupeCordeDuration);

        Debug.Log("PHASE 8 : Corde Tombe");
        yield return PlayAnimationAndWait(cordeAnimator, cordeTombeClip, "Corde Tombe", cordeStartPosition, cordeTombeDuration);

        Debug.Log("PHASE 9 : Éboulement + Écrasement Minotaure (EN PARALLÈLE)");
        StartCoroutine(PlayRocherEboulement());
        yield return PlayMinotaureEcrasementWithDelay();

        Debug.Log("PHASE 10 : Hero Possession");
        yield return PlayAnimationAndWait(heroAnimator, heroDeathClip, "Hero Possession/Death", heroStartPosition, heroDeathDuration);

        Debug.Log("=== Cinématique Boss Intangible - Fin ===");
    }

    IEnumerator PlayHeroRunAttack()
    {
        yield return PlayAnimationAndWait(heroAnimator, heroRunAttackClip, "  → Hero Run Attack", heroStartPosition, heroRunAttackDuration);
    }

    IEnumerator PlayMinotaureRireWithDelay()
    {
        yield return new WaitForSeconds(rireStartDelay);

        yield return PlayAnimationAndWait(minotaureAnimator, minotaureRireClip, "  → Minotaure Rire", minotaureStartPosition, minotaureRireDuration);
    }

    IEnumerator PlayRocherEboulement()
    {
        yield return PlayAnimationAndWait(rocherAnimator, rocherEboulementClip, "  → Éboulement", rocherStartPosition, rocherEboulementDuration);
    }

    IEnumerator PlayMinotaureEcrasementWithDelay()
    {
        yield return new WaitForSeconds(ecrasementStartDelay);

        yield return PlayAnimationAndWait(minotaureAnimator, minotaureEcrasementClip, "  → Minotaure Écrasement", minotaureStartPosition, minotaureEcrasementDuration);
    }

    IEnumerator PlayAnimationAndWait(Animator animator, AnimationClip clip, string debugName, Vector3 lockedPosition, float customDuration)
    {
        if (animator == null || clip == null)
        {
            Debug.LogWarning($"⚠️ Animation ou Animator manquant pour: {debugName}");
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

