using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private LayerMask wallMask;

    [SerializeField] private float cutoutSize = 0.04f;
    [SerializeField] private float falloffSize = 0.01f;

    private void Update()
    {
        if (targetObject == null) return;

        Vector3 cutoutWorldPos = targetObject.position;
        Vector3 offset = cutoutWorldPos - Camera.main.transform.position;
        float distance = offset.magnitude;

        // Raycast vers tous les objets sur le layer wallMask
        RaycastHit[] hits = Physics.RaycastAll(Camera.main.transform.position, offset, distance, wallMask);

        foreach (RaycastHit hit in hits)
        {
            Renderer renderer = hit.transform.GetComponent<Renderer>();
            if (renderer == null) continue;

            Material[] materials = renderer.materials;

            foreach (Material mat in materials)
            {
                // On passe la position dans le monde au shader
                mat.SetVector("_CutoutWorldPos", cutoutWorldPos);
                mat.SetFloat("_CutoutSize", cutoutSize);
                mat.SetFloat("_FalloffSize", falloffSize);
            }
        }
    }
}
