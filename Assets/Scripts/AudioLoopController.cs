using UnityEngine;

public class AudioLoopController : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Loop Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float loopStartPercent = 0.2f;

    [Range(0f, 1f)]
    [SerializeField] private float loopEndPercent = 0.9f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (audioSource != null && audioSource.isPlaying && audioSource.clip != null)
        {
            float currentPercent = audioSource.time / audioSource.clip.length;

            if (currentPercent >= loopEndPercent)
            {
                audioSource.time = audioSource.clip.length * loopStartPercent;
            }
        }
    }
}
