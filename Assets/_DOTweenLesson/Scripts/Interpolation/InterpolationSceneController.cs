using UnityEngine;

/// <summary>
/// 01_Interpolation_Lerp Scene의 Play/Reset을 중계합니다.
/// </summary>
public class InterpolationSceneController : MonoBehaviour, IPlayableDemo, IResettableDemo
{
    [SerializeField] private DurationLerpMover durationLerpMover;
    [SerializeField] private DampingLerpMover dampingLerpMover;

    public void Configure(DurationLerpMover durationMover, DampingLerpMover dampingMover)
    {
        durationLerpMover = durationMover;
        dampingLerpMover = dampingMover;
    }

    public void Play()
    {
        durationLerpMover?.Play();
        dampingLerpMover?.Play();
    }

    public void ResetDemo()
    {
        durationLerpMover?.ResetMover();
        dampingLerpMover?.ResetMover();
    }
}
