using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI 코인이 시작점에서 재화 아이콘으로 날아가는 연출입니다.
/// </summary>
public class CoinFlyUIFX : MonoBehaviour
{
    [SerializeField] private RectTransform coinFlyRoot;
    [SerializeField] private RectTransform coinIconTarget;
    [SerializeField] private Image coinTemplate;
    [SerializeField] private float moveDuration = 0.45f;
    [SerializeField] private float startScale = 0.7f;

    private Sequence flySequence;

    public void Configure(RectTransform root, RectTransform target, Image template)
    {
        coinFlyRoot = root;
        coinIconTarget = target;
        coinTemplate = template;
    }

    /// <summary>
    /// spawnAnchoredPositions의 UI 좌표에서 코인을 생성해 coinIcon으로 이동시킵니다.
    /// </summary>
    public Tween PlayFlyFromAnchoredPositions(Vector2[] spawnAnchoredPositions)
    {
        ResetFx();

        if (coinFlyRoot == null || coinIconTarget == null || coinTemplate == null)
        {
            return null;
        }

        flySequence = DOTween.Sequence()
            .SetTarget(gameObject)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        Vector2 targetAnchored = WorldToLocalAnchored(coinIconTarget);

        for (int index = 0; index < spawnAnchoredPositions.Length; index++)
        {
            Image coin = CreateCoin(spawnAnchoredPositions[index]);
            RectTransform coinRect = coin.rectTransform;
            float delay = index * 0.08f;

            Sequence coinSequence = DOTween.Sequence().SetLink(coin.gameObject, LinkBehaviour.KillOnDestroy);
            coinSequence.Append(
                coinRect.DOAnchorPos(targetAnchored, moveDuration).SetEase(Ease.InQuad));
            coinSequence.Join(
                coinRect.DOScale(0.2f, moveDuration).SetEase(Ease.InBack));
            coinSequence.OnComplete(() =>
            {
                if (coin != null && coin.gameObject != null)
                {
                    coin.transform.DOKill();
                    Destroy(coin.gameObject);
                }
            });

            flySequence.Insert(delay, coinSequence);
        }

        return flySequence;
    }

    public void ResetFx()
    {
        TweenKillHelper.KillAndNull(ref flySequence);
        TweenKillHelper.KillOnGameObject(gameObject);
        ClearSpawnedCoins();
    }

    private void OnDestroy()
    {
        ResetFx();
    }

    private Image CreateCoin(Vector2 anchoredPosition)
    {
        GameObject coinObject = new GameObject("CoinFly", typeof(RectTransform));
        coinObject.transform.SetParent(coinFlyRoot, false);

        RectTransform rect = coinObject.GetComponent<RectTransform>();
        rect.sizeDelta = coinTemplate.rectTransform.sizeDelta;
        rect.anchoredPosition = anchoredPosition;
        rect.localScale = Vector3.one * startScale;

        Image image = coinObject.AddComponent<Image>();
        image.sprite = coinTemplate.sprite;
        image.color = coinTemplate.color;
        image.raycastTarget = false;
        return image;
    }

    private Vector2 WorldToLocalAnchored(RectTransform target)
    {
        if (coinFlyRoot == null || target == null)
        {
            return Vector2.zero;
        }

        Vector3 world = target.TransformPoint(target.rect.center);
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(null, world);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            coinFlyRoot,
            screenPoint,
            null,
            out Vector2 localPoint);
        return localPoint;
    }

    /// <summary>
    /// SpawnPoint는 유지하고, 생성된 CoinFly 오브젝트만 제거합니다.
    /// </summary>
    private void ClearSpawnedCoins()
    {
        if (coinFlyRoot == null)
        {
            return;
        }

        for (int index = coinFlyRoot.childCount - 1; index >= 0; index--)
        {
            Transform child = coinFlyRoot.GetChild(index);
            if (child == null || !child.name.StartsWith("CoinFly"))
            {
                continue;
            }

            child.DOKill();
            Destroy(child.gameObject);
        }
    }
}
