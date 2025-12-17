using System.Collections.Generic;
using UnityEngine;

public class OnApproachChest : MonoBehaviour
{
    [SerializeField] public GameObject VFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Controller")
        {
            DisparitionAide();
        }
    }

    public void DisparitionAide()
    {
        BoxCollider[] _collider = GetComponents<BoxCollider>();
        for (int i = 0; i < _collider.Length; i++)
        {
            if (_collider[i].isTrigger == true)
            {
                _collider[i].enabled = false;
            }
        }
        VFX = FindFirstObjectByType<VFXScript>().gameObject;
        if (VFX != null && VFX.GetComponent<VFXScript>().secondEndPoint != null && VFX.GetComponent<VFXScript>().secondEndPoint != VFX.GetComponent<VFXScript>().endPoint)
        {
            VFX.GetComponent<VFXScript>().endPoint = VFX.GetComponent<VFXScript>().secondEndPoint;
        }
      /*  else if (VFX != null)
        {
            Destroy(VFX);
            Destroy(this);
        }*/
            
        //OpenChest();
    }

    public void DisparitionTotale()
    {
        if (VFX != null)
        {
            Destroy(VFX);
            Destroy(this);
        }
    }


    public void OpenChest()
    {
        GetComponentInChildren<OuvertureCoffre>().OpenCouvercle();
        Debug.Log("CoffreOuvert");
    }
}
