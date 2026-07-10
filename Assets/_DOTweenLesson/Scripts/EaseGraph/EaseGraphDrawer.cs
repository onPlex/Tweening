using DG.Tweening;
using UnityEngine;

/// <summary>
/// LineRenderer로 Ease 곡선 그래프를 그립니다.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class EaseGraphDrawer : MonoBehaviour
{
    [SerializeField] private Ease easeType = Ease.Linear;
    [SerializeField] private int sampleCount = 40;
    [SerializeField] private float graphWidth = 2f;
    [SerializeField] private float graphHeight = 1.5f;
    [SerializeField] private Color lineColor = Color.cyan;

    private LineRenderer lineRenderer;

    public void Configure(Ease ease, Color color, float width, float height, int samples)
    {
        easeType = ease;
        lineColor = color;
        graphWidth = width;
        graphHeight = height;
        sampleCount = samples;
        DrawGraph();
    }

    private void Awake()
    {
        DrawGraph();
    }

    public void DrawGraph()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.positionCount = sampleCount;
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.useWorldSpace = false;
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

        for (int index = 0; index < sampleCount; index++)
        {
            float normalizedTime = index / (float)(sampleCount - 1);
            float easedValue = DOVirtual.EasedValue(0f, 1f, normalizedTime, easeType);
            float localX = normalizedTime * graphWidth;
            float localY = easedValue * graphHeight;
            lineRenderer.SetPosition(index, new Vector3(localX, localY, 0f));
        }
    }
}
