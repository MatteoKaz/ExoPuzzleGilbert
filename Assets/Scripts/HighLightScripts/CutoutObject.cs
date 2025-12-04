using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CutoutObject : MonoBehaviour
{
    [SerializeField]
    private Transform targetObject;

    [SerializeField]
    private LayerMask wallMask;

    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private bool useXRCenterEye = true;

    private Transform xrCameraTransform;

    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = GetComponent<Camera>();
        }

        xrCameraTransform = mainCamera.transform;
    }

    private void Update()
    {
        Vector3 referencePosition = GetReferencePosition();
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);

        Vector3 offset = targetObject.position - referencePosition;
        RaycastHit[] hitObjects = Physics.RaycastAll(referencePosition, offset, offset.magnitude, wallMask);

        for (int i = 0; i < hitObjects.Length; ++i)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            for (int m = 0; m < materials.Length; ++m)
            {
                materials[m].SetVector("_CutoutPos", cutoutPos);
                materials[m].SetFloat("_CutoutSize", 0.04f);
                materials[m].SetFloat("_FalloffSize", 0.010f);
            }
        }
    }

    private Vector3 GetReferencePosition()
    {
        if (!useXRCenterEye || !XRSettings.enabled)
        {
            return mainCamera.transform.position;
        }

        Vector3 leftEyePos = InputDevices.GetDeviceAtXRNode(XRNode.LeftEye).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 leftPos) ? leftPos : Vector3.zero;
        Vector3 rightEyePos = InputDevices.GetDeviceAtXRNode(XRNode.RightEye).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 rightPos) ? rightPos : Vector3.zero;

        if (leftEyePos != Vector3.zero && rightEyePos != Vector3.zero)
        {
            Vector3 centerEyeLocal = (leftEyePos + rightEyePos) * 0.5f;
            return xrCameraTransform.TransformPoint(centerEyeLocal);
        }

        return xrCameraTransform.position;
    }
}
