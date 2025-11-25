using UnityEngine;

public class MurCollisionBlack : MonoBehaviour
{
    public GameObject blackScreen;     
    public LayerMask wallMask;         

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(wallMask);
        Debug.Log(other.gameObject.layer);
        Debug.Log(wallMask.value);
    //    if (other.gameObject.layer == wallMask.value)
       // {
            blackScreen.SetActive(true);
     //       Debug.Log("Enter");
     //  }
    }

    private void OnTriggerExit(Collider other)
    {
    //   if (other.gameObject.layer == wallMask.value)
    //    {
            blackScreen.SetActive(false);
            Debug.Log("Exit");
   //     }
    }
}
