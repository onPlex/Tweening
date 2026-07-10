using DG.Tweening;
using UnityEngine;

/// <summary>
/// 팝업 등장/퇴장 Sequence (Fade + Scale OutBack/InBack)를 제어합니다.
/// </summary>
public class PopupTweenController : MonoBehaviour, IPlayableDemo, IResettableDemo
{
    [SerializeField] private CanvasGroup dimGroup;
    [SerializeField] private CanvasGroup popupGroup;
    [SerializeField] private RectTransform popupWindow;
    [SerializeField] private float openDuration = 0.25f;
    [SerializeField] private float closeDuration = 0.15f;

    private Vector3 originalScale;

    private void Awake()
    {
        if (popupWindow != null)
        {
            originalScale = popupWindow.localScale;
        }

        CloseInstant();
    }

    public void Configure(CanvasGroup dim, CanvasGroup popup, RectTransform window)
    {
        dimGroup = dim;
        popupGroup = popup;
        popupWindow = window;

        if (popupWindow != null)
        {
            originalScale = popupWindow.localScale;
        }

        CloseInstant();
    }

    public void Play()
    {
        Open();
    }

    public void ResetDemo()
    {
        CloseInstant();
    }

    public void Open()
    {
        if (popupWindow == null || popupGroup == null || dimGroup == null)
        {
            return;
        }

        popupWindow.DOKill();
        popupGroup.DOKill();
        dimGroup.DOKill();

        popupWindow.gameObject.SetActive(true);
        dimGroup.gameObject.SetActive(true);

        popupWindow.localScale = originalScale * 0.8f;
        popupGroup.alpha = 0f;
        dimGroup.alpha = 0f;
        popupGroup.interactable = true;
        popupGroup.blocksRaycasts = true;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(dimGroup.DOFade(0.6f, openDuration));
        sequence.Join(popupGroup.DOFade(1f, openDuration));
        sequence.Join(popupWindow.DOScale(originalScale, openDuration).SetEase(Ease.OutBack));
    }

    public void Close()
    {
        if (popupWindow == null || popupGroup == null || dimGroup == null)
        {
            return;
        }

        popupWindow.DOKill();
        popupGroup.DOKill();
        dimGroup.DOKill();

        popupGroup.interactable = false;
        popupGroup.blocksRaycasts = false;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(popupWindow.DOScale(originalScale * 0.8f, closeDuration).SetEase(Ease.InBack));
        sequence.Join(popupGroup.DOFade(0f, closeDuration));
        sequence.Join(dimGroup.DOFade(0f, closeDuration));
        sequence.OnComplete(() =>
        {
            popupWindow.gameObject.SetActive(false);
            dimGroup.gameObject.SetActive(false);
        });
    }

    public void CloseInstant()
    {
        if (popupWindow != null)
        {
            popupWindow.DOKill();
            popupWindow.gameObject.SetActive(false);
            popupWindow.localScale = originalScale * 0.8f;
        }

        if (dimGroup != null)
        {
            dimGroup.DOKill();
            dimGroup.gameObject.SetActive(false);
            dimGroup.alpha = 0f;
        }

        if (popupGroup != null)
        {
            popupGroup.DOKill();
            popupGroup.alpha = 0f;
            popupGroup.interactable = false;
            popupGroup.blocksRaycasts = false;
        }
    }
}
