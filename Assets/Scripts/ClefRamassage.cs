using UnityEngine;
using UnityEngine.Events;

public class ClefRamassage : MonoBehaviour
{
    public UnityEvent keyGetted;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            OuverturePorte();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void OuverturePorte()
    {
        keyGetted.Invoke();
        Destroy(gameObject);
    }
}
