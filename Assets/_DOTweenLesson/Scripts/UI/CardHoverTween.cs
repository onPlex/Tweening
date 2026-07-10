using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 카드 Pointer Enter/Exit/Click Tween입니다.
/// </summary>
public class CardHoverTween : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform cardRect;
    [SerializeField] private Image cardImage;
    [SerializeField] private float hoverOffsetY = 40f;
    [SerializeField] private float hoverScale = 1.12f;
    [SerializeField] private float duration = 0.18f;

    private Vector2 originalAnchoredPosition;
    private Vector3 originalScale;
    private Color originalColor;
    private bool isSelected;
    [SerializeField] private CardHoverSceneController sceneController;

    private void Awake()
    {
        if (cardRect == null)
        {
            cardRect = transform as RectTransform;
        }

        CacheOriginalState();
    }

    public void Configure(RectTransform rect, Image image, CardHoverSceneController controller)
    {
        cardRect = rect;
        cardImage = image;
        sceneController = controller;
        CacheOriginalState();
    }

    private void CacheOriginalState()
    {
        if (cardRect != null)
        {
            originalAnchoredPosition = cardRect.anchoredPosition;
            originalScale = cardRect.localScale;
        }

        if (cardImage != null)
        {
            originalColor = cardImage.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected || cardRect == null)
        {
            return;
        }

        PlayHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected || cardRect == null)
        {
            return;
        }

        PlayHover(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        sceneController?.SelectCard(this);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;

        if (cardRect == null)
        {
            return;
        }

        cardRect.DOKill();

        if (selected)
        {
            cardRect.DOAnchorPos(originalAnchoredPosition + new Vector2(0f, hoverOffsetY + 20f), duration)
                .SetEase(Ease.OutBack);
            cardRect.DOScale(originalScale * (hoverScale + 0.08f), duration).SetEase(Ease.OutBack);

            if (cardImage != null)
            {
                cardImage.DOKill();
                cardImage.DOColor(Color.white, duration);
            }
        }
        else
        {
            PlayHover(false);

            if (cardImage != null)
            {
                cardImage.DOKill();
                cardImage.DOColor(originalColor * 0.65f, duration);
            }
        }
    }

    public void ResetCard()
    {
        isSelected = false;

        if (cardRect != null)
        {
            cardRect.DOKill();
            cardRect.anchoredPosition = originalAnchoredPosition;
            cardRect.localScale = originalScale;
        }

        if (cardImage != null)
        {
            cardImage.DOKill();
            cardImage.color = originalColor;
        }
    }

    private void PlayHover(bool hover)
    {
        cardRect.DOKill();

        if (hover)
        {
            cardRect.DOAnchorPos(originalAnchoredPosition + new Vector2(0f, hoverOffsetY), duration)
                .SetEase(Ease.OutQuad);
            cardRect.DOScale(originalScale * hoverScale, duration).SetEase(Ease.OutQuad);
        }
        else
        {
            cardRect.DOAnchorPos(originalAnchoredPosition, duration).SetEase(Ease.OutQuad);
            cardRect.DOScale(originalScale, duration).SetEase(Ease.OutQuad);
        }
    }
}
