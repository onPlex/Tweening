using DG.Tweening;
using UnityEngine;

/// <summary>
/// 07_Game_HitFeedback Scene 컨트롤러입니다.
/// Attack 시 Hit / Knockback / AttackEffect / HP / Floating Damage / Camera Shake를 실행합니다.
/// </summary>
public class HitFeedbackSceneController : MonoBehaviour, IPlayableDemo, IResettableDemo
{
    [SerializeField] private HitFeedbackController hitFeedback;
    [SerializeField] private AttackEffectTween attackEffect;
    [SerializeField] private HPBarTween hpBar;
    [SerializeField] private CameraShakeTween cameraShake;
    [SerializeField] private Transform floatingTextRoot;
    [SerializeField] private FloatingDamageText floatingDamagePrefab;
    [SerializeField] private Transform hitPoint;
    [SerializeField] private float damageAmount = 15f;

    public void Configure(
        HitFeedbackController hit,
        AttackEffectTween effect,
        HPBarTween bar,
        CameraShakeTween shake,
        Transform floatingRoot,
        FloatingDamageText prefab,
        Transform hitPointTransform)
    {
        hitFeedback = hit;
        attackEffect = effect;
        hpBar = bar;
        cameraShake = shake;
        floatingTextRoot = floatingRoot;
        floatingDamagePrefab = prefab;
        hitPoint = hitPointTransform;
    }

    public void Play()
    {
        OnAttackClicked();
    }

    public void ResetDemo()
    {
        hitFeedback?.ResetHit();
        attackEffect?.ResetEffect();
        hpBar?.ResetHp();
        cameraShake?.ResetShake();

        if (floatingTextRoot != null)
        {
            for (int index = floatingTextRoot.childCount - 1; index >= 0; index--)
            {
                Transform child = floatingTextRoot.GetChild(index);
                if (child == null)
                {
                    continue;
                }

                // Destroy 전에 Tween을 먼저 Kill해 null target 경고를 방지합니다.
                child.DOKill();
                DOTween.Kill(child.gameObject);
                Destroy(child.gameObject);
            }
        }
    }

    public void OnAttackClicked()
    {
        int damage = Mathf.RoundToInt(damageAmount);
        hitFeedback?.PlayHit();
        attackEffect?.Play();
        hpBar?.TakeDamage(damageAmount);
        cameraShake?.PlayShake();
        SpawnFloatingDamage(damage);
    }

    private void SpawnFloatingDamage(int damage)
    {
        if (floatingDamagePrefab == null || floatingTextRoot == null)
        {
            return;
        }

        FloatingDamageText instance = Instantiate(floatingDamagePrefab, floatingTextRoot);
        RectTransform rect = instance.GetComponent<RectTransform>();
        if (rect != null && hitPoint != null)
        {
            rect.anchoredPosition = new Vector2(Random.Range(-40f, 40f), Random.Range(20f, 60f));
        }

        instance.Play(damage);
    }
}
