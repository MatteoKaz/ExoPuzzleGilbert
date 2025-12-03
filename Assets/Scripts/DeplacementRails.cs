using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class DeplacementRails : MonoBehaviour
{
    public float minClamp = -1f;
    public float maxClamp = -0.15f;
    List<Collision> listColl = new List<Collision>();
    void FixedUpdate()
    {
        //Vector3 pos = transform.position;
        Vector3 pos = transform.localPosition;
        pos.z = Mathf.Clamp(pos.z, minClamp, maxClamp);
        if (pos.z >= maxClamp || pos.z <= minClamp)
        {
            if (listColl.Count > 0)
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        transform.localPosition = pos;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.GetComponent<PousseurAimant>() != null)
        {
            listColl.Add(collision);
            gameObject.layer = LayerMask.NameToLayer("ShadowPlayerTraversable");
        }
        else
        {
            if (listColl.Count <= 0)
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }
        }
       
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.gameObject.GetComponent<PousseurAimant>() != null)
        {
            listColl.Remove(collision);
            if (listColl.Count <= 0)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                gameObject.layer = LayerMask.NameToLayer("Aimant");
            }
        }
        else
        {
            if (listColl.Count <= 0)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                gameObject.layer = LayerMask.NameToLayer("Aimant");
            }
        }
       
    }
    
}
