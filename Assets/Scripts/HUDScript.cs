using UnityEngine;

public class HUDScript : MonoBehaviour
{

    private static HUDScript triggerInstance;
    void Awake()
    {
            DontDestroyOnLoad(this);

            if (triggerInstance == null)
            {
                triggerInstance = this;
            }
            else
            {
                DestroyObject(gameObject);
            }
        
    }
}
