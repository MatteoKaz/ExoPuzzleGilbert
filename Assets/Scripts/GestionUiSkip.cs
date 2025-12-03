using System.Collections;
using UnityEngine;

public class GestionUiSkip : MonoBehaviour
{
    private Coroutine opacity;
    private Coroutine unopacity;
    private CanvasGroup ui;

    [SerializeField] public float transitionDuree = 0.2f;


    public void AfficherUI()
    {
        ui = GetComponent<CanvasGroup>();

        gameObject.SetActive(true);
        if (opacity == null && unopacity == null)
        {
            opacity = StartCoroutine(OpacityUp());
        }
    }

    private IEnumerator OpacityUp()
    {
        float temps = 0f;
        float a = ui.alpha;
        while (temps < transitionDuree)
        {
            temps += Time.deltaTime;
            ui.alpha = Mathf.Lerp(a, 1f, temps / transitionDuree);
            yield return null;
        }
        ui.alpha = 1.0f;
        yield return null; 
    }

    public void DesafficherUI()
    {
        ui = GetComponent<CanvasGroup>();
        if (opacity == null && unopacity == null)
        {
            unopacity = StartCoroutine(OpacityDown());
        }

    }
    private IEnumerator OpacityDown()
    {
        float temps = 0f;
        float a = ui.alpha;

        while (temps < transitionDuree)
        {
            temps += Time.deltaTime;
            ui.alpha = Mathf.Lerp(a, 0f, temps / transitionDuree);
            yield return null;
        }
        ui.alpha = 0f;
        gameObject.SetActive(false);
        yield return null;
    }
}
