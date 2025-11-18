using UnityEngine;

public class HUDScript : MonoBehaviour
{

    private static HUDScript triggerInstance;
    [SerializeField] public GameObject trackCamera;
    [SerializeField] public Vector3 vector;

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

    private void Update()
    {
        if (trackCamera != null)
        {
            transform.rotation = trackCamera.transform.rotation;
            vector = trackCamera.transform.position;
            transform.position = vector * 1.01f;
        }
    }
}
