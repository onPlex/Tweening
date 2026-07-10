using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 06_Game_ItemCollect Scene Builder입니다.
    /// </summary>
    public sealed class ItemCollectSceneBuilder : LessonSceneBuilderBase
    {
        private ItemCollectSceneController controller;

        protected override MonoBehaviour GetDemoBehaviour()
        {
            return controller;
        }

        protected override void BuildContent()
        {
            Canvas canvas = LessonObjectFactory.CreateCanvas("Canvas_GameUI", 15);

            // DemoHUD 중앙 타이틀과 겹치지 않도록 좌측 상단에 재화 UI를 둡니다.
            Image topBar = LessonObjectFactory.CreateImage(
                canvas.transform,
                "Panel_TopBar",
                Vector2.zero,
                new Vector2(280f, 72f),
                new Color(0.08f, 0.1f, 0.14f, 0.9f));
            LessonObjectFactory.SetAnchors(
                topBar.rectTransform,
                new Vector2(0f, 1f),
                new Vector2(0f, 1f),
                new Vector2(0f, 1f),
                new Vector2(24f, -200f),
                new Vector2(280f, 72f));

            Image coinIcon = LessonObjectFactory.CreateImage(
                topBar.transform,
                "Image_CoinIcon",
                new Vector2(-90f, 0f),
                new Vector2(48f, 48f),
                new Color(1f, 0.85f, 0.2f));

            TMP_Text coinAmount = LessonObjectFactory.CreateTmpText(
                topBar.transform,
                "Text_CoinAmount",
                "0",
                new Vector2(40f, 0f),
                new Vector2(160f, 56f),
                36,
                TextAlignmentOptions.Left,
                Color.white);

            // DemoHUD 설명과 겹치지 않도록 중앙 상단(HUD 아래)에 배치합니다.
            LessonObjectFactory.CreateTmpText(
                canvas.transform,
                "Text_Instruction",
                "Play: 코인 자동 수집 / 또는 코인 클릭",
                new Vector2(0f, 180f),
                new Vector2(900f, 50f),
                24,
                TextAlignmentOptions.Center,
                new Color(0.85f, 0.9f, 1f));

            GameObject player = LessonObjectFactory.CreatePrimitive(
                "Player",
                PrimitiveType.Capsule,
                new Vector3(0f, 0.9f, 0f),
                new Vector3(0.8f, 0.9f, 0.8f),
                new Color(0.3f, 0.75f, 1f),
                StageRoot.transform);

            // 클릭 충돌용 Collider는 Primitive에 기본 포함
            Vector3[] spawnPositions =
            {
                new Vector3(-2.5f, 0.4f, 2f),
                new Vector3(0f, 0.4f, 3f),
                new Vector3(2.5f, 0.4f, 2f),
            };

            CollectibleItemTween[] items = new CollectibleItemTween[spawnPositions.Length];
            GameObject coinsRoot = LessonObjectFactory.CreateEmpty("CoinItems", StageRoot.transform);

            for (int index = 0; index < spawnPositions.Length; index++)
            {
                GameObject coin = LessonObjectFactory.CreatePrimitive(
                    $"CoinItem_0{index + 1}",
                    PrimitiveType.Sphere,
                    spawnPositions[index],
                    Vector3.one * 0.45f,
                    new Color(1f, 0.85f, 0.2f),
                    coinsRoot.transform);

                CollectibleItemTween itemTween = coin.AddComponent<CollectibleItemTween>();
                items[index] = itemTween;
            }

            GameObject counterObject = LessonObjectFactory.CreateEmpty("CurrencyCounterUI", StageRoot.transform);
            CurrencyCounterUI counter = counterObject.AddComponent<CurrencyCounterUI>();
            counter.Configure(coinAmount, coinIcon.rectTransform);
            LessonObjectFactory.SetObjectReference(counter, "amountText", coinAmount);
            LessonObjectFactory.SetObjectReference(counter, "iconTarget", coinIcon.rectTransform);

            GameObject controllerObject = LessonObjectFactory.CreateEmpty("ItemCollectSceneController", StageRoot.transform);
            controller = controllerObject.AddComponent<ItemCollectSceneController>();
            controller.Configure(items, counter);
            LessonObjectFactory.SetObjectReferenceArray(controller, "coinItems", items);
            LessonObjectFactory.SetObjectReference(controller, "currencyCounter", counter);

            for (int index = 0; index < items.Length; index++)
            {
                items[index].Configure(player.transform, 1, controller);
                LessonObjectFactory.SetObjectReference(items[index], "target", player.transform);
                LessonObjectFactory.SetObjectReference(items[index], "sceneController", controller);
            }
        }
    }
}
