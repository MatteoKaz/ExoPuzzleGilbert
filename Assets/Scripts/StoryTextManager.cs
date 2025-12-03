using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class StoryTextManager : MonoBehaviour
{
    [Header("Text Component")]
    [SerializeField] private TextMeshProUGUI storyText;
    
    [Header("Story Content")]
    [SerializeField] private string firstText = "Prologue — L'arrivée du Démon. Dans un village accroché aux flancs d'une montagne oubliée, les anciens murmurent qu'un mal sans nom ronge la vallée depuis des générations.";
    [SerializeField] private string secondText = "Nul ne sait quand il est né, ni quel visage il portait alors ; on ne se souvient que des ombres, des grondements, et de l'effroi qui couvrit les nuits pendant des siècles.";
    [SerializeField] private string thirdText = "";
    
    [Header("Timing - Durée de chaque dialogue")]
    [SerializeField] private float firstTextDuration = 6f;
    [SerializeField] private float secondTextDuration = 6f;
    [SerializeField] private float thirdTextDuration = 6f;
    
    [Header("Fade Settings")]
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private float fadeOutDuration = 1f;
    
    [Header("Transition Settings")]
    [SerializeField] private bool autoTransitionToNextLevel = true;
    [SerializeField] private float delayBeforeTransition = 1f;
    
    [Header("Music Settings")]
    [SerializeField] private bool triggerMusicSwitch = false;
    [SerializeField] private string sceneThatSwitchesMusic = "Entracte3";
    
    private TransitionEntracte transitionManager;
    
    void Start()
    {
        if (storyText == null)
        {
            storyText = GetComponent<TextMeshProUGUI>();
        }
        
        transitionManager = FindFirstObjectByType<TransitionEntracte>();
        
        if (triggerMusicSwitch && SceneManager.GetActiveScene().name == sceneThatSwitchesMusic)
        {
            if (PrologueMusicManager.Instance != null)
            {
                PrologueMusicManager.Instance.TriggerSwitchToMain();
            }
            else
            {
                Debug.LogWarning("PrologueMusicManager.Instance est null, impossible de changer la musique.");
            }
        }
        
        StartCoroutine(DisplayStorySequence());
    }
    
    private IEnumerator DisplayStorySequence()
    {
        yield return StartCoroutine(ShowTextWithFade(firstText, firstTextDuration));
        
        yield return StartCoroutine(ShowTextWithFade(secondText, secondTextDuration));
        
        if (!string.IsNullOrEmpty(thirdText))
        {
            yield return StartCoroutine(ShowTextWithFade(thirdText, thirdTextDuration));
        }
        
        if (autoTransitionToNextLevel && transitionManager != null)
        {
            yield return new WaitForSeconds(delayBeforeTransition);
            transitionManager.Fondeur();
        }
    }
    
    private IEnumerator ShowTextWithFade(string text, float displayDuration)
    {
        storyText.text = text;
        
        yield return StartCoroutine(FadeIn());
        
        yield return new WaitForSeconds(displayDuration);
        
        yield return StartCoroutine(FadeOut());
    }
    
    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        Color textColor = storyText.color;
        
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            storyText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }
        
        storyText.color = new Color(textColor.r, textColor.g, textColor.b, 1f);
    }
    
    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color textColor = storyText.color;
        
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            storyText.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }
        
        storyText.color = new Color(textColor.r, textColor.g, textColor.b, 0f);
    }
}
