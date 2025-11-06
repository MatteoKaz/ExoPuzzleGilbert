using System.Collections;
using UnityEngine;

public class LIghtManager : MonoBehaviour
{
    [SerializeField] public GameObject lighting;
    [SerializeField] public float transitionDuree;
    [SerializeField] public float transitionDuree2;
    [SerializeField] public float valueLight;
    private Light lum;
    public bool state = false;

    private void Start()
    {
        TurnOn(lighting);
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
            lighting.SetActive(false);
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
    }



    private IEnumerator FonduEnNoir()
    {
        float temps = 0f;

        while (temps < transitionDuree2)
        {
            temps += Time.deltaTime;
            //opacité image = Mathf.Lerp(0f, 1f, temps / transitionDuree2);
            yield return null;
        }
    }
}
