using DG.Tweening;
using UnityEngine;

/// <summary>
/// 적 피격 시 Knockback + Shake + Color Flash를 재생합니다.
/// </summary>
public class HitFeedbackController : MonoBehaviour
{
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private SpriteRenderer enemyRenderer;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private Vector3 knockbackOffset = new Vector3(0.45f, 0.1f, 0f);
    [SerializeField] private float knockbackDuration = 0.12f;

    private Vector3 originalPosition;
    private Color originalColor;
    private Sequence hitSequence;

    private void Awake()
    {
        CacheOriginalState();
    }

    public void Configure(Transform enemy, SpriteRenderer renderer)
    {
        enemyTransform = enemy;
        enemyRenderer = renderer;
        CacheOriginalState();
    }

    private void CacheOriginalState()
    {
        if (enemyTransform != null)
        {
            originalPosition = enemyTransform.position;
        }

        if (enemyRenderer != null)
        {
            originalColor = enemyRenderer.color;
        }
    }

    public void PlayHit()
    {
        if (enemyTransform == null)
        {
            return;
        }

        TweenKillHelper.KillAndNull(ref hitSequence);
        enemyTransform.DOKill();
        enemyTransform.position = originalPosition;

        if (enemyRenderer != null)
        {
            enemyRenderer.DOKill();
            enemyRenderer.color = originalColor;
        }

        hitSequence = DOTween.Sequence()
            .SetTarget(gameObject)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        // Append: 짧은 Knockback(플레이어 반대 방향으로 밀림)
        hitSequence.Append(
            enemyTransform.DOMove(originalPosition + knockbackOffset, knockbackDuration)
                .SetEase(Ease.OutQuad));

        // Append: shake
        hitSequence.Append(
            enemyTransform.DOShakePosition(0.18f, new Vector3(0.18f, 0.08f, 0f), 12, 90f));

        // Append: 원위치로 복귀
        hitSequence.Append(
            enemyTransform.DOMove(originalPosition, 0.1f).SetEase(Ease.OutQuad));

        if (enemyRenderer != null)
        {
            // Join: knockback과 동시에 hit flash
            hitSequence.Join(
                enemyRenderer.DOColor(hitColor, 0.06f)
                    .OnComplete(() =>
                    {
                        enemyRenderer.DOColor(originalColor, 0.12f).SetTarget(gameObject);
                    }));
        }
    }

    public void ResetHit()
    {
        TweenKillHelper.KillAndNull(ref hitSequence);
        TweenKillHelper.KillOnGameObject(gameObject);

        if (enemyTransform != null)
        {
            enemyTransform.DOKill();
            enemyTransform.position = originalPosition;
        }

        if (enemyRenderer != null)
        {
            enemyRenderer.DOKill();
            enemyRenderer.color = originalColor;
        }
    }

    private void OnDestroy()
    {
        ResetHit();
    }
}
