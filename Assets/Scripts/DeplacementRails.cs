using UnityEngine;

public class DeplacementRails : MonoBehaviour
{
    void FixedUpdate()
    {
        //Vector3 pos = transform.position;
        Vector3 pos = transform.localPosition;
        pos.z = Mathf.Clamp(pos.z, -1f, -0.15f); // Limit X axis
        transform.localPosition = pos;
    }
}
