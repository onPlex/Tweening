using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 00_DemoHub — 예제 Scene 선택 메뉴입니다.
    /// </summary>
    public sealed class DemoHubSceneBuilder : LessonSceneBuilderBase
    {
        protected override bool IncludeDemoHud()
        {
            return false;
        }

        protected override MonoBehaviour GetDemoBehaviour()
        {
            return null;
        }

        protected override void BuildContent()
        {
            Canvas canvas = LessonObjectFactory.CreateCanvas("Canvas_Hub", 0);

            LessonObjectFactory.CreateTmpText(
                canvas.transform,
                "Text_Title",
                "DOTween Lesson Hub",
                new Vector2(0f, 420f),
                new Vector2(1200f, 90f),
                56,
                TextAlignmentOptions.Center,
                Color.white);

            LessonObjectFactory.CreateTmpText(
                canvas.transform,
                "Text_Subtitle",
                "예제 Scene을 선택하세요",
                new Vector2(0f, 340f),
                new Vector2(1000f, 50f),
                28,
                TextAlignmentOptions.Center,
                new Color(0.8f, 0.85f, 0.95f));

            // Hub에는 Play/Reset이 필요 없으므로 DemoController HUD 대신 전용 버튼만 사용합니다.
            string[] sceneNames =
            {
                "01_Interpolation_Lerp",
                "02_EaseGraph_Comparison",
                "03_DOTween_TransformBasic",
                "04_UI_ButtonFeedback",
                "05_UI_Popup",
                "06_Game_ItemCollect",
                "07_Game_HitFeedback",
                "08_Game_QuestMarker",
                "09_Game_RewardSequence",
                "10_CardHover_Select",
                "11_CameraShake_TimeScale",
            };

            string[] labels =
            {
                "01 Interpolation",
                "02 Ease Graph",
                "03 Transform Basic",
                "04 Button Feedback",
                "05 UI Popup",
                "06 Item Collect",
                "07 Hit Feedback",
                "08 Quest Marker",
                "09 Reward Sequence",
                "10 Card Hover",
                "11 Camera / TimeScale",
            };

            Color[] colors =
            {
                new Color(0.25f, 0.55f, 0.85f),
                new Color(0.3f, 0.6f, 0.7f),
                new Color(0.35f, 0.5f, 0.75f),
                new Color(0.45f, 0.4f, 0.8f),
                new Color(0.55f, 0.35f, 0.75f),
                new Color(0.2f, 0.65f, 0.5f),
                new Color(0.75f, 0.3f, 0.35f),
                new Color(0.7f, 0.55f, 0.2f),
                new Color(0.85f, 0.65f, 0.15f),
                new Color(0.4f, 0.45f, 0.7f),
                new Color(0.5f, 0.35f, 0.55f),
            };

            const int columns = 2;
            const float startY = 240f;
            const float rowHeight = 75f;

            for (int index = 0; index < sceneNames.Length; index++)
            {
                int column = index % columns;
                int row = index / columns;
                float x = column == 0 ? -280f : 280f;
                float y = startY - row * rowHeight;

                Button button = LessonObjectFactory.CreateButton(
                    canvas.transform,
                    $"Button_{sceneNames[index]}",
                    labels[index],
                    new Vector2(x, y),
                    new Vector2(480f, 60f),
                    colors[index]);

                LessonObjectFactory.BindButtonLoadScene(button, SceneLoader, sceneNames[index]);
            }
        }
    }
}
