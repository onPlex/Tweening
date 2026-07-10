using DG.Tweening;
using UnityEngine;

/// <summary>
/// 보물상자 뚜껑 열기 Tween입니다.
/// </summary>
public class ChestOpenTween : MonoBehaviour
{
    [SerializeField] private Transform chestLid;
    [SerializeField] private Vector3 openEulerAngles = new Vector3(0f, 0f, -70f);
    [SerializeField] private float openDuration = 0.35f;

    private Vector3 closedEulerAngles;

    private void Awake()
    {
        if (chestLid != null)
        {
            closedEulerAngles = chestLid.localEulerAngles;
        }
    }

    public void Configure(Transform lid)
    {
        chestLid = lid;
        if (chestLid != null)
        {
            closedEulerAngles = chestLid.localEulerAngles;
        }
    }

    /// <summary>
    /// 뚜껑을 열고 해당 Tween을 반환합니다. Sequence에 Append할 때 사용합니다.
    /// </summary>
    public Tween Open()
    {
        if (chestLid == null)
        {
            return null;
        }

        chestLid.DOKill();
        return chestLid.DOLocalRotate(openEulerAngles, openDuration).SetEase(Ease.OutBack);
    }

    public void ResetChest()
    {
        if (chestLid == null)
        {
            return;
        }

        chestLid.DOKill();
        chestLid.localEulerAngles = closedEulerAngles;
    }
}
