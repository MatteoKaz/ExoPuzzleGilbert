using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

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
        List<RaycastHit> allHitObjects = new List<RaycastHit>();

        for (int i = 0; i < hitObjects.Length; ++i)
        {
            if (!allHitObjects.Contains(hitObjects[i]))
            {
                allHitObjects.Add(hitObjects[i]);
            }
        }
        for (int j = 0; j < allHitObjects.Count; ++j)
        {
            if (hitObjects.Contains<RaycastHit>(allHitObjects[j])) 
            {
                Renderer renderer = allHitObjects[j].transform.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material[] materials = renderer.materials;

                    for (int m = 0; m < materials.Length; ++m)
                    {
                        materials[m].SetVector("_CutoutPos", cutoutPos);
                        materials[m].SetFloat("_CutoutSize", 0.2f);
                        materials[m].SetFloat("_FalloffSize", 0.010f);
                        materials[m].SetVector("_CharacterPosition", targetObject.position);
                        materials[m].SetVector("_CameraPosition", (cameraCenter - targetObject.position).normalized);
                    }
                }
            }
            else
            {
                Renderer renderer = allHitObjects[j].transform.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Material[] materials = renderer.materials;

                    for (int m = 0; m < materials.Length; ++m)
                    {
                        materials[m].SetVector("_CutoutPos", cutoutPos);
                        materials[m].SetFloat("_CutoutSize", 0f);
                        materials[m].SetFloat("_FalloffSize", 0f);
                        materials[m].SetVector("_CharacterPosition", targetObject.position);
                        materials[m].SetVector("_CameraPosition", (cameraCenter - targetObject.position).normalized);
                    }
                }
            }
        }
        
    }
}
