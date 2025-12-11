using UnityEngine;

public class VFXScript : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform startPoint;
    public Vector3 width;
    public Transform endPoint;

    [Header("Line Settings")]
    public LineRenderer lineRenderer;
    public int lineSegments = 10;
    public float lineWidth = 0.1f;

    [Header("Particle Settings")]
    public ParticleSystem particleEffect;
    public int particlesPerSegment = 2;

    private ParticleSystem.EmitParams emitParams;

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = lineSegments;
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
        }

        emitParams = new ParticleSystem.EmitParams();
    }

    void Update()
    {
        if (startPoint == null || endPoint == null)
            return;

        UpdateLine();
        EmitParticlesAlongLine();
    }

    void UpdateLine()
    {
        if (lineRenderer == null)
            return;

        Vector3 startPos = startPoint.position;
        Vector3 endPos = endPoint.position;

        for (int i = 0; i < lineSegments; i++)
        {
            float t = i / (float)(lineSegments - 1);
            Vector3 position = Vector3.Lerp(startPos, endPos, t);
            lineRenderer.SetPosition(i, position);
        }
    }

    void EmitParticlesAlongLine()
    {
        if (particleEffect == null)
            return;

        Vector3 startPos = startPoint.position - width;
        Vector3 endPos = endPoint.position;

        for (int i = 0; i < particlesPerSegment; i++)
        {
            float t = Random.Range(0f, 1f);
            Vector3 position = Vector3.Lerp(startPos, endPos, t);

            emitParams.position = position;
            particleEffect.Emit(emitParams, 1);
        }
    }
}
