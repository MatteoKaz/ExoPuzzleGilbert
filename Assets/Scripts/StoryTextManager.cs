using UnityEngine;
using TMPro;
using System.Collections;

public class StoryTextManager : MonoBehaviour
{
    [Header("Text Component")]
    [SerializeField] private TextMeshProUGUI storyText;
    
    [Header("Story Content")]
    [SerializeField] private string firstText = "Prologue — L'arrivée du Démon. Dans un village accroché aux flancs d'une montagne oubliée, les anciens murmurent qu'un mal sans nom ronge la vallée depuis des générations.";
    [SerializeField] private string secondText = "Nul ne sait quand il est né, ni quel visage il portait alors ; on ne se souvient que des ombres, des grondements, et de l'effroi qui couvrit les nuits pendant des siècles.";
    [SerializeField] private string thirdText = "";
    
    [Header("Timing")]
    [SerializeField] private float delayBeforeSecondText = 6f;
    [SerializeField] private float delayBeforeThirdText = 6f;
    
    void Start()
    {
        if (storyText == null)
        {
            storyText = GetComponent<TextMeshProUGUI>();
        }
        
        StartCoroutine(DisplayStorySequence());
    }
    
    private IEnumerator DisplayStorySequence()
    {
        storyText.text = firstText;
        
        yield return new WaitForSeconds(delayBeforeSecondText);
        
        storyText.text = secondText;
        
        if (!string.IsNullOrEmpty(thirdText))
        {
            yield return new WaitForSeconds(delayBeforeThirdText);
            storyText.text = thirdText;
        }
    }
}