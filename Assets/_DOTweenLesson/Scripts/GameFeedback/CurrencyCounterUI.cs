using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// 재화 수량 표시와 아이콘 Punch Scale을 담당합니다.
/// </summary>
public class CurrencyCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private RectTransform iconTarget;

    private int currentAmount;

    public void Configure(TMP_Text text, RectTransform icon)
    {
        amountText = text;
        iconTarget = icon;
        currentAmount = 0;
        RefreshText();
    }

    public void AddCoin(int value)
    {
        currentAmount += value;
        RefreshText();

        if (iconTarget == null)
        {
            return;
        }

        iconTarget.DOKill();
        iconTarget.localScale = Vector3.one;
        iconTarget.DOScale(1.25f, 0.12f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuad);
    }

    public void ResetCounter()
    {
        currentAmount = 0;
        RefreshText();

        if (iconTarget != null)
        {
            iconTarget.DOKill();
            iconTarget.localScale = Vector3.one;
        }
    }

    private void RefreshText()
    {
        if (amountText != null)
        {
            amountText.text = currentAmount.ToString();
        }
    }
}
