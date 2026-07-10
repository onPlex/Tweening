using DG.Tweening;
using UnityEngine;

/// <summary>
/// 상호작용 오브젝트용 Scale Pulse 강조 연출입니다.
/// </summary>
public class AttentionPulseTween : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float pulseScale = 1.2f;
    [SerializeField] private float duration = 0.7f;

    private Vector3 originalScale;

    private void Start()
    {
        if (target == null)
        {
            target = transform;
        }

        originalScale = target.localScale;
    }

    private void OnDestroy()
    {
        StopLoop();
    }

    public void Configure(Transform pulseTarget)
    {
        target = pulseTarget;
        originalScale = target != null ? target.localScale : Vector3.one;
    }

    public void PlayLoop()
    {
        if (target == null)
        {
            return;
        }

        // Play 버튼을 누를 때마다 Pulse 루프를 재시작할 수 있도록 이전 Tween을 정리합니다.
        StopLoop();
        target.DOScale(originalScale * pulseScale, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void StopLoop()
    {
        if (target == null)
        {
            return;
        }

        target.DOKill();
        target.localScale = originalScale;
    }
}
