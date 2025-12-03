using System.Linq;
using UnityEngine;

public class Aimantage : MonoBehaviour
{
    public float distanceMagnétisme = 3f;
    public float vitesseDePoussee = 0.2f;
    public float liftForce = 0f;
    private Vector3 pointSortie = Vector3.zero;
    public PlayerMovement movePlayer;
    public Rigidbody rb;
    public bool directionBas;


    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<CanBeAimanted>() != null)
        {
            var directionAimant = other.transform.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionAimant, out hit, distanceMagnétisme))
            {
                Debug.DrawRay(transform.position, directionAimant * hit.distance, Color.blue);

                if (hit.transform.GetComponent<PlayerMovement>() != null)
                {
                    if (hit.transform.GetComponent<PlayerMovement>().speed != 0)
                    {
                        Rigidbody rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                        hit.transform.GetComponent<PlayerMovement>().controlGravity = 0f;
                        rb.useGravity = false;
                        rb.AddForce(-directionAimant * vitesseDePoussee, ForceMode.VelocityChange);
                        //   rb.AddForce(Vector3.up * liftForce, ForceMode.VelocityChange);
                    }
                }
                else if (hit.transform.gameObject.GetComponent<Traversable>())
                {
                    Debug.Log("je hit traversable");
                    //Vector3 center = hit.transform.TransformPoint(hit.center);
                    Vector3 center = new Vector3(transform.position.x, hit.transform.position.y, hit.transform.position.z);
                    Vector3 impact = hit.point;
                    Vector3 betweenImpact = impact - center;


                    if (directionBas == true)
                    {
                        Vector3 impactOpposee = new Vector3(betweenImpact.x, betweenImpact.y * -1, betweenImpact.z);
                        pointSortie = center - impactOpposee;
                    }
                    else 
                    {
                        Vector3 impactOpposee = new Vector3(betweenImpact.x, betweenImpact.y, betweenImpact.z * -1);
                        pointSortie = center - impactOpposee;
                    }

                    float sphereHeight = Vector3.Distance(impact, pointSortie);
                    float newDistance = distanceMagnétisme - hit.distance - sphereHeight;



                    Collider[] overlaps1 = Physics.OverlapSphere(pointSortie, 0.01f);
                    Collider[] filtered1 = overlaps1
                    .Where(col => col.GetComponent<NoAffectByRaycastDetect>() == null)
                    .ToArray();

                    if (filtered1.Length > 0)
                    {
                        
                        foreach (Collider col in filtered1)
                        {
                            Debug.Log(col);
                        }
                    }
                    else
                    {

                        if (Physics.Raycast(pointSortie, transform.TransformDirection(directionAimant), out hit, newDistance))
                        {
                            Debug.Log("je suis au point de sortie");
                            Debug.DrawRay(pointSortie, transform.TransformDirection(directionAimant) * hit.distance, Color.red);
                            if (hit.transform.gameObject.GetComponent<PlayerMovement>() != null)
                            {
                                Rigidbody rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                                hit.transform.GetComponent<PlayerMovement>().controlGravity = 0f;
                                rb.useGravity = false;
                                rb.AddForce(-directionAimant * vitesseDePoussee, ForceMode.VelocityChange);

                            }
                        }
                        else
                        {
                            Debug.DrawRay(pointSortie, transform.TransformDirection(directionAimant) * newDistance, Color.magenta);
                        }
                    }
                }
                else
                {
                    if (hit.transform.GetComponent<CanBeAimanted>() != null)
                    {
                        Debug.Log("Hit autre un can be aimanted");
                        Rigidbody rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                        rb.useGravity = false;
                        rb.AddForce(-directionAimant * vitesseDePoussee, ForceMode.VelocityChange);
                    }

                    else
                    {
                        Debug.Log("Il y a un obstacle entre l'aimant et la cible");
                    }//   rb.AddForce(Vector3.up * liftForce, ForceMode.VelocityChange);
                }


               
                //PlayerMovement movePlayer = other.transform.gameObject.GetComponent<PlayerMovement>();

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CanBeAimanted>())
        {
            if (other.GetComponent<PlayerMovement>() != null)
            {
                other.GetComponent<PlayerMovement>().controlGravity = 1f;
                Rigidbody rb = other.transform.gameObject.GetComponent<Rigidbody>();
                rb.useGravity = true;

            }
            else
            {
                Rigidbody rb = other.transform.gameObject.GetComponent<Rigidbody>();
                rb.useGravity = true;
            }

        }
    }
}
