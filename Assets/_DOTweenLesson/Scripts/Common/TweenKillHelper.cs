using DG.Tweening;
using UnityEngine;

/// <summary>
/// DOTween Kill 처리를 공통화하여 중복 Tween과 Scene 전환 시 잔존 Tween을 방지합니다.
/// </summary>
public static class TweenKillHelper
{
    /// <summary>
    /// Sequence를 Kill한 뒤 참조를 null로 비웁니다.
    /// </summary>
    public static void KillAndNull(ref Sequence sequence, bool complete = false)
    {
        if (sequence != null && sequence.IsActive())
        {
            sequence.Kill(complete);
        }

        sequence = null;
    }

    /// <summary>
    /// Tween을 Kill한 뒤 참조를 null로 비웁니다.
    /// </summary>
    public static void KillAndNull(ref Tween tween, bool complete = false)
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill(complete);
        }

        tween = null;
    }

    /// <summary>
    /// SetTarget(gameObject)로 묶인 Tween을 일괄 종료합니다.
    /// </summary>
    public static void KillOnGameObject(GameObject target, bool complete = false)
    {
        if (target == null)
        {
            return;
        }

        DOTween.Kill(target, complete);
    }

    /// <summary>
    /// 지정 Transform과 하위 오브젝트의 모든 Tween을 종료합니다.
    /// </summary>
    public static void KillAllInHierarchy(Transform root, bool complete = false)
    {
        if (root == null)
        {
            return;
        }

        root.DOKill(complete);

        Transform[] children = root.GetComponentsInChildren<Transform>(true);
        for (int index = 0; index < children.Length; index++)
        {
            children[index].DOKill(complete);
        }
    }

    /// <summary>
    /// CanvasGroup 계열의 Fade Tween을 종료합니다.
    /// </summary>
    public static void KillCanvasGroups(Transform root, bool complete = false)
    {
        if (root == null)
        {
            return;
        }

        CanvasGroup[] canvasGroups = root.GetComponentsInChildren<CanvasGroup>(true);
        for (int index = 0; index < canvasGroups.Length; index++)
        {
            canvasGroups[index].DOKill(complete);
        }
    }

    /// <summary>
    /// SpriteRenderer 계열의 Color/Fade Tween을 종료합니다.
    /// </summary>
    public static void KillSpriteRenderers(Transform root, bool complete = false)
    {
        if (root == null)
        {
            return;
        }

        SpriteRenderer[] renderers = root.GetComponentsInChildren<SpriteRenderer>(true);
        for (int index = 0; index < renderers.Length; index++)
        {
            renderers[index].DOKill(complete);
        }
    }
}
