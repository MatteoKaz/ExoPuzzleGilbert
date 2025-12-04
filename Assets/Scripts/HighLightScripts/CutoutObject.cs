using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutoutObject : MonoBehaviour
{
    [SerializeField]
    private Transform targetObject;

    [SerializeField]
    private LayerMask wallMask;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        Vector2 cutoutPos = mainCamera.WorldToViewportPoint(targetObject.position);
     //   Ray ray = CustomScreenPointToRay(targetObject.position);
      //  Vector3 newCutoutPos = CustomScreenToWorldPoint(targetObject.position);


        // Vector3 leftPos = WorldToViewportStereo(mainCamera, targetObject.position, Camera.StereoscopicEye.Left);
        // Vector3 rightPos = WorldToViewportStereo(mainCamera, targetObject.position, Camera.StereoscopicEye.Right);

        Vector3 leftPos = WorldToViewportStereo(mainCamera, targetObject.position, Camera.StereoscopicEye.Left);
        Vector3 rightPos = WorldToViewportStereo(mainCamera, targetObject.position, Camera.StereoscopicEye.Right);

        // Moyenne pour centrer sur les deux yeux
        Vector3 centerPos = (leftPos + rightPos) * 0.5f;


        Vector3 offset = targetObject.position - mainCamera.transform.position;
        RaycastHit[] hitObjects = Physics.RaycastAll(mainCamera.transform.position, offset, offset.magnitude, wallMask);

        for (int i = 0; i < hitObjects.Length; ++i)
        {
            Material[] materials = hitObjects[i].transform.GetComponent<Renderer>().materials;

            for (int m = 0; m < materials.Length; ++m)
            {

               // materials[m].SetVector("_CutoutPosLeft", leftPos);
              //  materials[m].SetVector("_CutoutPosRight", rightPos);
                materials[m].SetVector("_CutoutPos", centerPos);
             //   materials[m].SetVector("_CutoutPos", newCutoutPos);
                materials[m].SetFloat("_CutoutSize", 0.04f);
                materials[m].SetFloat("_FalloffSize", 0.010f);
            }
        }
    }

    private Vector3 WorldToViewportStereo(Camera cam, Vector3 worldPos, Camera.StereoscopicEye eye)
    {
        Matrix4x4 vp = cam.GetStereoProjectionMatrix(eye) * cam.GetStereoViewMatrix(eye);

        Vector4 clip = vp * new Vector4(worldPos.x, worldPos.y, worldPos.z, 1f);

        clip.x /= clip.w;
        clip.y /= clip.w;
        clip.z /= clip.w;

        return new Vector3(
            (clip.x + 1f) * 0.5f,
            (clip.y + 1f) * 0.5f,
            clip.z
        );
    }


   /* public Ray CustomScreenPointToRay(Vector3 screenPoint)
    {
        // スクリーン空間の点をワールド空間に変換（自作の関数を使用）
        Vector3 pointInWorldSpace = CustomScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, mainCamera.nearClipPlane));

        // レイの方向を計算
        Vector3 rayDirection = pointInWorldSpace - mainCamera.transform.position;

        // レイを生成
        return new Ray(mainCamera.transform.position, rayDirection);
    }

    public Vector3 CustomScreenToWorldPoint(Vector3 screenPoint)
    {
        // スクリーン座標の正規化
        Vector3 normalizedPoint = new Vector3(screenPoint.x / Screen.width, screenPoint.y / Screen.height, screenPoint.z);

        // ビューポート座標への変換（透視投影の逆変換は省略）
        Vector3 viewportPoint = mainCamera.ViewportToWorldPoint(normalizedPoint);

        // ワールド空間への変換（カメラの位置と向きを考慮）
        // （透視投影の完全な逆変換を行うには、カメラの透視投影行列の逆行列計算が必要）

        return viewportPoint;
    }*/


}