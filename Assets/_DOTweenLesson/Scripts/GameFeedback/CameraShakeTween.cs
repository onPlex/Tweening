using DG.Tweening;
using UnityEngine;

/// <summary>
/// 카메라 DOShakePosition을 재생합니다.
/// </summary>
public class CameraShakeTween : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float duration = 0.2f;
    [SerializeField] private float strength = 0.25f;
    [SerializeField] private int vibrato = 18;

    private Vector3 originalPosition;

    private void Awake()
    {
        if (cameraTarget == null)
        {
            cameraTarget = transform;
        }

        originalPosition = cameraTarget.localPosition;
    }

    public void Configure(Transform target, float shakeDuration, float shakeStrength)
    {
        cameraTarget = target;
        duration = shakeDuration;
        strength = shakeStrength;
        originalPosition = cameraTarget != null ? cameraTarget.localPosition : Vector3.zero;
    }

    public void PlayShake()
    {
        if (cameraTarget == null)
        {
            return;
        }

        cameraTarget.DOKill();
        cameraTarget.localPosition = originalPosition;
        cameraTarget.DOShakePosition(duration, strength, vibrato, 90f, false, true)
            .OnComplete(() =>
            {
                cameraTarget.localPosition = originalPosition;
            });
    }

    public void ResetShake()
    {
        if (cameraTarget == null)
        {
            return;
        }

        cameraTarget.DOKill();
        cameraTarget.localPosition = originalPosition;
    }
}
