using System.Collections;
using UnityEngine;

public class LIghtManager : MonoBehaviour
{
    [SerializeField] public GameObject lighting;
    [SerializeField] public GameObject HUD;
    [SerializeField] public float transitionDuree = 1f;
    [SerializeField] public float transitionDuree2 = 1.5f;
    [SerializeField] public float valueLight;
    private Light lum;
    public bool state = false;
    private float aValue = 0f;
    private CanvasGroup trans;

    private void Start()
    {
        lum = lighting.GetComponent<Light>();
        lum.intensity = 0f;
        HUD = GameObject.Find("=== HUD ===");
        trans = HUD.GetComponent<CanvasGroup>();
        RetourALaLumiere();
        
        state = lighting.active;
        Debug.Log(state);

    }

    public void TurnOn(GameObject lighting)
    {
        if (lighting != null)
        {
            lum = lighting.GetComponent<Light>();
            StartCoroutine(IncreaseOpacity());
            state = true;
            lighting.SetActive(true);
        }
        else
        {
            Debug.Log("LightIsNull");
        }
    }

    public void TurnOff(GameObject lighting)
    {
        if (lighting != null)
        {
            StartCoroutine(DecreaseOpacity());

            state = false;
            //lighting.SetActive(false);
        }
    }


    private IEnumerator IncreaseOpacity()
    {
        float temps = 0f;

        while (temps < transitionDuree)
        {
            temps += Time.deltaTime;
            lum.intensity = Mathf.Lerp(0f, valueLight, temps / transitionDuree);
            yield return null;
        }

    }

    

    private IEnumerator DecreaseOpacity()
    {
        float temps = 0f;

        while (temps < transitionDuree)
        {
            temps += Time.deltaTime;
            lum.intensity = Mathf.Lerp(valueLight, 0f, temps / transitionDuree);
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        if (trans != null)
        {
            StartCoroutine(FonduEnNoir());
        }
        else
        {
            Debug.Log("Canvasgroup Invalide");
        }
    }



    private IEnumerator FonduEnNoir()
    {
        float temps = 0f;

        while (temps < transitionDuree2)
        {
            temps += Time.deltaTime;
            aValue = Mathf.Lerp(0f, 1f, temps / transitionDuree2);
            trans.alpha = aValue;
            yield return null;
        }
    }

    public void RetourALaLumiere()
    {
        StartCoroutine(DeFonduEnNoir());
    }

    private IEnumerator DeFonduEnNoir()
    {
        float temps = 0f;

        while (temps < transitionDuree2)
        {
            temps += Time.deltaTime;
            aValue = Mathf.Lerp(1f, 0f, temps / transitionDuree2);
            trans.alpha = aValue;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        TurnOn(lighting);
    }
}
