using UnityEngine;
using UnityEngine.XR;

public class VRFootstepController : MonoBehaviour
{
    [Header("Footstep Sounds")]
    [SerializeField] private AudioClip[] footstepSounds = new AudioClip[6];
    [SerializeField] private AudioClip creakSound;

    [Header("Settings")]
    [SerializeField] private float stepDistance = 1.5f;
    [SerializeField] private float creakChance = 0.05f;
    [SerializeField] private float footstepVolume = 1f;
    [SerializeField] private float creakVolume = 1f;

    [Header("VR Camera Reference")]
    [SerializeField] private Transform vrCamera;

    private AudioSource audioSource;
    private Vector3 lastStepPosition;
    private float distanceTraveled;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;

        if (vrCamera == null)
        {
            vrCamera = Camera.main.transform;
        }

        lastStepPosition = new Vector3(vrCamera.position.x, 0, vrCamera.position.z);
    }

    void Update()
    {
        Vector3 currentPosition = new Vector3(vrCamera.position.x, 0, vrCamera.position.z);
        float distanceThisFrame = Vector3.Distance(currentPosition, lastStepPosition);

        distanceTraveled += distanceThisFrame;

        if (distanceTraveled >= stepDistance)
        {
            PlayFootstep();
            distanceTraveled = 0f;
            lastStepPosition = currentPosition;
        }
        else
        {
            lastStepPosition = currentPosition;
        }
    }

    private void PlayFootstep()
    {
        if (footstepSounds.Length == 0) return;

        float randomValue = Random.value;

        if (randomValue < creakChance && creakSound != null)
        {
            audioSource.PlayOneShot(creakSound, creakVolume);
        }
        else
        {
            AudioClip randomFootstep = footstepSounds[Random.Range(0, footstepSounds.Length)];

            if (randomFootstep != null)
            {
                audioSource.PlayOneShot(randomFootstep, footstepVolume);
            }
        }
    }

    public void SetFootstepVolume(float volume)
    {
        footstepVolume = volume;
    }

    public void SetCreakVolume(float volume)
    {
        creakVolume = volume;
    }

    public void SetStepDistance(float distance)
    {
        stepDistance = distance;
    }

    public void SetCreakChance(float chance)
    {
        creakChance = Mathf.Clamp01(chance);
    }
}
