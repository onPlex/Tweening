using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// 떠오르며 사라지는 데미지 텍스트입니다.
/// </summary>
public class FloatingDamageText : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;

    private Sequence playSequence;

    public void Configure(TMP_Text text, CanvasGroup group, RectTransform rect)
    {
        damageText = text;
        canvasGroup = group;
        rectTransform = rect;
    }

    public void Play(int damage)
    {
        if (damageText == null || canvasGroup == null || rectTransform == null)
        {
            return;
        }

        TweenKillHelper.KillAndNull(ref playSequence);

        damageText.text = damage.ToString();
        rectTransform.localScale = Vector3.one * 0.8f;
        canvasGroup.alpha = 1f;

        // SetLink: 오브젝트 Destroy 시 Tween도 함께 종료되어 null target 경고를 방지합니다.
        playSequence = DOTween.Sequence().SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        playSequence.Append(rectTransform.DOScale(1.2f, 0.12f).SetEase(Ease.OutBack));
        playSequence.Join(rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + 60f, 0.5f));
        playSequence.Join(canvasGroup.DOFade(0f, 0.5f));
        playSequence.OnComplete(() =>
        {
            TweenKillHelper.KillAndNull(ref playSequence);
            if (this != null && gameObject != null)
            {
                Destroy(gameObject);
            }
        });
    }

    private void OnDestroy()
    {
        TweenKillHelper.KillAndNull(ref playSequence);
        TweenKillHelper.KillOnGameObject(gameObject);
    }
}
