using UnityEngine;

/// <summary>
/// 02_EaseGraph_Comparison Scene의 Play/Reset을 중계합니다.
/// </summary>
public class EaseGraphSceneController : MonoBehaviour, IPlayableDemo, IResettableDemo
{
    [SerializeField] private EaseTrackMover[] trackMovers;

    public void Configure(EaseTrackMover[] movers)
    {
        trackMovers = movers;
    }

    public void Play()
    {
        if (trackMovers == null)
        {
            return;
        }

        for (int index = 0; index < trackMovers.Length; index++)
        {
            trackMovers[index]?.Play();
        }
    }

    public void ResetDemo()
    {
        if (trackMovers == null)
        {
            return;
        }

        for (int index = 0; index < trackMovers.Length; index++)
        {
            trackMovers[index]?.ResetMover();
        }
    }
}
