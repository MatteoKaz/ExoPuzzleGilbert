using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class AutoColForSprite : MonoBehaviour
{
    [Header("Small Thickness (required for PhysX)")]
    public float thickness = 0.001f;

    void Start()
    {
        GenerateCollider();
    }

    [ContextMenu("Regenerate Collider From Sprite Physics Shape")]
    public void GenerateCollider()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Sprite sprite = sr.sprite;
        MeshCollider mc = GetComponent<MeshCollider>();

        if (sprite == null)
        {
            Debug.LogWarning("No sprite found.");
            return;
        }

        // Get physics shape
        List<Vector2> shape2D = new List<Vector2>();
        sprite.GetPhysicsShape(0, shape2D);
        if (shape2D.Count < 3)
        {
            Debug.LogWarning("Shape has <3 points.");
            return;
        }

        // Triangulate
        int[] triangles2D = Triangulate(shape2D);
        if (triangles2D == null || triangles2D.Length < 3)
        {
            Debug.LogWarning("Triangulation failed.");
            return;
        }

        // -----------------------------------------------
        // BUILD 3D MESH WITH ULTRA SMALL THICKNESS
        // -----------------------------------------------
        int count = shape2D.Count;

        Vector3[] vertices = new Vector3[count * 2];
        List<int> tris = new List<int>();

        float dz = thickness * 0.5f;

        // FRONT vertices (-dz)
        for (int i = 0; i < count; i++)
            vertices[i] = new Vector3(shape2D[i].x, shape2D[i].y, -dz);

        // BACK vertices (+dz)
        for (int i = 0; i < count; i++)
            vertices[i + count] = new Vector3(shape2D[i].x, shape2D[i].y, +dz);

        // FRONT triangles
        for (int i = 0; i < triangles2D.Length; i++)
            tris.Add(triangles2D[i]);

        // BACK triangles (reverse winding)
        for (int i = 0; i < triangles2D.Length; i += 3)
        {
            tris.Add(triangles2D[i] + count);
            tris.Add(triangles2D[i + 2] + count);
            tris.Add(triangles2D[i + 1] + count);
        }

        // SIDE WALLS
        for (int i = 0; i < count; i++)
        {
            int next = (i + 1) % count;

            int A = i;
            int B = next;
            int C = i + count;
            int D = next + count;

            // Quad A-B-D-C as two triangles
            tris.Add(A);
            tris.Add(B);
            tris.Add(D);

            tris.Add(A);
            tris.Add(D);
            tris.Add(C);
        }

        // -----------------------------------------------
        // BUILD FINAL MESH
        // -----------------------------------------------
        Mesh mesh = new Mesh();
        mesh.name = "ColliderMesh";
        mesh.vertices = vertices;
        mesh.triangles = tris.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        mc.sharedMesh = mesh;
        mc.convex = false;

        Debug.Log("MeshCollider generated (tiny thickness applied).");
    }

    // -----------------------------------------------------------
    // Your Ear-Clipping triangulation (unchanged)
    // -----------------------------------------------------------
    private int[] Triangulate(List<Vector2> points)
    {
        int n = points.Count;
        if (n < 3) return null;

        List<int> indices = new List<int>();
        int[] V = new int[n];

        // Orientation
        float area = 0f;
        for (int i = 0; i < n; i++)
        {
            int j = (i + 1) % n;
            area += points[i].x * points[j].y - points[j].x * points[i].y;
        }

        if (area > 0f)
        {
            for (int i = 0; i < n; i++) V[i] = i;
        }
        else
        {
            for (int i = 0; i < n; i++) V[i] = (n - 1) - i;
        }

        int nv = n;
        int v = 0;
        int count = 2 * nv;

        while (nv > 2 && count > 0)
        {
            int u = v;
            if (u >= nv) u = 0;
            v = u + 1;
            if (v >= nv) v = 0;
            int w = v + 1;
            if (w >= nv) w = 0;

            if (Snip(points, u, v, w, nv, V))
            {
                int a = V[u];
                int b = V[v];
                int c = V[w];
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);

                for (int s = v; s < nv - 1; s++)
                    V[s] = V[s + 1];

                nv--;
                count = 2 * nv;
            }
            else
            {
                v++;
            }
            count--;
        }

        if (indices.Count < 3)
            return null;

        return indices.ToArray();
    }

    private bool Snip(List<Vector2> points, int u, int v, int w, int nv, int[] V)
    {
        const float EPS = 1e-6f;

        Vector2 A = points[V[u]];
        Vector2 B = points[V[v]];
        Vector2 C = points[V[w]];

        float area = (B.x - A.x) * (C.y - A.y) - (B.y - A.y) * (C.x - A.x);
        if (area <= EPS) return false;

        for (int p = 0; p < nv; p++)
        {
            if (p == u || p == v || p == w) continue;
            Vector2 P = points[V[p]];
            if (PointInTriangle(P, A, B, C)) return false;
        }
        return true;
    }

    private bool PointInTriangle(Vector2 P, Vector2 A, Vector2 B, Vector2 C)
    {
        float cross1 = (B.x - A.x) * (P.y - A.y) - (B.y - A.y) * (P.x - A.x);
        float cross2 = (C.x - B.x) * (P.y - B.y) - (C.y - B.y) * (P.x - B.x);
        float cross3 = (A.x - C.x) * (P.y - C.y) - (A.y - C.y) * (P.x - C.x);

        bool hasNeg = (cross1 < 0) || (cross2 < 0) || (cross3 < 0);
        bool hasPos = (cross1 > 0) || (cross2 > 0) || (cross3 > 0);

        return !(hasNeg && hasPos);
    }
}


