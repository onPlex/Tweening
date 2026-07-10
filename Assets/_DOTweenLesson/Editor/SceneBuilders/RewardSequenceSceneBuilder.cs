using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 09_Game_RewardSequence Scene Builder입니다.
    /// </summary>
    public sealed class RewardSequenceSceneBuilder : LessonSceneBuilderBase
    {
        private RewardSequenceController controller;

        protected override MonoBehaviour GetDemoBehaviour()
        {
            return controller;
        }

        protected override void BuildContent()
        {
            Canvas canvas = LessonObjectFactory.CreateCanvas("Canvas_GameUI", 25);

            // DemoHUD 타이틀과 겹치지 않도록 좌측 상단에 재화 UI를 둡니다.
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

            // CoinFlyFXRoot + SpawnPoint_Coins
            GameObject coinFlyRootObject = new GameObject("CoinFlyFXRoot", typeof(RectTransform));
            coinFlyRootObject.transform.SetParent(canvas.transform, false);
            RectTransform coinFlyRoot = coinFlyRootObject.GetComponent<RectTransform>();
            LessonObjectFactory.SetAnchors(
                coinFlyRoot,
                Vector2.zero,
                Vector2.one,
                new Vector2(0.5f, 0.5f),
                Vector2.zero,
                Vector2.zero);

            GameObject spawnRoot = new GameObject("SpawnPoint_Coins", typeof(RectTransform));
            spawnRoot.transform.SetParent(coinFlyRoot, false);
            RectTransform spawnRootRect = spawnRoot.GetComponent<RectTransform>();
            spawnRootRect.anchoredPosition = Vector2.zero;

            // 팝업 하단 근처에서 코인이 출발하도록 배치 (팝업/HUD와 겹침 최소화)
            RectTransform[] spawnPoints = new RectTransform[3];
            Vector2[] spawnOffsets =
            {
                new Vector2(-120f, -40f),
                new Vector2(0f, -10f),
                new Vector2(120f, -40f),
            };

            for (int index = 0; index < spawnPoints.Length; index++)
            {
                GameObject spawnObject = new GameObject($"SpawnPoint_0{index + 1}", typeof(RectTransform));
                spawnObject.transform.SetParent(spawnRoot.transform, false);
                RectTransform spawnRect = spawnObject.GetComponent<RectTransform>();
                spawnRect.anchoredPosition = spawnOffsets[index];
                spawnRect.sizeDelta = new Vector2(40f, 40f);
                spawnPoints[index] = spawnRect;
            }

            // RewardTargetPoint: 최종 도착 UI 표시용 (좌측 상단 코인 아이콘 근처)
            GameObject rewardTarget = new GameObject("RewardTargetPoint", typeof(RectTransform));
            rewardTarget.transform.SetParent(coinFlyRoot, false);
            RectTransform rewardTargetRect = rewardTarget.GetComponent<RectTransform>();
            LessonObjectFactory.SetAnchors(
                rewardTargetRect,
                new Vector2(0f, 1f),
                new Vector2(0f, 1f),
                new Vector2(0f, 1f),
                new Vector2(48f, -236f),
                new Vector2(40f, 40f));

            // Panel_RewardPopup — DemoHUD 상/하단과 겹치지 않게 약간 아래로
            GameObject popupRoot = new GameObject("Panel_RewardPopup", typeof(RectTransform));
            popupRoot.transform.SetParent(canvas.transform, false);
            RectTransform popupRect = popupRoot.GetComponent<RectTransform>();
            popupRect.sizeDelta = new Vector2(460f, 320f);
            popupRect.anchoredPosition = new Vector2(0f, 20f);

            Image popupBg = popupRoot.AddComponent<Image>();
            popupBg.color = new Color(0.16f, 0.18f, 0.26f, 1f);
            popupBg.sprite = LessonObjectFactory.GetWhiteSprite();
            CanvasGroup popupGroup = popupRoot.AddComponent<CanvasGroup>();

            Image rewardIconImage = LessonObjectFactory.CreateImage(
                popupRoot.transform,
                "Image_RewardIcon",
                new Vector2(0f, 80f),
                new Vector2(90f, 90f),
                new Color(1f, 0.85f, 0.2f));

            LessonObjectFactory.CreateTmpText(
                popupRoot.transform,
                "Text_RewardTitle",
                "Reward!",
                new Vector2(0f, 0f),
                new Vector2(400f, 50f),
                36,
                TextAlignmentOptions.Center,
                Color.white);

            TMP_Text rewardAmountText = LessonObjectFactory.CreateTmpText(
                popupRoot.transform,
                "Text_RewardAmount",
                "+100",
                new Vector2(0f, -55f),
                new Vector2(400f, 50f),
                42,
                TextAlignmentOptions.Center,
                new Color(1f, 0.9f, 0.3f));

            Button claimButton = LessonObjectFactory.CreateButton(
                popupRoot.transform,
                "Button_Claim",
                "Claim",
                new Vector2(0f, -130f),
                new Vector2(180f, 55f),
                new Color(0.25f, 0.7f, 0.45f));

            // Treasure Chest
            GameObject chestRoot = LessonObjectFactory.CreateEmpty(
                "TreasureChest",
                StageRoot.transform,
                new Vector3(0f, 0f, 0f));
            GameObject chestBase = LessonObjectFactory.CreatePrimitive(
                "Chest_Base",
                PrimitiveType.Cube,
                new Vector3(0f, 0.35f, 0f),
                new Vector3(1.4f, 0.7f, 0.9f),
                new Color(0.65f, 0.4f, 0.18f),
                chestRoot.transform);

            GameObject chestLid = LessonObjectFactory.CreatePrimitive(
                "Chest_Lid",
                PrimitiveType.Cube,
                new Vector3(0f, 0.75f, -0.15f),
                new Vector3(1.4f, 0.15f, 0.9f),
                new Color(0.75f, 0.5f, 0.22f),
                chestRoot.transform);

            ChestOpenTween chestOpen = chestRoot.AddComponent<ChestOpenTween>();
            chestOpen.Configure(chestLid.transform);
            LessonObjectFactory.SetObjectReference(chestOpen, "chestLid", chestLid.transform);

            RewardPopupTween rewardPopup = popupRoot.AddComponent<RewardPopupTween>();
            rewardPopup.Configure(popupGroup, popupRect);
            LessonObjectFactory.SetObjectReference(rewardPopup, "popupGroup", popupGroup);
            LessonObjectFactory.SetObjectReference(rewardPopup, "popupWindow", popupRect);

            GameObject counterObject = LessonObjectFactory.CreateEmpty("CurrencyCounterUI", StageRoot.transform);
            CurrencyCounterUI counter = counterObject.AddComponent<CurrencyCounterUI>();
            counter.Configure(coinAmount, coinIcon.rectTransform);
            LessonObjectFactory.SetObjectReference(counter, "amountText", coinAmount);
            LessonObjectFactory.SetObjectReference(counter, "iconTarget", coinIcon.rectTransform);

            GameObject coinFlyObject = LessonObjectFactory.CreateEmpty("CoinFlyUIFX", StageRoot.transform);
            CoinFlyUIFX coinFlyFx = coinFlyObject.AddComponent<CoinFlyUIFX>();
            coinFlyFx.Configure(coinFlyRoot, coinIcon.rectTransform, coinIcon);
            LessonObjectFactory.SetObjectReference(coinFlyFx, "coinFlyRoot", coinFlyRoot);
            LessonObjectFactory.SetObjectReference(coinFlyFx, "coinIconTarget", coinIcon.rectTransform);
            LessonObjectFactory.SetObjectReference(coinFlyFx, "coinTemplate", coinIcon);

            // P3: GameObject 이름은 문서의 RewardSequenceSceneController
            GameObject controllerObject = LessonObjectFactory.CreateEmpty(
                "RewardSequenceSceneController",
                StageRoot.transform);
            controller = controllerObject.AddComponent<RewardSequenceController>();
            controller.Configure(
                chestOpen,
                rewardPopup,
                counter,
                coinFlyFx,
                rewardAmountText,
                rewardIconImage.rectTransform,
                spawnPoints,
                claimButton);

            LessonObjectFactory.SetObjectReference(controller, "chestOpen", chestOpen);
            LessonObjectFactory.SetObjectReference(controller, "rewardPopup", rewardPopup);
            LessonObjectFactory.SetObjectReference(controller, "currencyCounter", counter);
            LessonObjectFactory.SetObjectReference(controller, "coinFlyFx", coinFlyFx);
            LessonObjectFactory.SetObjectReference(controller, "rewardAmountText", rewardAmountText);
            LessonObjectFactory.SetObjectReference(controller, "rewardIcon", rewardIconImage.rectTransform);
            LessonObjectFactory.SetObjectReferenceArray(controller, "coinSpawnPoints", spawnPoints);
            LessonObjectFactory.SetObjectReference(controller, "claimButton", claimButton);

            LessonObjectFactory.BindButtonMethod(
                claimButton,
                controller,
                nameof(RewardSequenceController.OnClaimClicked));

            _ = chestBase;
            _ = rewardTargetRect;
        }
    }
}
