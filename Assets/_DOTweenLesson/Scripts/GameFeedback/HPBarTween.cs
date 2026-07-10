using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HP Fill Amount를 Tween으로 감소시킵니다.
/// </summary>
public class HPBarTween : MonoBehaviour
{
    [SerializeField] private Image hpFill;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private float currentHp = 100f;
    [SerializeField] private float maxHp = 100f;

    public void Configure(Image fill, TMP_Text text, float max)
    {
        hpFill = fill;
        hpText = text;
        maxHp = max;
        currentHp = max;
        RefreshInstant();
    }

    public void TakeDamage(float damage)
    {
        currentHp = Mathf.Max(0f, currentHp - damage);
        float fillAmount = currentHp / maxHp;

        if (hpFill != null)
        {
            hpFill.DOKill();
            hpFill.DOFillAmount(fillAmount, 0.25f).SetEase(Ease.OutQuad);
        }

        if (hpText != null)
        {
            hpText.text = $"{Mathf.CeilToInt(currentHp)} / {Mathf.CeilToInt(maxHp)}";
        }
    }

    public void ResetHp()
    {
        currentHp = maxHp;
        RefreshInstant();
    }

    private void RefreshInstant()
    {
        if (hpFill != null)
        {
            hpFill.DOKill();
            hpFill.fillAmount = 1f;
        }

        if (hpText != null)
        {
            hpText.text = $"{Mathf.CeilToInt(currentHp)} / {Mathf.CeilToInt(maxHp)}";
        }
    }
}
