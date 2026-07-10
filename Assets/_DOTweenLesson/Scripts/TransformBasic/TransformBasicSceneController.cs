using DG.Tweening;
using UnityEngine;

/// <summary>
/// DOMove / DOScale / DORotate / DOFade 기본 Transform Tween 예제입니다.
/// </summary>
public class TransformBasicSceneController : MonoBehaviour, IPlayableDemo, IResettableDemo
{
    [Header("Move")]
    [SerializeField] private Transform moveTarget;
    [SerializeField] private Transform moveStart;
    [SerializeField] private Transform moveEnd;

    [Header("Scale")]
    [SerializeField] private Transform scaleTarget;

    [Header("Rotate")]
    [SerializeField] private Transform rotateTarget;

    [Header("Fade")]
    [SerializeField] private SpriteRenderer fadeTarget;

    public void Configure(
        Transform move,
        Transform start,
        Transform end,
        Transform scale,
        Transform rotate,
        SpriteRenderer fade)
    {
        moveTarget = move;
        moveStart = start;
        moveEnd = end;
        scaleTarget = scale;
        rotateTarget = rotate;
        fadeTarget = fade;
    }

    public void Play()
    {
        ResetDemo();

        if (moveTarget != null && moveEnd != null)
        {
            moveTarget.DOMove(moveEnd.position, 1f).SetEase(Ease.InOutQuad);
        }

        if (scaleTarget != null)
        {
            scaleTarget.DOScale(Vector3.one * 1.4f, 0.4f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutBack);
        }

        if (rotateTarget != null)
        {
            rotateTarget.DORotate(new Vector3(0f, 0f, 360f), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear);
        }

        if (fadeTarget != null)
        {
            fadeTarget.DOFade(0.2f, 0.5f).SetLoops(2, LoopType.Yoyo);
        }
    }

    public void ResetDemo()
    {
        if (moveTarget != null)
        {
            moveTarget.DOKill();
            if (moveStart != null)
            {
                moveTarget.position = moveStart.position;
            }
        }

        if (scaleTarget != null)
        {
            scaleTarget.DOKill();
            scaleTarget.localScale = Vector3.one;
        }

        if (rotateTarget != null)
        {
            rotateTarget.DOKill();
            rotateTarget.rotation = Quaternion.identity;
        }

        if (fadeTarget != null)
        {
            fadeTarget.DOKill();
            Color fadeColor = fadeTarget.color;
            fadeColor.a = 1f;
            fadeTarget.color = fadeColor;
        }
    }
}
