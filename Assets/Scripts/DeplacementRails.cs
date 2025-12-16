using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class DeplacementRails : MonoBehaviour
{
    public float minClamp = -1f;
    public float maxClamp = -0.15f;
    List<Collision> listColl = new List<Collision>();
    public float forcePoussee = 1;
    Rigidbody rb;
    Transform rail;
    private RigidbodyConstraints baseConstrain;

    private void Start()
    { 
        rb = GetComponent<Rigidbody>();
        rail = transform.parent;
        baseConstrain = rb.constraints;
    }
    void FixedUpdate()
    {
        //Vector3 pos = transform.position;
        Vector3 pos = transform.localPosition;
        pos.z = Mathf.Clamp(pos.z, minClamp, maxClamp);
        Vector3 rPos = transform.parent.TransformPoint(pos);
        if (transform.localPosition.z >= maxClamp || transform.localPosition.z <= minClamp)
        {
            rb.MovePosition(rPos);
            if (listColl.Count > 0)
            {
                rb.constraints = RigidbodyConstraints.FreezeAll;
            //    rb.isKinematic = true;
                
            }
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.GetComponent<PousseurAimant>() != null)
        {
            listColl.Add(collision);
          //  gameObject.layer = LayerMask.NameToLayer("ShadowPlayerTraversable");
        }
        else
        {
            if (listColl.Count > 0)
            {
             //   GetComponent<Rigidbody>().isKinematic = true;
            }
            else if (listColl.Count <= 0)
            {
                //   rb.isKinematic = false;
                rb.constraints = baseConstrain;
                    /*RigidbodyConstraints.FreezePositionX |
                 RigidbodyConstraints.FreezePositionY |
                 RigidbodyConstraints.FreezeRotation;*/

                gameObject.layer = LayerMask.NameToLayer("Aimant");
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
                //   rb.isKinematic = false;
                rb.constraints = baseConstrain;
                    /*RigidbodyConstraints.FreezePositionX |
                 RigidbodyConstraints.FreezePositionY |
                 RigidbodyConstraints.FreezeRotation;*/

                gameObject.layer = LayerMask.NameToLayer("Aimant");
            }
        }
        else
        {
            if (listColl.Count <= 0)
            {
                //   rb.isKinematic = false;
                rb.constraints = baseConstrain;
                    /*RigidbodyConstraints.FreezePositionX |
                 RigidbodyConstraints.FreezePositionY |
                 RigidbodyConstraints.FreezeRotation;*/

                gameObject.layer = LayerMask.NameToLayer("Aimant");
            }
        }
       
    }

    private void OnCollisionStay(Collision collision)
    {
        PousseurAimant pousseur = collision.gameObject.GetComponent<PousseurAimant>();
        if (pousseur == null)
        {
            return;
        }
        Debug.Log("POUSSE");

        Vector3 toPusher = rb.position - collision.transform.position;
        float sign = Mathf.Sign(Vector3.Dot(toPusher, rail.forward));
        Vector3 pushDir = rail.forward * sign;
        //    Vector3 pousseeDirection = new Vector3(0, 0, collision.contacts[0].normal.z); //prend la direction par laquelle le joueur pousse le cube (il prend la normale au point de contact des deux plans)
        // rb.AddForce(Vector3.forward * forcePoussee, ForceMode.Force);

        rb.AddForce(pushDir * forcePoussee, ForceMode.Force);
    }

}
