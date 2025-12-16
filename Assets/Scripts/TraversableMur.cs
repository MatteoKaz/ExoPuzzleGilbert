using UnityEngine;

public class TraversableMur : MonoBehaviour
{
    private AudioSource audioMur;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioMur = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySoundMur(float volumeSound)
    {
        if (audioMur != null)
        {
            audioMur.volume = volumeSound;
            audioMur.Play();
        }
    }
}
