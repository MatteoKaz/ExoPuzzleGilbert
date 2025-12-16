using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ClefRamassage : MonoBehaviour
{
    public UnityEvent keyGetted;
    private AudioSource audioKey;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            audioKey = GetComponent<AudioSource>();
            audioKey.Play();
            StartCoroutine(ClearLag());
            OuverturePorte();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void OuverturePorte()
    {
        keyGetted.Invoke();
    }

    IEnumerator ClearLag()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        yield return null;
    }
}
