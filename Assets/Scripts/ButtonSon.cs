using UnityEngine;

public class ButtonSon : MonoBehaviour
{
    [SerializeField] AudioSource _ButtonSon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound()
    {
        _ButtonSon.Play();
    }
}
