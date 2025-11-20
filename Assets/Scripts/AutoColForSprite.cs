using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class AutoColForSprite : MonoBehaviour
{
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
            Debug.LogWarning("[" + name + "] No sprite found on SpriteRenderer.");
            return;
        }

        int shapeCount = sprite.GetPhysicsShapeCount();
        if (shapeCount == 0)
        {
            Debug.LogWarning("[" + name + "] Sprite has no Custom Physics Shape. Open Sprite Editor > Custom Physics Shape.");
            return;
        }

        // On utilise seulement la première shape pour ce script
        List<Vector2> shape2D = new List<Vector2>();
        sprite.GetPhysicsShape(0, shape2D);

        if (shape2D.Count < 3)
        {
            Debug.LogWarning("[" + name + "] Physics Shape has fewer than 3 points.");
            return;
        }

        // Triangulation du polygone 2D
        int[] triangles = Triangulate(shape2D);
        if (triangles == null || triangles.Length < 3)
        {
            Debug.LogWarning("[" + name + "] Triangulation failed.");
            return;
        }

        // Conversion en vertices 3D (z = 0 => mur plat)
        Vector3[] vertices = new Vector3[shape2D.Count];
        for (int i = 0; i < shape2D.Count; i++)
        {
            Vector2 p = shape2D[i];
            vertices[i] = new Vector3(p.x, p.y, 0f);
        }

        Mesh mesh = new Mesh();
        mesh.name = "SpritePhysicsWallMesh";
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        mc.sharedMesh = mesh;

        // IMPORTANT : non-convexe pour garder la forme exacte (pour des murs statiques)
        mc.convex = false;

        Debug.Log("[" + name + "] MeshCollider generated from Sprite Physics Shape.");
    }

    // ------------------------------
    //  Triangulation Ear-Clipping
    // ------------------------------
    private int[] Triangulate(List<Vector2> points)
    {
        int n = points.Count;
        if (n < 3) return null;

        List<int> indices = new List<int>();
        int[] V = new int[n];

        // Orientation du polygone (aire signée)
        float area = 0f;
        for (int i = 0; i < n; i++)
        {
            int j = (i + 1) % n;
            area += points[i].x * points[j].y - points[j].x * points[i].y;
        }

        // Si area > 0 => CCW, sinon on inverse
        if (area > 0f)
        {
            for (int i = 0; i < n; i++)
                V[i] = i;
        }
        else
        {
            for (int i = 0; i < n; i++)
                V[i] = (n - 1) - i;
        }

        int nv = n;
        int count = 2 * nv;   // sécurité anti boucle infinie
        int v = 0;

        while (nv > 2 && count > 0)
        {
            int u = v;
            if (nv <= u) u = 0;
            v = u + 1;
            if (nv <= v) v = 0;
            int w = v + 1;
            if (nv <= w) w = 0;

            if (Snip(points, u, v, w, nv, V))
            {
                int a = V[u];
                int b = V[v];
                int c = V[w];

                indices.Add(a);
                indices.Add(b);
                indices.Add(c);

                // On retire le vertex "v"
                for (int s = v; s < nv - 1; s++)
                {
                    V[s] = V[s + 1];
                }
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
        const float EPSILON = 1e-6f;

        Vector2 A = points[V[u]];
        Vector2 B = points[V[v]];
        Vector2 C = points[V[w]];

        // Aire du triangle
        float area = (B.x - A.x) * (C.y - A.y) - (B.y - A.y) * (C.x - A.x);
        if (area <= EPSILON)
            return false;

        // Vérifier qu'aucun autre point n'est à l'intérieur du triangle
        for (int p = 0; p < nv; p++)
        {
            if (p == u || p == v || p == w) continue;
            Vector2 P = points[V[p]];
            if (PointInTriangle(P, A, B, C))
                return false;
        }

        return true;
    }

    private bool PointInTriangle(Vector2 P, Vector2 A, Vector2 B, Vector2 C)
    {
        float ax = C.x - B.x;
        float ay = C.y - B.y;
        float bx = A.x - C.x;
        float by = A.y - C.y;
        float cx = B.x - A.x;
        float cy = B.y - A.y;
        float apx = P.x - A.x;
        float apy = P.y - A.y;
        float bpx = P.x - B.x;
        float bpy = P.y - B.y;
        float cpx = P.x - C.x;
        float cpy = P.y - C.y;

        float aCROSSbp = ax * bpy - ay * bpx;
        float cCROSSap = cx * apy - cy * apx;
        float bCROSScp = bx * cpy - by * cpx;

        return (aCROSSbp >= 0f) && (bCROSScp >= 0f) && (cCROSSap >= 0f);
    }
}