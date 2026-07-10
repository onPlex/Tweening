using TMPro;
using UnityEngine;

/// <summary>
/// 04_UI_ButtonFeedback Scene 컨트롤러입니다.
/// Normal / Reward / Disabled 버튼의 피드백 차이를 보여줍니다.
/// </summary>
public class ButtonFeedbackSceneController : MonoBehaviour, IPlayableDemo, IResettableDemo
{
    [SerializeField] private TMP_Text feedbackLogText;
    [SerializeField] private UIButtonTweenFeedback normalButtonFeedback;
    [SerializeField] private UIButtonTweenFeedback rewardButtonFeedback;
    [SerializeField] private UIButtonTweenFeedback disabledButtonFeedback;

    public void Configure(
        TMP_Text logText,
        UIButtonTweenFeedback normalFeedback,
        UIButtonTweenFeedback rewardFeedback,
        UIButtonTweenFeedback disabledFeedback)
    {
        feedbackLogText = logText;
        normalButtonFeedback = normalFeedback;
        rewardButtonFeedback = rewardFeedback;
        disabledButtonFeedback = disabledFeedback;
    }

    public void Play()
    {
        AppendLog("Play: Normal / Reward 클릭 피드백 미리보기");
        normalButtonFeedback?.PlayClick();
        rewardButtonFeedback?.PlayRewardClick();
    }

    public void ResetDemo()
    {
        normalButtonFeedback?.ResetFeedback();
        rewardButtonFeedback?.ResetFeedback();
        disabledButtonFeedback?.ResetFeedback();

        if (feedbackLogText != null)
        {
            feedbackLogText.text = "버튼을 클릭해 피드백을 확인하세요.";
        }
    }

    public void OnNormalClicked()
    {
        normalButtonFeedback?.PlayClick();
        AppendLog("Normal 버튼 클릭 — Scale Down/Up");
    }

    public void OnRewardClicked()
    {
        rewardButtonFeedback?.PlayRewardClick();
        AppendLog("Reward 버튼 클릭 — Scale + Glow + Text Punch");
    }

    public void OnDisabledClicked()
    {
        disabledButtonFeedback?.PlayInvalid();
        AppendLog("Disabled 버튼 — Invalid Shake");
    }

    private void AppendLog(string message)
    {
        if (feedbackLogText != null)
        {
            feedbackLogText.text = message;
        }
    }
}
