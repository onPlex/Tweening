using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 10_CardHover_Select Scene Builder입니다.
    /// </summary>
    public sealed class CardHoverSceneBuilder : LessonSceneBuilderBase
    {
        private CardHoverSceneController controller;

        protected override MonoBehaviour GetDemoBehaviour()
        {
            return controller;
        }

        protected override void BuildContent()
        {
            Canvas canvas = LessonObjectFactory.CreateCanvas("Canvas_Cards", 30);

            // DemoHUD 타이틀/설명과 겹치지 않도록 안내 문구를 중앙으로 내립니다.
            LessonObjectFactory.CreateTmpText(
                canvas.transform,
                "Text_Instruction",
                "카드를 Hover / Click 하여 선택하세요. Play는 첫 카드를 선택합니다.",
                new Vector2(0f, 200f),
                new Vector2(1200f, 50f),
                26,
                TextAlignmentOptions.Center,
                Color.white);

            GameObject cardRoot = new GameObject("CardRoot", typeof(RectTransform));
            cardRoot.transform.SetParent(canvas.transform, false);
            RectTransform cardRootRect = cardRoot.GetComponent<RectTransform>();
            // 하단 Play바와 겹치지 않도록 카드 위치를 약간 위로 올립니다.
            cardRootRect.anchoredPosition = new Vector2(0f, 10f);
            cardRootRect.sizeDelta = new Vector2(1200f, 360f);

            Color[] cardColors =
            {
                new Color(0.35f, 0.45f, 0.85f),
                new Color(0.75f, 0.35f, 0.55f),
                new Color(0.3f, 0.7f, 0.5f),
                new Color(0.85f, 0.65f, 0.2f),
            };

            CardHoverTween[] cards = new CardHoverTween[4];

            GameObject controllerObject = LessonObjectFactory.CreateEmpty("CardHoverSceneController", StageRoot.transform);
            controller = controllerObject.AddComponent<CardHoverSceneController>();

            for (int index = 0; index < 4; index++)
            {
                float x = -360f + index * 240f;
                Image cardImage = LessonObjectFactory.CreateImage(
                    cardRoot.transform,
                    $"Card_0{index + 1}",
                    new Vector2(x, 0f),
                    new Vector2(180f, 280f),
                    cardColors[index]);

                LessonObjectFactory.CreateTmpText(
                    cardImage.transform,
                    "Text",
                    $"Card {index + 1}",
                    Vector2.zero,
                    new Vector2(160f, 60f),
                    28,
                    TextAlignmentOptions.Center,
                    Color.white);

                CardHoverTween cardTween = cardImage.gameObject.AddComponent<CardHoverTween>();
                cardTween.Configure(cardImage.rectTransform, cardImage, controller);
                LessonObjectFactory.SetObjectReference(cardTween, "cardRect", cardImage.rectTransform);
                LessonObjectFactory.SetObjectReference(cardTween, "cardImage", cardImage);
                cards[index] = cardTween;
            }

            controller.Configure(cards);
            LessonObjectFactory.SetObjectReferenceArray(controller, "cards", cards);

            // CardHoverTween의 sceneController 필드 연결
            for (int index = 0; index < cards.Length; index++)
            {
                LessonObjectFactory.SetObjectReference(cards[index], "sceneController", controller);
            }
        }
    }
}
