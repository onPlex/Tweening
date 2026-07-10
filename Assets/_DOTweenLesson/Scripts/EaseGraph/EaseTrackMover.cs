using DG.Tweening;
using UnityEngine;

/// <summary>
/// 지정 Ease로 Start에서 End까지 DOMove 합니다.
/// </summary>
public class EaseTrackMover : MonoBehaviour
{
    [SerializeField] private Transform mover;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Ease easeType = Ease.Linear;
    [SerializeField] private float duration = 2f;

    public void Configure(Transform moverTransform, Transform start, Transform end, Ease ease, float moveDuration)
    {
        mover = moverTransform;
        startPoint = start;
        endPoint = end;
        easeType = ease;
        duration = moveDuration;
    }

    public void Play()
    {
        if (mover == null || startPoint == null || endPoint == null)
        {
            return;
        }

        mover.DOKill();
        mover.position = startPoint.position;
        mover.DOMove(endPoint.position, duration).SetEase(easeType);
    }

    public void ResetMover()
    {
        if (mover == null)
        {
            return;
        }

        mover.DOKill();

        if (startPoint != null)
        {
            mover.position = startPoint.position;
        }
    }
}
