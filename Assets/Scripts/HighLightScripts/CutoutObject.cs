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

    private List<Material> affectedMaterials = new List<Material>();

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
        if (targetObject != null)
        {
            Vector3 cameraCenter = xrOriginCamera.position;

            Vector2 cutoutPos = renderCamera.WorldToViewportPoint(targetObject.position);

            cutoutPos.x = Mathf.Clamp01(cutoutPos.x);
            cutoutPos.y = Mathf.Clamp01(cutoutPos.y);

            Vector3 offset = targetObject.position - cameraCenter;
            RaycastHit[] hitObjects = Physics.RaycastAll(cameraCenter, offset.normalized, offset.magnitude, wallMask);

            List<Material> currentFrameMaterials = new List<Material>();

            for (int i = 0; i < hitObjects.Length; ++i)
            {
                Renderer renderer = hitObjects[i].transform.GetComponent<Renderer>();
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

                        if (!currentFrameMaterials.Contains(materials[m]))
                        {
                            currentFrameMaterials.Add(materials[m]);
                        }
                    }
                }
            }

            for (int i = affectedMaterials.Count - 1; i >= 0; i--)
            {
                if (affectedMaterials[i] != null && !currentFrameMaterials.Contains(affectedMaterials[i]))
                {
                    affectedMaterials[i].SetFloat("_CutoutSize", 0f);
                    affectedMaterials[i].SetFloat("_FalloffSize", 0f);
                    affectedMaterials.RemoveAt(i);
                }
            }

            affectedMaterials = currentFrameMaterials;
        }
        else
        {
            for (int i = 0; i < affectedMaterials.Count; i++)
            {
                if (affectedMaterials[i] != null)
                {
                    affectedMaterials[i].SetFloat("_CutoutSize", 0f);
                    affectedMaterials[i].SetFloat("_FalloffSize", 0f);
                }
            }
            affectedMaterials.Clear();
        }
    }
}
