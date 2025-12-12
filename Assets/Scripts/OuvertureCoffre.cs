using System.Collections;
using UnityEngine;

public class OuvertureCoffre : MonoBehaviour
{
    [SerializeField] public Vector3 rotationToRush = new Vector3(0, 0, -45);
    [SerializeField] public float vitesseDePositionnement = 1f;

    public void OpenCouvercle()
    {
        StartCoroutine(Opener());
    }

    public IEnumerator Opener()
    {
        Quaternion startRotation = transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(rotationToRush);

        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            timeElapsed += Time.deltaTime * vitesseDePositionnement;
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed);
            yield return null;
        }

        transform.localRotation = targetRotation;
    }
}
