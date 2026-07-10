using DG.Tweening;
using UnityEngine;

/// <summary>
/// 타격 순간에 잠깐 커졌다가 사라지는 Attack Effect입니다.
/// </summary>
public class AttackEffectTween : MonoBehaviour
{
    [SerializeField] private Transform effectTransform;
    [SerializeField] private SpriteRenderer effectRenderer;
    [SerializeField] private float playDuration = 0.25f;
    [SerializeField] private float maxScale = 1.4f;

    private Vector3 originalScale;
    private Color originalColor;
    private Sequence effectSequence;

    private void Awake()
    {
        CacheState();
        ResetEffect();
    }

    public void Configure(Transform effect, SpriteRenderer renderer)
    {
        effectTransform = effect;
        effectRenderer = renderer;
        CacheState();
        ResetEffect();
    }

    private void CacheState()
    {
        if (effectTransform == null)
        {
            effectTransform = transform;
        }

        originalScale = effectTransform.localScale;

        if (effectRenderer != null)
        {
            originalColor = effectRenderer.color;
        }
    }

    public void Play()
    {
        if (effectTransform == null)
        {
            return;
        }

        TweenKillHelper.KillAndNull(ref effectSequence);

        effectTransform.gameObject.SetActive(true);
        effectTransform.localScale = originalScale * 0.35f;

        if (effectRenderer != null)
        {
            Color startColor = originalColor;
            startColor.a = 1f;
            effectRenderer.color = startColor;
        }

        effectSequence = DOTween.Sequence()
            .SetTarget(gameObject)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        effectSequence.Append(
            effectTransform.DOScale(originalScale * maxScale, playDuration * 0.55f)
                .SetEase(Ease.OutBack));

        if (effectRenderer != null)
        {
            effectSequence.Join(
                effectRenderer.DOFade(0f, playDuration).SetEase(Ease.InQuad));
        }

        effectSequence.OnComplete(() =>
        {
            if (effectTransform != null)
            {
                effectTransform.gameObject.SetActive(false);
            }
        });
    }

    public void ResetEffect()
    {
        TweenKillHelper.KillAndNull(ref effectSequence);
        TweenKillHelper.KillOnGameObject(gameObject);

        if (effectTransform != null)
        {
            effectTransform.DOKill();
            effectTransform.localScale = originalScale;
            effectTransform.gameObject.SetActive(false);
        }

        if (effectRenderer != null)
        {
            effectRenderer.DOKill();
            effectRenderer.color = originalColor;
        }
    }

    private void OnDestroy()
    {
        ResetEffect();
    }
}
