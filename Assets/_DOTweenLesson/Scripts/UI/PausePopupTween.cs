using DG.Tweening;
using UnityEngine;

/// <summary>
/// Time.timeScale = 0 상태에서도 동작하는 Pause 팝업 Tween입니다.
/// SetUpdate(true)로 unscaled time을 사용합니다.
/// </summary>
public class PausePopupTween : MonoBehaviour
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

    public void Show()
    {
        if (popupGroup == null || popupWindow == null)
        {
            return;
        }

        popupWindow.gameObject.SetActive(true);
        popupWindow.localScale = Vector3.one * 0.8f;
        popupGroup.alpha = 0f;
        popupGroup.interactable = true;
        popupGroup.blocksRaycasts = true;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(popupGroup.DOFade(1f, 0.2f));
        sequence.Join(popupWindow.DOScale(1f, 0.25f).SetEase(Ease.OutBack));
        sequence.SetUpdate(true);
    }

    public void Hide()
    {
        if (popupGroup == null || popupWindow == null)
        {
            return;
        }

        popupGroup.interactable = false;
        popupGroup.blocksRaycasts = false;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(popupWindow.DOScale(0.8f, 0.15f).SetEase(Ease.InBack));
        sequence.Join(popupGroup.DOFade(0f, 0.15f));
        sequence.SetUpdate(true);
        sequence.OnComplete(() =>
        {
            popupWindow.gameObject.SetActive(false);
        });
    }

    public void HideInstant()
    {
        if (popupGroup != null)
        {
            popupGroup.DOKill();
            popupGroup.alpha = 0f;
            popupGroup.interactable = false;
            popupGroup.blocksRaycasts = false;
        }

        if (popupWindow != null)
        {
            popupWindow.DOKill();
            popupWindow.localScale = originalScale * 0.8f;
            popupWindow.gameObject.SetActive(false);
        }
    }
}
