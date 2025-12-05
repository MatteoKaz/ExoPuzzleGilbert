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

    [Header("Audio Settings")]
    [SerializeField] private AudioClip magnetismSound;
    [SerializeField] private float minVolume = 0.3f;
    [SerializeField] private float maxVolume = 1f;
    private AudioSource audioSource;
    private bool isPlayerAttracted = false;
    private Transform playerTransform;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        
        if (magnetismSound != null)
        {
            audioSource.clip = magnetismSound;
        }
    }

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
                        if (!isPlayerAttracted)
                        {
                            playerTransform = hit.transform;
                            PlayMagnetismSound();
                        }
                        
                        UpdateVolumeBasedOnDistance(hit.distance);
                        
                        Rigidbody rb = hit.transform.gameObject.GetComponent<Rigidbody>();
                        hit.transform.GetComponent<PlayerMovement>().controlGravity = 0f;
                        rb.useGravity = false;
                        rb.AddForce(-directionAimant * vitesseDePoussee, ForceMode.VelocityChange);
                    }
                }
                else if (hit.transform.gameObject.GetComponent<Traversable>())
                {
                    Debug.Log("je hit traversable");
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
                                if (!isPlayerAttracted)
                                {
                                    playerTransform = hit.transform;
                                    PlayMagnetismSound();
                                }
                                
                                UpdateVolumeBasedOnDistance(hit.distance);
                                
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
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CanBeAimanted>())
        {
            if (other.GetComponent<PlayerMovement>() != null)
            {
                StopMagnetismSound();
                
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

    private void PlayMagnetismSound()
    {
        if (audioSource != null && magnetismSound != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            isPlayerAttracted = true;
        }
    }

    private void StopMagnetismSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            isPlayerAttracted = false;
            playerTransform = null;
        }
    }

    private void UpdateVolumeBasedOnDistance(float distance)
    {
        if (audioSource != null && isPlayerAttracted)
        {
            float normalizedDistance = Mathf.Clamp01(distance / distanceMagnétisme);
            float volume = Mathf.Lerp(maxVolume, minVolume, normalizedDistance);
            audioSource.volume = volume;
        }
    }
}
