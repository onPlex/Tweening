using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 상자 열림 → 팝업 → 코인 플라이 → 재화 증가까지의 종합 Sequence를 구성합니다.
/// Append / Join / Insert 차이를 수업에서 비교할 수 있도록 구성합니다.
/// </summary>
public class RewardSequenceController : MonoBehaviour, IPlayableDemo, IResettableDemo
{
    [SerializeField] private ChestOpenTween chestOpen;
    [SerializeField] private RewardPopupTween rewardPopup;
    [SerializeField] private CurrencyCounterUI currencyCounter;
    [SerializeField] private CoinFlyUIFX coinFlyFx;
    [SerializeField] private TMP_Text rewardAmountText;
    [SerializeField] private RectTransform rewardIcon;
    [SerializeField] private RectTransform[] coinSpawnPoints;
    [SerializeField] private Button claimButton;
    [SerializeField] private int rewardAmount = 100;
    [SerializeField] private int coinFlyCount = 3;

    private Sequence rewardSequence;
    private Vector3 rewardIconOriginalScale;

    private void Awake()
    {
        if (rewardIcon != null)
        {
            rewardIconOriginalScale = rewardIcon.localScale;
        }
    }

    public void Configure(
        ChestOpenTween chest,
        RewardPopupTween popup,
        CurrencyCounterUI counter,
        CoinFlyUIFX flyFx,
        TMP_Text rewardText,
        RectTransform icon,
        RectTransform[] spawnPoints,
        Button claim)
    {
        chestOpen = chest;
        rewardPopup = popup;
        currencyCounter = counter;
        coinFlyFx = flyFx;
        rewardAmountText = rewardText;
        rewardIcon = icon;
        coinSpawnPoints = spawnPoints;
        claimButton = claim;

        if (rewardIcon != null)
        {
            rewardIconOriginalScale = rewardIcon.localScale;
        }
    }

    public void Play()
    {
        PlayRewardCore();
    }

    /// <summary>
    /// Claim 버튼 또는 Play에서 호출하는 핵심 Sequence입니다.
    /// </summary>
    public void PlayRewardCore()
    {
        ResetDemo();

        if (rewardAmountText != null)
        {
            rewardAmountText.text = $"+{rewardAmount}";
        }

        rewardSequence = DOTween.Sequence()
            .SetTarget(gameObject)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        // Append: 앞 단계가 끝난 뒤 상자 열림 Tween (순차)
        Tween chestTween = chestOpen != null ? chestOpen.Open() : null;
        if (chestTween != null)
        {
            rewardSequence.Append(chestTween);
        }

        rewardSequence.AppendInterval(0.15f);

        // Append: 팝업 등장 Sequence (팝업 내부에서 Fade+Scale은 Join으로 구성됨)
        Tween popupTween = rewardPopup != null ? rewardPopup.Show() : null;
        if (popupTween != null)
        {
            rewardSequence.Append(popupTween);
        }

        // Insert: 특정 시각에 아이콘 punch를 끼워 넣음 (Join이 아닌 시점 지정)
        if (rewardIcon != null)
        {
            float insertAt = Mathf.Max(0f, rewardSequence.Duration() - 0.2f);
            rewardSequence.Insert(
                insertAt,
                rewardIcon.DOPunchScale(Vector3.one * 0.28f, 0.3f, 8, 0.5f));
        }

        rewardSequence.AppendInterval(0.1f);

        // Append: 코인 플라이 Sequence (CoinFlyUIFX 내부에서 Insert로 stagger)
        Tween flyTween = PlayCoinFly();
        if (flyTween != null)
        {
            rewardSequence.Append(flyTween);
        }
        else
        {
            rewardSequence.AppendInterval(0.2f);
        }

        // AppendCallback: 수량 반영 + 상단 아이콘 punch
        rewardSequence.AppendCallback(() =>
        {
            currencyCounter?.AddCoin(rewardAmount);
        });
    }

    public void OnClaimClicked()
    {
        PlayRewardCore();
    }

    public void ResetDemo()
    {
        TweenKillHelper.KillAndNull(ref rewardSequence);
        TweenKillHelper.KillOnGameObject(gameObject);

        chestOpen?.ResetChest();
        rewardPopup?.HideInstant();
        currencyCounter?.ResetCounter();
        coinFlyFx?.ResetFx();

        if (rewardAmountText != null)
        {
            rewardAmountText.text = string.Empty;
        }

        if (rewardIcon != null)
        {
            rewardIcon.DOKill();
            rewardIcon.localScale = rewardIconOriginalScale == Vector3.zero
                ? Vector3.one
                : rewardIconOriginalScale;
        }
    }

    private void OnDestroy()
    {
        TweenKillHelper.KillAndNull(ref rewardSequence);
        TweenKillHelper.KillOnGameObject(gameObject);
        coinFlyFx?.ResetFx();
    }

    private Tween PlayCoinFly()
    {
        if (coinFlyFx == null || coinSpawnPoints == null || coinSpawnPoints.Length == 0)
        {
            return null;
        }

        int count = Mathf.Min(coinFlyCount, coinSpawnPoints.Length);
        Vector2[] positions = new Vector2[count];
        for (int index = 0; index < count; index++)
        {
            RectTransform spawn = coinSpawnPoints[index];
            positions[index] = spawn != null ? spawn.anchoredPosition : Vector2.zero;
        }

        return coinFlyFx.PlayFlyFromAnchoredPositions(positions);
    }
}
