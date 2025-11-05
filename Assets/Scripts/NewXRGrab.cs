using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Attachment;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class NewXRGrab : MonoBehaviour
{
    private bool isGrabed = false;
    private Transform grabber;
    private Vector3 grabOffset;
    private XRSimpleInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(OnGrab);
        interactable.selectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        interactable.selectEntered.RemoveListener(OnGrab);
        interactable.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabed = true;
        grabber = args.interactorObject.transform;
        grabOffset = transform.position - grabber.position;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabed = false;
        grabber = null;
    }


    //public void StartGrabing(XRBaseInteractor interactor)
   // {
   //     isGrabed = true;
   //     grabber = interactor.transform;
   //     grabOffset = transform.position - grabber.position;
  //  }


 //   public void StopGrabing()
 //   {
 //       isGrabed = false;
 //       grabber = null;
 //   }


    private void Update()
    {
        if (isGrabed && grabber != null)
        {
            transform.position = grabber.position + grabOffset;
        }

    }
}
