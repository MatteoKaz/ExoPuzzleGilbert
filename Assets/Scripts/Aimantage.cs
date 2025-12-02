using UnityEngine;

public class Aimantage : MonoBehaviour
{
    public float distanceMagnétisme = 3f;
    public float vitesseDePoussee = 0.2f;
    public float liftForce = 0f;
    

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
                else
                {
                    if (hit.transform.GetComponent<Rigidbody>() != null)
                    {
                        Rigidbody rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                        rb.useGravity = false;
                        rb.AddForce(-directionAimant * vitesseDePoussee, ForceMode.VelocityChange);
                    }
                 //   rb.AddForce(Vector3.up * liftForce, ForceMode.VelocityChange);
                }
            }
            else
            {
                Debug.Log("Il y a un obstacle entre l'aimant et la cible");
            }
                //PlayerMovement movePlayer = other.transform.gameObject.GetComponent<PlayerMovement>();
                
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
