using DG.Tweening;
using UnityEngine;

/// <summary>
/// 06_Game_ItemCollect Scene 컨트롤러입니다.
/// Play 예약 Sequence를 필드로 보관해 Reset 시 지연 Collect 콜백을 취소합니다.
/// </summary>
public class ItemCollectSceneController : MonoBehaviour, IPlayableDemo, IResettableDemo
{
    [SerializeField] private CollectibleItemTween[] coinItems;
    [SerializeField] private CurrencyCounterUI currencyCounter;

    private Sequence playSequence;

    public void Configure(CollectibleItemTween[] items, CurrencyCounterUI counter)
    {
        coinItems = items;
        currencyCounter = counter;
    }

    public void Play()
    {
        if (coinItems == null)
        {
            return;
        }

        // 이전 Play의 DelayedCall/InsertCallback이 남아 있지 않도록 먼저 취소합니다.
        TweenKillHelper.KillAndNull(ref playSequence);
        TweenKillHelper.KillOnGameObject(gameObject);

        playSequence = DOTween.Sequence().SetTarget(gameObject);

        for (int index = 0; index < coinItems.Length; index++)
        {
            int delayIndex = index;
            CollectibleItemTween item = coinItems[delayIndex];
            if (item == null)
            {
                continue;
            }

            // InsertCallback으로 하나의 Sequence에 예약해 Reset 시 일괄 Kill 가능하게 합니다.
            playSequence.InsertCallback(delayIndex * 0.2f, () => item.Collect());
        }
    }

    public void ResetDemo()
    {
        TweenKillHelper.KillAndNull(ref playSequence);
        TweenKillHelper.KillOnGameObject(gameObject);

        currencyCounter?.ResetCounter();

        if (coinItems == null)
        {
            return;
        }

        for (int index = 0; index < coinItems.Length; index++)
        {
            coinItems[index]?.ResetItem();
        }
    }

    public void OnCoinCollected(int value)
    {
        currencyCounter?.AddCoin(value);
    }
}
