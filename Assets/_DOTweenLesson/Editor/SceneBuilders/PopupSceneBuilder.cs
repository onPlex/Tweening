using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 05_UI_Popup Scene Builder입니다.
    /// </summary>
    public sealed class PopupSceneBuilder : LessonSceneBuilderBase
    {
        private PopupTweenController controller;

        protected override MonoBehaviour GetDemoBehaviour()
        {
            return controller;
        }

        protected override void BuildContent()
        {
            Canvas canvas = LessonObjectFactory.CreateCanvas("Canvas_Popup", 20);

            // DemoHUD와 겹치지 않도록 Open 버튼을 중앙에 배치합니다.
            Button openButton = LessonObjectFactory.CreateButton(
                canvas.transform,
                "Button_OpenPopup",
                "Open Popup",
                new Vector2(0f, 40f),
                new Vector2(280f, 80f),
                new Color(0.2f, 0.55f, 0.85f));

            Image dimImage = LessonObjectFactory.CreateImage(
                canvas.transform,
                "DimBackground",
                Vector2.zero,
                new Vector2(2400f, 1400f),
                new Color(0f, 0f, 0f, 0.6f));
            CanvasGroup dimGroup = dimImage.gameObject.AddComponent<CanvasGroup>();
            LessonObjectFactory.SetAnchors(
                dimImage.rectTransform,
                Vector2.zero,
                Vector2.one,
                new Vector2(0.5f, 0.5f),
                Vector2.zero,
                Vector2.zero);

            GameObject popupRoot = new GameObject("Popup_Window", typeof(RectTransform));
            popupRoot.transform.SetParent(canvas.transform, false);
            RectTransform popupRect = popupRoot.GetComponent<RectTransform>();
            popupRect.sizeDelta = new Vector2(520f, 360f);
            popupRect.anchoredPosition = Vector2.zero;

            Image popupBg = popupRoot.AddComponent<Image>();
            popupBg.color = new Color(0.18f, 0.2f, 0.28f, 1f);
            popupBg.sprite = LessonObjectFactory.GetWhiteSprite();
            CanvasGroup popupGroup = popupRoot.AddComponent<CanvasGroup>();

            LessonObjectFactory.CreateTmpText(
                popupRoot.transform,
                "Text_Title",
                "Quest Complete!",
                new Vector2(0f, 110f),
                new Vector2(440f, 60f),
                36,
                TextAlignmentOptions.Center,
                Color.white);

            LessonObjectFactory.CreateTmpText(
                popupRoot.transform,
                "Text_Description",
                "팝업 Sequence (Fade + Scale OutBack)",
                new Vector2(0f, 30f),
                new Vector2(440f, 80f),
                24,
                TextAlignmentOptions.Center,
                new Color(0.85f, 0.88f, 0.95f));

            Button confirmButton = LessonObjectFactory.CreateButton(
                popupRoot.transform,
                "Button_Confirm",
                "Confirm",
                new Vector2(-110f, -110f),
                new Vector2(180f, 60f),
                new Color(0.25f, 0.7f, 0.4f));

            Button closeButton = LessonObjectFactory.CreateButton(
                popupRoot.transform,
                "Button_Close",
                "Close",
                new Vector2(110f, -110f),
                new Vector2(180f, 60f),
                new Color(0.7f, 0.3f, 0.35f));

            // P3: Hierarchy 이름은 문서의 PopupSceneController (클래스는 PopupTweenController 유지)
            GameObject controllerObject = LessonObjectFactory.CreateEmpty("PopupSceneController", StageRoot.transform);
            controller = controllerObject.AddComponent<PopupTweenController>();
            controller.Configure(dimGroup, popupGroup, popupRect);

            LessonObjectFactory.SetObjectReference(controller, "dimGroup", dimGroup);
            LessonObjectFactory.SetObjectReference(controller, "popupGroup", popupGroup);
            LessonObjectFactory.SetObjectReference(controller, "popupWindow", popupRect);

            LessonObjectFactory.BindButtonMethod(openButton, controller, nameof(PopupTweenController.Open));
            LessonObjectFactory.BindButtonMethod(confirmButton, controller, nameof(PopupTweenController.Close));
            LessonObjectFactory.BindButtonMethod(closeButton, controller, nameof(PopupTweenController.Close));
        }
    }
}
