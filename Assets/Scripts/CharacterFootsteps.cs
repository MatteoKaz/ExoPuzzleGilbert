using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterFootsteps : MonoBehaviour
{
    [Header("Footstep Sounds - Prologue")]
    [SerializeField] private AudioClip prologueFootstep;

    [Header("Footstep Sounds - Act1")]
    [SerializeField] private AudioClip[] act1Footsteps = new AudioClip[4];

    [Header("Footstep Sounds - Act2")]
    [SerializeField] private AudioClip[] act2Footsteps = new AudioClip[3];

    [Header("Footstep Sounds - Act3")]
    [SerializeField] private AudioClip[] act3Footsteps = new AudioClip[3];

    [Header("Settings")]
    [SerializeField] private float stepInterval = 0.75f;
    [SerializeField] private float minSpeedToPlayFootsteps = 0.1f;
    [SerializeField] private float footstepVolume = 1f;
    [SerializeField] private float footstepVolumeAct1 = 1f;
    [SerializeField] private float footstepVolumeAct2 = 1f;
    [SerializeField] private float footstepVolumeAct3 = 1f;
    [SerializeField] private float footstepVolumePrologue = 1f;

    private AudioSource dedicatedAudioSource;
    private PlayerMovement playerMovement;
    private Rigidbody rb;
    private float stepTimer = 0f;

    void Start()
    {
        GameObject footstepObject = new GameObject("FootstepAudioSource");
        footstepObject.transform.SetParent(transform);
        footstepObject.transform.localPosition = Vector3.zero;

        dedicatedAudioSource = footstepObject.AddComponent<AudioSource>();
        dedicatedAudioSource.playOnAwake = false;
        dedicatedAudioSource.spatialBlend = 1f;
        dedicatedAudioSource.loop = false;

        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (IsMoving())
        {
            stepTimer += Time.deltaTime;

            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f;

            if (dedicatedAudioSource != null && dedicatedAudioSource.isPlaying)
            {
                dedicatedAudioSource.Stop();
            }
        }
    }

    private bool IsMoving()
    {
        if (playerMovement != null)
        {
            return Mathf.Abs(playerMovement.speed) > minSpeedToPlayFootsteps;
        }

        if (rb != null)
        {
            return rb.linearVelocity.magnitude > minSpeedToPlayFootsteps;
        }

        return false;
    }

    private void PlayFootstep()
    {
        if (dedicatedAudioSource.isPlaying) return;

        string currentSceneName = SceneManager.GetActiveScene().name;
        AudioClip selectedClip = null;
        float volumeMultiplier = footstepVolume;

        if (currentSceneName.StartsWith("Acte1"))
        {
            if (act1Footsteps.Length > 0)
            {
                selectedClip = act1Footsteps[Random.Range(0, act1Footsteps.Length)];
                volumeMultiplier *= footstepVolumeAct1;
            }
        }
        else if (currentSceneName.StartsWith("Acte2"))
        {
            if (act2Footsteps.Length > 0)
            {
                selectedClip = act2Footsteps[Random.Range(0, act2Footsteps.Length)];
                volumeMultiplier *= footstepVolumeAct2;
            }
        }
        else if (currentSceneName.StartsWith("Acte3"))
        {
            if (act3Footsteps.Length > 0)
            {
                selectedClip = act3Footsteps[Random.Range(0, act3Footsteps.Length)];
                volumeMultiplier *= footstepVolumeAct3;
            }
        }
        else
        {
            selectedClip = prologueFootstep;
            volumeMultiplier *= footstepVolumePrologue;
        }

        if (selectedClip != null && dedicatedAudioSource != null)
        {
            dedicatedAudioSource.PlayOneShot(selectedClip, volumeMultiplier);
        }
    }

    public void SetStepInterval(float interval)
    {
        stepInterval = interval;
    }

    public void SetFootstepVolume(float volume)
    {
        footstepVolume = volume;
    }

    public void SetFootstepVolumeAct1(float volume)
    {
        footstepVolumeAct1 = volume;
    }

    public void SetFootstepVolumeAct2(float volume)
    {
        footstepVolumeAct2 = volume;
    }

    public void SetFootstepVolumeAct3(float volume)
    {
        footstepVolumeAct3 = volume;
    }

    public void SetFootstepVolumePrologue(float volume)
    {
        footstepVolumePrologue = volume;
    }
}
