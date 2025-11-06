using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToEntracte : MonoBehaviour
{
    [SerializeField] public int whichNext = 2;
    [SerializeField] public GameObject HUD;
    [SerializeField] public LIghtManager lightner;
    private float aValue = 0f;
    private CanvasGroup trans;
    [SerializeField] public float transitionDuree = 1.5f;
    public LightToTurn lighter;
    public GameObject lighterGame;



    private void Start()
    {
        lightner = FindFirstObjectByType<LIghtManager>();
        lighter = FindFirstObjectByType<LightToTurn>();

    }

    public void ToEntracte()
    {
        StartCoroutine(Fondu());
    }

    public void ChangeLevel(int whichNext)
    {
        SceneManager.LoadScene(whichNext);
    }

    public IEnumerator Fondu()
    {
        if (lighter != null)
        {    
         lighterGame = lighter.gameObject;
         lightner.TurnOff(lighterGame); 
        }
        else
        { Debug.Log("Vide"); }

        yield return new WaitForSeconds(3.5f);

        ChangeLevel(whichNext);
    }





}
