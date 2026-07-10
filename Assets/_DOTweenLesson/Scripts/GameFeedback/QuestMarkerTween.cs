using DG.Tweening;
using UnityEngine;

/// <summary>
/// 퀘스트 마커의 무한 상하 부유 + Pulse + Glow Fade 루프입니다.
/// </summary>
public class QuestMarkerTween : MonoBehaviour
{
    [SerializeField] private Transform icon;
    [SerializeField] private SpriteRenderer glowRenderer;
    [SerializeField] private float floatHeight = 0.25f;
    [SerializeField] private float floatDuration = 0.6f;
    [SerializeField] private float pulseScale = 1.15f;

    private Vector3 iconStartLocalPosition;
    private Vector3 iconStartScale;
    private Color originalGlowColor;
    private bool hasOriginalGlowColor;

    private void Start()
    {
        // 초기 상태만 캐시하고, 재생은 HUD Play 버튼에서 시작합니다.
        CacheStartState();
    }

    private void OnDestroy()
    {
        // Scene 전환 시 무한 Loop Tween이 Destroy된 Transform을 건드리지 않도록 종료합니다.
        StopLoop();
    }

    public void Configure(Transform iconTransform, SpriteRenderer glow)
    {
        icon = iconTransform;
        glowRenderer = glow;
        CacheStartState();
    }

    private void CacheStartState()
    {
        if (icon == null)
        {
            icon = transform;
        }

        iconStartLocalPosition = icon.localPosition;
        iconStartScale = icon.localScale;

        if (glowRenderer != null)
        {
            originalGlowColor = glowRenderer.color;
            hasOriginalGlowColor = true;
        }

        ConfigureVisuals();
    }

    private void ConfigureVisuals()
    {
        if (glowRenderer != null)
        {
            glowRenderer.sortingOrder = 0;
        }

        SpriteRenderer iconRenderer = icon != null ? icon.GetComponent<SpriteRenderer>() : null;
        if (iconRenderer == null)
        {
            return;
        }

        iconRenderer.sortingOrder = 2;

        // 기존 Scene의 단일 사각 Sprite도 런타임에서 느낌표처럼 보이도록 보정합니다.
        if (icon.Find("Icon_Bar") != null)
        {
            return;
        }

        Sprite sprite = iconRenderer.sprite;
        Material material = iconRenderer.sharedMaterial;
        Color color = iconRenderer.color;
        iconRenderer.enabled = false;

        CreateIconPart("Icon_Bar", new Vector3(0f, 0.12f, 0f), new Vector3(0.22f, 0.62f, 1f), sprite, material, color);
        CreateIconPart("Icon_Dot", new Vector3(0f, -0.34f, 0f), new Vector3(0.24f, 0.18f, 1f), sprite, material, color);
    }

    private void CreateIconPart(
        string partName,
        Vector3 localPosition,
        Vector3 localScale,
        Sprite sprite,
        Material material,
        Color color)
    {
        GameObject part = new GameObject(partName);
        part.transform.SetParent(icon, false);
        part.transform.localPosition = localPosition;
        part.transform.localScale = localScale;

        SpriteRenderer renderer = part.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.sharedMaterial = material;
        renderer.color = color;
        renderer.sortingOrder = 2;
    }

    public void PlayLoop()
    {
        if (icon == null)
        {
            return;
        }

        // Start() 자동 재생 여부와 관계없이 Play 버튼마다 루프를 다시 시작할 수 있도록 합니다.
        StopLoop();

        icon.DOLocalMoveY(iconStartLocalPosition.y + floatHeight, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        icon.DOScale(iconStartScale * pulseScale, floatDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        if (glowRenderer != null)
        {
            glowRenderer.DOFade(0.25f, floatDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
    }

    public void StopLoop()
    {
        if (icon != null)
        {
            icon.DOKill();
            icon.localPosition = iconStartLocalPosition;
            icon.localScale = iconStartScale;
        }

        if (glowRenderer != null)
        {
            glowRenderer.DOKill();
            glowRenderer.color = hasOriginalGlowColor ? originalGlowColor : glowRenderer.color;
        }
    }
}
