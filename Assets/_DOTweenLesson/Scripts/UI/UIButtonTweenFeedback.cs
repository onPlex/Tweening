using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 버튼 클릭 시 Scale Down/Up, Reward(glow+text punch), Invalid Shake 피드백을 재생합니다.
/// </summary>
public class UIButtonTweenFeedback : MonoBehaviour
{
    [SerializeField] private RectTransform target;
    [SerializeField] private Image targetImage;
    [SerializeField] private Image glowImage;
    [SerializeField] private TMP_Text labelText;
    [SerializeField] private float pressedScale = 0.9f;
    [SerializeField] private float downDuration = 0.08f;
    [SerializeField] private float upDuration = 0.12f;

    private Vector3 originalScale;
    private Color originalColor;
    private Color originalGlowColor;
    private Vector3 originalLabelScale;
    private Sequence activeSequence;
    private bool isInitialized;

    private void Awake()
    {
        Initialize();
    }

    public void Configure(RectTransform rectTarget, Image image, Image glow = null, TMP_Text label = null)
    {
        target = rectTarget;
        targetImage = image;
        glowImage = glow;
        labelText = label;
        Initialize();
    }

    private void Initialize()
    {
        if (target == null)
        {
            target = transform as RectTransform;
        }

        if (target == null)
        {
            return;
        }

        originalScale = target.localScale;

        if (targetImage != null)
        {
            originalColor = targetImage.color;
        }

        if (glowImage != null)
        {
            originalGlowColor = glowImage.color;
        }

        if (labelText != null)
        {
            originalLabelScale = labelText.rectTransform.localScale;
        }

        isInitialized = true;
    }

    /// <summary>
    /// 정상 클릭 피드백: 살짝 작아졌다가 OutBack으로 복귀합니다.
    /// </summary>
    public void PlayClick()
    {
        if (!EnsureReady())
        {
            return;
        }

        KillActiveTweens();
        target.localScale = originalScale;

        activeSequence = DOTween.Sequence().SetTarget(gameObject);
        activeSequence.Append(target.DOScale(originalScale * pressedScale, downDuration).SetEase(Ease.OutQuad));
        activeSequence.Append(target.DOScale(originalScale, upDuration).SetEase(Ease.OutBack));
    }

    /// <summary>
    /// Reward 클릭 피드백: scale + glow pulse + text punch를 동시에 보여줍니다.
    /// </summary>
    public void PlayRewardClick()
    {
        if (!EnsureReady())
        {
            return;
        }

        KillActiveTweens();
        target.localScale = originalScale;

        if (glowImage != null)
        {
            glowImage.color = originalGlowColor;
        }

        if (labelText != null)
        {
            labelText.rectTransform.localScale = originalLabelScale;
        }

        activeSequence = DOTween.Sequence().SetTarget(gameObject);
        activeSequence.Append(target.DOScale(originalScale * pressedScale, downDuration).SetEase(Ease.OutQuad));
        activeSequence.Append(target.DOScale(originalScale * 1.08f, upDuration).SetEase(Ease.OutBack));
        activeSequence.Append(target.DOScale(originalScale, 0.1f).SetEase(Ease.OutQuad));

        // Join: scale과 동시에 glow / text punch를 진행합니다.
        if (glowImage != null)
        {
            Color brightGlow = Color.Lerp(originalGlowColor, Color.white, 0.65f);
            brightGlow.a = Mathf.Max(originalGlowColor.a, 0.85f);
            activeSequence.Join(
                glowImage.DOColor(brightGlow, 0.12f)
                    .SetLoops(2, LoopType.Yoyo)
                    .SetEase(Ease.OutQuad));
        }

        if (labelText != null)
        {
            activeSequence.Join(
                labelText.rectTransform.DOPunchScale(Vector3.one * 0.25f, 0.3f, 8, 0.6f));
        }
    }

    /// <summary>
    /// 잘못된 클릭 피드백: 좌우 Shake + 빨강 Flash입니다.
    /// </summary>
    public void PlayInvalid()
    {
        if (!EnsureReady())
        {
            return;
        }

        KillActiveTweens();
        target.localScale = originalScale;
        target.DOShakeAnchorPos(0.25f, new Vector2(20f, 0f), 12, 90f).SetTarget(gameObject);

        if (targetImage != null)
        {
            targetImage.DOKill();
            targetImage.DOColor(Color.red, 0.08f)
                .SetTarget(gameObject)
                .OnComplete(() =>
                {
                    targetImage.DOColor(originalColor, 0.12f).SetTarget(gameObject);
                });
        }
    }

    public void ResetFeedback()
    {
        KillActiveTweens();

        if (target != null)
        {
            target.localScale = originalScale;
        }

        if (targetImage != null)
        {
            targetImage.color = originalColor;
        }

        if (glowImage != null)
        {
            glowImage.color = originalGlowColor;
        }

        if (labelText != null)
        {
            labelText.rectTransform.localScale = originalLabelScale;
        }
    }

    private bool EnsureReady()
    {
        if (!isInitialized)
        {
            Initialize();
        }

        return target != null;
    }

    private void KillActiveTweens()
    {
        TweenKillHelper.KillAndNull(ref activeSequence);
        TweenKillHelper.KillOnGameObject(gameObject);

        if (target != null)
        {
            target.DOKill();
        }

        if (targetImage != null)
        {
            targetImage.DOKill();
        }

        if (glowImage != null)
        {
            glowImage.DOKill();
        }

        if (labelText != null)
        {
            labelText.rectTransform.DOKill();
        }
    }

    private void OnDestroy()
    {
        KillActiveTweens();
    }
}
