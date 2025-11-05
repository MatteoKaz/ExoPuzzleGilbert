using UnityEngine;

public class Button : MonoBehaviour
{
    private PlayerMovement _MovementRef;
    private bool _Pressed = false;  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _MovementRef = GameObject.Find("Character").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (_Pressed = !false)
        {
            _Pressed = false;
            _MovementRef.speed = 0f;
           
        }
        else
        {
            _Pressed = true;
            _MovementRef.speed = -0.5f;
            
        }
    }

     
}
