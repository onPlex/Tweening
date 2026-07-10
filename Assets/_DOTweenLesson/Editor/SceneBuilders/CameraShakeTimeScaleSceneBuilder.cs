using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 11_CameraShake_TimeScale Scene Builder입니다.
    /// </summary>
    public sealed class CameraShakeTimeScaleSceneBuilder : LessonSceneBuilderBase
    {
        private CameraShakeTimeScaleController controller;

        protected override MonoBehaviour GetDemoBehaviour()
        {
            return controller;
        }

        protected override void BuildContent()
        {
            CameraShakeTween cameraShake = MainCamera.gameObject.AddComponent<CameraShakeTween>();
            cameraShake.Configure(MainCamera.transform, 0.25f, 0.3f);
            LessonObjectFactory.SetObjectReference(cameraShake, "cameraTarget", MainCamera.transform);

            GameObject movingDemo = LessonObjectFactory.CreatePrimitive(
                "MovingDemo",
                PrimitiveType.Cube,
                new Vector3(0f, 0.5f, 0f),
                Vector3.one * 0.8f,
                new Color(0.4f, 0.8f, 1f),
                StageRoot.transform);

            Canvas canvas = LessonObjectFactory.CreateCanvas("Canvas_TimeScale", 40);

            // DemoHUD 하단 Play바와 겹치지 않도록 조작 버튼을 중앙 하단에 배치합니다.
            Button shakeButton = LessonObjectFactory.CreateButton(
                canvas.transform,
                "Button_Shake",
                "Shake",
                new Vector2(-280f, -160f),
                new Vector2(200f, 70f),
                new Color(0.8f, 0.35f, 0.3f));

            Button pauseButton = LessonObjectFactory.CreateButton(
                canvas.transform,
                "Button_Pause",
                "Pause Toggle",
                new Vector2(0f, -160f),
                new Vector2(220f, 70f),
                new Color(0.7f, 0.55f, 0.2f));

            Button showPopupButton = LessonObjectFactory.CreateButton(
                canvas.transform,
                "Button_ShowPausePopup",
                "Pause Popup",
                new Vector2(280f, -160f),
                new Vector2(220f, 70f),
                new Color(0.35f, 0.45f, 0.8f));

            LessonObjectFactory.CreateTmpText(
                canvas.transform,
                "Text_Hint",
                "Pause 중에도 Popup은 SetUpdate(true)로 움직입니다.",
                new Vector2(0f, 180f),
                new Vector2(1100f, 50f),
                24,
                TextAlignmentOptions.Center,
                Color.white);

            // Pause Popup
            GameObject popupRoot = new GameObject("PausePopup", typeof(RectTransform));
            popupRoot.transform.SetParent(canvas.transform, false);
            RectTransform popupRect = popupRoot.GetComponent<RectTransform>();
            popupRect.sizeDelta = new Vector2(460f, 280f);
            popupRect.anchoredPosition = Vector2.zero;

            Image popupBg = popupRoot.AddComponent<Image>();
            popupBg.color = new Color(0.12f, 0.14f, 0.22f, 0.95f);
            popupBg.sprite = LessonObjectFactory.GetWhiteSprite();
            CanvasGroup popupGroup = popupRoot.AddComponent<CanvasGroup>();

            LessonObjectFactory.CreateTmpText(
                popupRoot.transform,
                "Text_Title",
                "Paused",
                new Vector2(0f, 60f),
                new Vector2(400f, 60f),
                42,
                TextAlignmentOptions.Center,
                Color.white);

            LessonObjectFactory.CreateTmpText(
                popupRoot.transform,
                "Text_Desc",
                "Time.timeScale = 0 이지만 UI Tween은 동작합니다.",
                new Vector2(0f, 0f),
                new Vector2(400f, 70f),
                22,
                TextAlignmentOptions.Center,
                new Color(0.85f, 0.88f, 0.95f));

            Button resumeButton = LessonObjectFactory.CreateButton(
                popupRoot.transform,
                "Button_Resume",
                "Resume",
                new Vector2(0f, -80f),
                new Vector2(180f, 55f),
                new Color(0.25f, 0.7f, 0.45f));

            PausePopupTween pausePopup = popupRoot.AddComponent<PausePopupTween>();
            pausePopup.Configure(popupGroup, popupRect);
            LessonObjectFactory.SetObjectReference(pausePopup, "popupGroup", popupGroup);
            LessonObjectFactory.SetObjectReference(pausePopup, "popupWindow", popupRect);

            GameObject controllerObject = LessonObjectFactory.CreateEmpty("CameraShakeTimeScaleController", StageRoot.transform);
            controller = controllerObject.AddComponent<CameraShakeTimeScaleController>();
            controller.Configure(cameraShake, pausePopup, movingDemo.transform);

            LessonObjectFactory.SetObjectReference(controller, "cameraShake", cameraShake);
            LessonObjectFactory.SetObjectReference(controller, "pausePopup", pausePopup);
            LessonObjectFactory.SetObjectReference(controller, "movingDemoTarget", movingDemo.transform);

            LessonObjectFactory.BindButtonMethod(shakeButton, controller, nameof(CameraShakeTimeScaleController.OnShakeClicked));
            LessonObjectFactory.BindButtonMethod(pauseButton, controller, nameof(CameraShakeTimeScaleController.OnPauseToggleClicked));
            LessonObjectFactory.BindButtonMethod(showPopupButton, controller, nameof(CameraShakeTimeScaleController.OnShowPausePopupClicked));
            LessonObjectFactory.BindButtonMethod(resumeButton, controller, nameof(CameraShakeTimeScaleController.OnHidePausePopupClicked));
        }
    }
}
