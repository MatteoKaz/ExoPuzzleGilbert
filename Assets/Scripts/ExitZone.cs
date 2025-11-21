using UnityEngine;

public class ExitZone : MonoBehaviour
{
    public Vector3 positionSpawn;
    [SerializeField] public GameObject positionRespawn;
    private void OnTriggerExit(Collider other)
    {
        if (positionRespawn  != null)
        {
           other.transform.position = positionRespawn.transform.position;
        }
        else
        {
           other.transform.position = positionSpawn;
        }
             
    }
}
