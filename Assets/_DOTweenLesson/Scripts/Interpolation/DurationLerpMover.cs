using UnityEngine;

/// <summary>
/// duration 기반 선형 보간으로 시작점에서 끝점으로 이동합니다.
/// </summary>
public class DurationLerpMover : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float duration = 2f;

    private float elapsedTime;
    private bool isPlaying;

    public void Configure(Transform start, Transform end, float moveDuration)
    {
        startPoint = start;
        endPoint = end;
        duration = moveDuration;
    }

    public void Play()
    {
        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("[DurationLerpMover] Start/End Point가 없습니다.");
            return;
        }

        elapsedTime = 0f;
        isPlaying = true;
        transform.position = startPoint.position;
    }

    public void ResetMover()
    {
        elapsedTime = 0f;
        isPlaying = false;

        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }
    }

    private void Update()
    {
        if (!isPlaying || startPoint == null || endPoint == null)
        {
            return;
        }

        elapsedTime += Time.deltaTime;
        float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, normalizedTime);

        if (normalizedTime >= 1f)
        {
            isPlaying = false;
        }
    }
}
