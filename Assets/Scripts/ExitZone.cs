using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ExitZone : MonoBehaviour
{
    public Vector3 positionSpawn;
    [SerializeField] public GameObject positionRespawn;
    private void OnTriggerEnter(Collider other)
    {
        XRGrabInteractable grab = other.GetComponent<XRGrabInteractable>();

        if (grab != null && grab.isSelected == true)
        {
            var interactor = grab.firstInteractorSelecting;

            if (interactor != null)
            {
                other.GetComponent<XRGrabInteractable>().interactionManager.SelectExit(interactor, grab);
            }
        }

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
