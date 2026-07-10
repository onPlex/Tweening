using DG.Tweening;
using UnityEngine;

/// <summary>
/// 클릭 시 목표(플레이어 또는 UI)로 이동하며 Scale Down 후 콜백을 호출합니다.
/// </summary>
public class CollectibleItemTween : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private ItemCollectSceneController sceneController;
    [SerializeField] private float popScale = 1.3f;
    [SerializeField] private float popDuration = 0.12f;
    [SerializeField] private float moveDuration = 0.45f;
    [SerializeField] private int coinValue = 1;

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private bool isCollected;
    private Sequence collectSequence;

    private void Awake()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    public void Configure(Transform collectTarget, int value, ItemCollectSceneController controller)
    {
        target = collectTarget;
        coinValue = value;
        sceneController = controller;
        originalScale = transform.localScale;
        originalPosition = transform.position;
        isCollected = false;
    }

    private void OnMouseDown()
    {
        Collect();
    }

    public void Collect()
    {
        if (isCollected || target == null)
        {
            return;
        }

        isCollected = true;
        TweenKillHelper.KillAndNull(ref collectSequence);
        transform.DOKill();

        collectSequence = DOTween.Sequence().SetTarget(gameObject);
        collectSequence.Append(transform.DOScale(originalScale * popScale, popDuration).SetEase(Ease.OutBack));
        collectSequence.Append(transform.DOMove(target.position, moveDuration).SetEase(Ease.InQuad));
        collectSequence.Join(transform.DOScale(Vector3.zero, moveDuration));
        collectSequence.OnComplete(() =>
        {
            sceneController?.OnCoinCollected(coinValue);
            gameObject.SetActive(false);
        });
    }

    public void ResetItem()
    {
        TweenKillHelper.KillAndNull(ref collectSequence);
        TweenKillHelper.KillOnGameObject(gameObject);
        transform.DOKill();
        isCollected = false;
        transform.position = originalPosition;
        transform.localScale = originalScale;
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        TweenKillHelper.KillAndNull(ref collectSequence);
        TweenKillHelper.KillOnGameObject(gameObject);
    }
}
