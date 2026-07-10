using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 04_UI_ButtonFeedback Scene Builder입니다.
    /// </summary>
    public sealed class ButtonFeedbackSceneBuilder : LessonSceneBuilderBase
    {
        private ButtonFeedbackSceneController controller;

        protected override MonoBehaviour GetDemoBehaviour()
        {
            return controller;
        }

        protected override void BuildContent()
        {
            Canvas canvas = LessonObjectFactory.CreateCanvas("Canvas_ButtonFeedback", 10);

            // DemoHUD(상단 타이틀/하단 Play바)와 겹치지 않도록 중앙 영역에 배치합니다.
            Image panel = LessonObjectFactory.CreateImage(
                canvas.transform,
                "Panel_Background",
                new Vector2(0f, 20f),
                new Vector2(900f, 360f),
                new Color(0.1f, 0.12f, 0.18f, 0.85f));

            TMP_Text logText = LessonObjectFactory.CreateTmpText(
                canvas.transform,
                "Text_FeedbackLog",
                "버튼을 클릭해 피드백을 확인하세요.",
                new Vector2(0f, -120f),
                new Vector2(800f, 50f),
                26,
                TextAlignmentOptions.Center,
                Color.white);

            Button normalButton = LessonObjectFactory.CreateButton(
                canvas.transform,
                "Button_Normal",
                "Normal",
                new Vector2(-260f, 40f),
                new Vector2(220f, 90f),
                new Color(0.25f, 0.55f, 0.9f));

            Button rewardButton = LessonObjectFactory.CreateButton(
                canvas.transform,
                "Button_Reward",
                "Reward",
                new Vector2(0f, 40f),
                new Vector2(220f, 90f),
                new Color(0.9f, 0.7f, 0.15f));

            // Reward 전용 glow 아이콘 (문서의 Image_Icon) — 버튼 우측 상단에 배치해 텍스트와 겹치지 않게 합니다.
            Image rewardGlow = LessonObjectFactory.CreateImage(
                rewardButton.transform,
                "Image_Icon",
                new Vector2(70f, 28f),
                new Vector2(36f, 36f),
                new Color(1f, 0.95f, 0.55f, 0.9f));
            rewardGlow.raycastTarget = false;
            rewardGlow.transform.SetAsFirstSibling();

            Button disabledButton = LessonObjectFactory.CreateButton(
                canvas.transform,
                "Button_DisabledDemo",
                "Disabled",
                new Vector2(260f, 40f),
                new Vector2(220f, 90f),
                new Color(0.45f, 0.45f, 0.5f));

            _ = panel;

            TMP_Text rewardLabel = rewardButton.GetComponentInChildren<TMP_Text>(true);

            UIButtonTweenFeedback normalFeedback = normalButton.gameObject.AddComponent<UIButtonTweenFeedback>();
            normalFeedback.Configure(
                normalButton.GetComponent<RectTransform>(),
                normalButton.GetComponent<Image>());

            UIButtonTweenFeedback rewardFeedback = rewardButton.gameObject.AddComponent<UIButtonTweenFeedback>();
            rewardFeedback.Configure(
                rewardButton.GetComponent<RectTransform>(),
                rewardButton.GetComponent<Image>(),
                rewardGlow,
                rewardLabel);

            UIButtonTweenFeedback disabledFeedback = disabledButton.gameObject.AddComponent<UIButtonTweenFeedback>();
            disabledFeedback.Configure(
                disabledButton.GetComponent<RectTransform>(),
                disabledButton.GetComponent<Image>());

            GameObject controllerObject = LessonObjectFactory.CreateEmpty(
                "ButtonFeedbackSceneController",
                StageRoot.transform);
            controller = controllerObject.AddComponent<ButtonFeedbackSceneController>();
            controller.Configure(logText, normalFeedback, rewardFeedback, disabledFeedback);

            LessonObjectFactory.SetObjectReference(controller, "feedbackLogText", logText);
            LessonObjectFactory.SetObjectReference(controller, "normalButtonFeedback", normalFeedback);
            LessonObjectFactory.SetObjectReference(controller, "rewardButtonFeedback", rewardFeedback);
            LessonObjectFactory.SetObjectReference(controller, "disabledButtonFeedback", disabledFeedback);

            LessonObjectFactory.SetObjectReference(rewardFeedback, "glowImage", rewardGlow);
            LessonObjectFactory.SetObjectReference(rewardFeedback, "labelText", rewardLabel);

            LessonObjectFactory.BindButtonMethod(normalButton, controller, nameof(ButtonFeedbackSceneController.OnNormalClicked));
            LessonObjectFactory.BindButtonMethod(rewardButton, controller, nameof(ButtonFeedbackSceneController.OnRewardClicked));
            LessonObjectFactory.BindButtonMethod(disabledButton, controller, nameof(ButtonFeedbackSceneController.OnDisabledClicked));
        }
    }
}
