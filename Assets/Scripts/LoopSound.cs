using UnityEngine;

public class LoopSound : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        // On s'assure que la boucle est active
        audioSource.loop = true;

        // On programme le démarrage du son dans le moteur audio (DSP)
        // 0.1s de marge pour laisser le temps de charger le buffer
        double startTime = AudioSettings.dspTime + 0.3f;
        audioSource.PlayScheduled(startTime);
    }
}