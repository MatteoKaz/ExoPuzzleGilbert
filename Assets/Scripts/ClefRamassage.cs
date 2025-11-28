using UnityEngine;

public class ClefRamassage : MonoBehaviour
{
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
        Destroy(gameObject);
    }
}
