using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField]
    private Transform targetObject;

    [SerializeField]
    private LayerMask wallMask;

    [SerializeField]
    private Transform xrOriginCamera;

    private Camera renderCamera;

    private void Awake()
    {
        renderCamera = GetComponent<Camera>();

        if (xrOriginCamera == null)
        {
            xrOriginCamera = transform;
        }
    }

    private void Update()
    {
        Vector3 cameraCenter = xrOriginCamera.position;

        Vector2 cutoutPos = renderCamera.WorldToViewportPoint(targetObject.position);

        cutoutPos.x = Mathf.Clamp01(cutoutPos.x);
        cutoutPos.y = Mathf.Clamp01(cutoutPos.y);

        Vector3 offset = targetObject.position - cameraCenter;
        RaycastHit[] hitObjects = Physics.RaycastAll(cameraCenter, offset.normalized, offset.magnitude, wallMask);

        for (int i = 0; i < hitObjects.Length; ++i)
        {
            Renderer renderer = hitObjects[i].transform.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material[] materials = renderer.materials;

                for (int m = 0; m < materials.Length; ++m)
                {
                    materials[m].SetVector("_CutoutPos", cutoutPos);
                    materials[m].SetFloat("_CutoutSize", 0.04f);
                    materials[m].SetFloat("_FalloffSize", 0.010f);
                }
            }
        }
    }
}
