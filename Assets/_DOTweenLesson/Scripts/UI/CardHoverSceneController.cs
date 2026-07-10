using UnityEngine;

/// <summary>
/// 10_CardHover_Select Scene 컨트롤러입니다.
/// </summary>
public class CardHoverSceneController : MonoBehaviour, IPlayableDemo, IResettableDemo
{
    [SerializeField] private CardHoverTween[] cards;

    public void Configure(CardHoverTween[] cardTweens)
    {
        cards = cardTweens;
    }

    public void Play()
    {
        if (cards == null || cards.Length == 0)
        {
            return;
        }

        SelectCard(cards[0]);
    }

    public void ResetDemo()
    {
        if (cards == null)
        {
            return;
        }

        for (int index = 0; index < cards.Length; index++)
        {
            cards[index]?.ResetCard();
        }
    }

    public void SelectCard(CardHoverTween selectedCard)
    {
        if (cards == null)
        {
            return;
        }

        for (int index = 0; index < cards.Length; index++)
        {
            CardHoverTween card = cards[index];
            if (card == null)
            {
                continue;
            }

            card.SetSelected(card == selectedCard);
        }
    }
}
