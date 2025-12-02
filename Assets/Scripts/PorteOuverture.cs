using System.Collections;
using UnityEngine;

public class PorteOuverture : MonoBehaviour
{
    public ClefRamassage clef;
    public float hauteur = 2f;
    private Vector3 hautePosition;
    public float vitesseDePositionnement = 1f;
    private Coroutine moveRoutine;

    void Start()
    {
        hautePosition = new Vector3(transform.localPosition.x, hauteur, transform.localPosition.z);
        clef.keyGetted.AddListener(() => OuvrirPorte());
    }

    public void OuvrirPorte()
    {
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }
        moveRoutine = StartCoroutine(MovePorte());
    }

    private IEnumerator MovePorte()
    {
        while (Vector3.Distance(transform.localPosition, hautePosition) > 0.01f)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                hautePosition,
                Time.deltaTime * vitesseDePositionnement
            );
            yield return null;
        }
        transform.localPosition = hautePosition;
    }

}
