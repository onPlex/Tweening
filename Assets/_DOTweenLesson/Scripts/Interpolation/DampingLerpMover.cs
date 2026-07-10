using UnityEngine;

/// <summary>
/// 매 프레임 현재 위치에서 목표로 Lerp하는 감쇠형 보간입니다.
/// duration이 아닌 followSpeed로 접근 속도를 제어합니다.
/// </summary>
public class DampingLerpMover : MonoBehaviour
{
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float followSpeed = 2f;
    [SerializeField] private float arrivalThreshold = 0.01f;

    private bool isPlaying;

    public void Configure(Transform start, Transform end, float speed)
    {
        startPoint = start;
        endPoint = end;
        followSpeed = speed;
    }

    public void Play()
    {
        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("[DampingLerpMover] Start/End Point가 없습니다.");
            return;
        }

        isPlaying = true;
        transform.position = startPoint.position;
    }

    public void ResetMover()
    {
        isPlaying = false;

        if (startPoint != null)
        {
            transform.position = startPoint.position;
        }
    }

    private void Update()
    {
        if (!isPlaying || endPoint == null)
        {
            return;
        }

        transform.position = Vector3.Lerp(
            transform.position,
            endPoint.position,
            Time.deltaTime * followSpeed);

        if (Vector3.Distance(transform.position, endPoint.position) <= arrivalThreshold)
        {
            transform.position = endPoint.position;
        }
    }
}
