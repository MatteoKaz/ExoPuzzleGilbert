using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToEntracte : MonoBehaviour
{
    [SerializeField] public int whichNext;
    [SerializeField] public float timeInLevel = 10f;
    [SerializeField] public GameObject HUD;
    private float aValue = 0f;
    private CanvasGroup trans;
    [SerializeField] public float transitionDuree = 1.5f;

    publie void ToEntracte()
    {
        StartCoroutine(Fondu());
    }

    public void ChangeLevel(int whichNext)
    {
        SceneManager.LoadScene(whichNext);
    }

    public IEnumerator Fondu()
    {
        float temps = 0f;

        while (temps < transitionDuree)
        {
            temps += Time.deltaTime;
            aValue = Mathf.Lerp(0f, 1f, temps / transitionDuree);
            trans.alpha = aValue;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        ChangeLevel(whichNext);
    }





}
