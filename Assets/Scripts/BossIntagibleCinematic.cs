using System.Collections;
using UnityEngine;

public class BossIntangibleCinematic : MonoBehaviour
{
    [Header("GameObjects References")]
    public Animator heroAnimator;
    public Animator minotaureAnimator;
    public Animator rocherAnimator;

    [Header("Animation Names")]
    public string heroIdleAnim = "Idle_Booshero";
    public string heroRunAttackAnim = "Anim_Saut_Attaque_Boss";
    public string heroCoupeCordeAnim = "coupe_cordeanim";
    public string heroDeathAnim = "Death_BossIntangible";

    public string minotaureAttaqueAnim = "Boss_Fight_ATTAQUE_Minotaure";
    public string minotaureRireAnim = "Boss_Fight_RIRE_Minotaure";
    public string minotaureEcrasementAnim = "Boss_Fight_Ecrasement_Minotaure";

    public string rocherEboulementAnim = "Eboulement"

    [Header("Timing (in seconds)")]
    public float idleDuration = 2f;
    public float runAttackDuration = 2.5f;
    public float minotaureAttaqueDuration = 2f;
    public float minotaureRireDuration = 2f;
    public float coupeCordeDuration = 1f;
    public float eboulementDuration = 2f;
    public float ecrasementDuration = 2f;

    [Header("Settings")]
    public bool playOnStart = true;

    void Start()
    {
        if (playOnStart)
        {
            StartCinematic();
        }
    }

    public void StartCinematic()
    {
        StartCoroutine(PlayCinematicSequence());
    }

    IEnumerator PlayCinematicSequence()
    {
        Debug.Log("Cinématique Boss Intangible - Début");

        // 1. Idle du héro
        Debug.Log("1. Hero Idle");
        PlayAnimation(heroAnimator, heroIdleAnim);
        yield return new WaitForSeconds(idleDuration);

        // 2. RunAttack du héro
        Debug.Log("2. Hero Run Attack");
        PlayAnimation(heroAnimator, heroRunAttackAnim);
        yield return new WaitForSeconds(runAttackDuration);

        // 3. Attaque du Minotaure
        Debug.Log("3. Minotaure Attaque");
        PlayAnimation(minotaureAnimator, minotaureAttaqueAnim);
        yield return new WaitForSeconds(minotaureAttaqueDuration);

        // 4. Rire du Minotaure
        Debug.Log("4. Minotaure Rire");
        PlayAnimation(minotaureAnimator, minotaureRireAnim);
        yield return new WaitForSeconds(minotaureRireDuration);

        // 5. Héro coupe la corde
        Debug.Log("5. Hero Coupe Corde");
        PlayAnimation(heroAnimator, heroCoupeCordeAnim);
        yield return new WaitForSeconds(coupeCordeDuration);

        // 6. Éboulement du rocher
        Debug.Log("6. Éboulement");
        PlayAnimation(rocherAnimator, rocherEboulementAnim);
        yield return new WaitForSeconds(eboulementDuration);

        // 7. Écrasement du Minotaure
        Debug.Log("7. Minotaure Écrasement");
        PlayAnimation(minotaureAnimator, minotaureEcrasementAnim);
        yield return new WaitForSeconds(ecrasementDuration);

        // 8. Death du héro
        Debug.Log("8. Hero Death");
        PlayAnimation(heroAnimator, heroDeathAnim);

        Debug.Log("Cinématique Boss Intangible - Fin");
    }

    void PlayAnimation(Animator animator, string animationName)
    {
        if (animator != null && !string.IsNullOrEmpty(animationName))
        {
            animator.Play(animationName);
        }
        else
        {
            Debug.LogWarning($"Impossible de jouer l'animation: {animationName}");
        }
    }
}
