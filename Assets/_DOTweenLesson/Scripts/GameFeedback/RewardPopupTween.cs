using DG.Tweening;
using UnityEngine;

/// <summary>
/// 보상 팝업 Fade + Scale 등장 연출입니다.
/// </summary>
public class RewardPopupTween : MonoBehaviour
{
    [SerializeField] private CanvasGroup popupGroup;
    [SerializeField] private RectTransform popupWindow;

    private Vector3 originalScale;

    private void Awake()
    {
        if (popupWindow != null)
        {
            originalScale = popupWindow.localScale;
        }

        HideInstant();
    }

    public void Configure(CanvasGroup group, RectTransform window)
    {
        popupGroup = group;
        popupWindow = window;
        originalScale = popupWindow != null ? popupWindow.localScale : Vector3.one;
        HideInstant();
    }

    public Tween Show()
    {
        if (popupGroup == null || popupWindow == null)
        {
            return null;
        }

        popupWindow.DOKill();
        popupGroup.DOKill();

        popupWindow.gameObject.SetActive(true);
        popupWindow.localScale = originalScale * 0.8f;
        popupGroup.alpha = 0f;

        Sequence sequence = DOTween.Sequence().SetTarget(gameObject);
        // Join: Fade와 Scale을 동시에 진행 (Append와의 차이를 보여주는 예)
        sequence.Append(popupGroup.DOFade(1f, 0.2f));
        sequence.Join(popupWindow.DOScale(originalScale, 0.25f).SetEase(Ease.OutBack));
        return sequence;
    }

    public void HideInstant()
    {
        TweenKillHelper.KillOnGameObject(gameObject);

        if (popupGroup != null)
        {
            popupGroup.DOKill();
            popupGroup.alpha = 0f;
        }

        if (popupWindow != null)
        {
            popupWindow.DOKill();
            popupWindow.localScale = originalScale * 0.8f;
            popupWindow.gameObject.SetActive(false);
        }
    }
}
