using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SwitchMaterial : MonoBehaviour
{
    private XRGrabInteractable parentGrab;
    public bool isGrabbed = false;
    public Material newMat; //= Resources.Load("Transparant", typeof(Material)) as Material;
    public Material firstMat; //= Resources.Load("Lit", typeof(Material)) as Material;

    private void Awake()
    {
        parentGrab = GetComponentInParent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        if (parentGrab != null)
        {
            parentGrab.selectEntered.AddListener(OnGrab);
            parentGrab.selectExited.AddListener(OnRelease);
        }
    }

    private void OnDisable()
    {
        if (parentGrab != null)
        {
            parentGrab.selectEntered.RemoveListener(OnGrab);
            parentGrab.selectExited.RemoveListener(OnRelease);
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        GetComponent<MeshRenderer>().material = newMat;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        GetComponent<MeshRenderer>().material = firstMat;
    }

}
