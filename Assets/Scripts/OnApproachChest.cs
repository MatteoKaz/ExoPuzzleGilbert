using System.Collections.Generic;
using UnityEngine;

public class OnApproachChest : MonoBehaviour
{
    [SerializeField] public GameObject VFX;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Controller")
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
            if (VFX != null)
            {
                Destroy(VFX);
            }
            OpenChest();
        }
    }

    public void OpenChest()
    {
        GetComponentInChildren<OuvertureCoffre>().OpenCouvercle();
        Debug.Log("CoffreOuvert");
    }
}
