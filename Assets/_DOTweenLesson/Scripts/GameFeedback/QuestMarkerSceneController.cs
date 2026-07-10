using UnityEngine;

/// <summary>
/// 08_Game_QuestMarker Scene 컨트롤러입니다.
/// </summary>
public class QuestMarkerSceneController : MonoBehaviour, IPlayableDemo, IResettableDemo
{
    [SerializeField] private QuestMarkerTween[] questMarkers;
    [SerializeField] private AttentionPulseTween[] attentionPulses;

    public void Configure(QuestMarkerTween[] markers, AttentionPulseTween[] pulses)
    {
        questMarkers = markers;
        attentionPulses = pulses;
    }

    public void Play()
    {
        if (questMarkers != null)
        {
            for (int index = 0; index < questMarkers.Length; index++)
            {
                questMarkers[index]?.PlayLoop();
            }
        }

        if (attentionPulses != null)
        {
            for (int index = 0; index < attentionPulses.Length; index++)
            {
                attentionPulses[index]?.PlayLoop();
            }
        }
    }

    public void ResetDemo()
    {
        if (questMarkers != null)
        {
            for (int index = 0; index < questMarkers.Length; index++)
            {
                questMarkers[index]?.StopLoop();
            }
        }

        if (attentionPulses != null)
        {
            for (int index = 0; index < attentionPulses.Length; index++)
            {
                attentionPulses[index]?.StopLoop();
            }
        }
    }
}
