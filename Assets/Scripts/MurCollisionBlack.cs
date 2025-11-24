using UnityEngine;

public class MurCollisionBlack : MonoBehaviour
{
    public GameObject blackScreen;     
    public LayerMask wallMask;         

    private void OnTriggerEnter(Collider other)
    {
        if ((wallMask.value & (1 << other.gameObject.layer)) != 0)
        {
            blackScreen.SetActive(true);
            Debug.Log("Enter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((wallMask.value & (1 << other.gameObject.layer)) != 0)
        {
            blackScreen.SetActive(false);
            Debug.Log("Exit");
        }
    }
}
