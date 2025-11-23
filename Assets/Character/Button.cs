using UnityEngine;

public class Button : MonoBehaviour
{
    private PlayerMovement _MovementRef;
    private bool _Pressed = true;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _MovementRef = GameObject.Find("Character").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartPush()
    {
        if (_MovementRef.speed == 0)
        {
            
            _MovementRef.speed = -0.25f;
            _MovementRef.speedOriginal = -0.25f; 
            _Pressed = false;

        }
        else 
        {
            _MovementRef.speed = 0f;
            _MovementRef.speedOriginal = 0f;
            _Pressed = true;
            Debug.Log("off");
        }

            
        
    }

     
}
