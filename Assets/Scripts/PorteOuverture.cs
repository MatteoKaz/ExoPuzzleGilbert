using UnityEngine;

public class PorteOuverture : MonoBehaviour
{
    public ClefRamassage clef;
    void Start()
    {
        clef.keyGetted.AddListener(() => OuvrirPorte());
    }

    public void OuvrirPorte()
    {
       /* public void MouvementRampe(bool hauteur)
    {
        _bMonte = hauteur;
        if (moveRoutine != null)
        {
            StopCoroutine(moveRoutine);
        }
        moveRoutine = StartCoroutine(MoveRamp());
    }

    private IEnumerator MoveRamp()
    {
        Vector3 target = _bMonte ? hautePosition : bassePosition;  // en fonction de la valeur de _bMonte, target a la valeur de haute position ou de basse position

        while (Vector3.Distance(rampe.position, target) > 0.01f)
        {
            rampe.position = Vector3.MoveTowards(rampe.position, target, Time.deltaTime * vitesseDePositionnement);
            yield return null;

            Vector3 newTarget = _bMonte ? hautePosition : bassePosition;
            if (newTarget != target)
            {
                target = newTarget;
            }
        }
        rampe.position = target;*/
    }
}
