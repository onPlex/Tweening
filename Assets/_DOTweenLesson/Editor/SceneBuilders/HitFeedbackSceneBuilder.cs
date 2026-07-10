using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 07_Game_HitFeedback Scene Builder입니다.
    /// </summary>
    public sealed class HitFeedbackSceneBuilder : LessonSceneBuilderBase
    {
        private HitFeedbackSceneController controller;

        protected override MonoBehaviour GetDemoBehaviour()
        {
            return controller;
        }

        protected override void BuildContent()
        {
            GameObject cameraShakeTarget = LessonObjectFactory.CreateEmpty("CameraShakeTarget", MainCamera.transform);
            cameraShakeTarget.transform.localPosition = Vector3.zero;
            CameraShakeTween cameraShake = cameraShakeTarget.AddComponent<CameraShakeTween>();
            cameraShake.Configure(MainCamera.transform, 0.2f, 0.25f);
            LessonObjectFactory.SetObjectReference(cameraShake, "cameraTarget", MainCamera.transform);

            Canvas canvas = LessonObjectFactory.CreateCanvas("Canvas_GameUI", 20);

            // DemoHUD 하단 Play바와 겹치지 않도록 Attack은 중앙 하단에 둡니다.
            Button attackButton = LessonObjectFactory.CreateButton(
                canvas.transform,
                "Button_Attack",
                "Attack",
                new Vector2(0f, -180f),
                new Vector2(240f, 70f),
                new Color(0.85f, 0.25f, 0.3f));

            GameObject hpBarRoot = new GameObject("Enemy_HPBar", typeof(RectTransform));
            hpBarRoot.transform.SetParent(canvas.transform, false);
            RectTransform hpBarRect = hpBarRoot.GetComponent<RectTransform>();
            // DemoHUD 설명 텍스트(상단 -120) 아래에 HP 바를 배치합니다.
            LessonObjectFactory.SetAnchors(
                hpBarRect,
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0f, -210f),
                new Vector2(420f, 40f));

            Image hpBg = LessonObjectFactory.CreateImage(
                hpBarRoot.transform,
                "Image_HPBackground",
                Vector2.zero,
                new Vector2(420f, 36f),
                new Color(0.15f, 0.15f, 0.18f));

            Image hpFill = LessonObjectFactory.CreateImage(
                hpBarRoot.transform,
                "Image_HPFill",
                Vector2.zero,
                new Vector2(412f, 28f),
                new Color(0.85f, 0.2f, 0.25f));
            hpFill.type = Image.Type.Filled;
            hpFill.fillMethod = Image.FillMethod.Horizontal;
            hpFill.fillOrigin = (int)Image.OriginHorizontal.Left;
            hpFill.fillAmount = 1f;

            TMP_Text hpText = LessonObjectFactory.CreateTmpText(
                hpBarRoot.transform,
                "Text_HP",
                "100 / 100",
                Vector2.zero,
                new Vector2(400f, 36f),
                22,
                TextAlignmentOptions.Center,
                Color.white);

            GameObject floatingRoot = new GameObject("FloatingTextRoot", typeof(RectTransform));
            floatingRoot.transform.SetParent(canvas.transform, false);
            RectTransform floatingRect = floatingRoot.GetComponent<RectTransform>();
            // 적 위치(우측) 근처, HUD와 겹치지 않는 중앙 영역
            floatingRect.anchoredPosition = new Vector2(220f, 40f);
            floatingRect.sizeDelta = new Vector2(300f, 200f);

            GameObject player = LessonObjectFactory.CreatePrimitive(
                "Player",
                PrimitiveType.Capsule,
                new Vector3(-2.5f, 0.9f, 0f),
                new Vector3(0.8f, 0.9f, 0.8f),
                new Color(0.3f, 0.7f, 1f),
                StageRoot.transform);

            SpriteRenderer enemyRenderer = LessonObjectFactory.CreateSpriteProxy(
                "Enemy",
                new Vector3(2.2f, 1f, 0f),
                new Vector3(1.6f, 2.2f, 1f),
                new Color(0.9f, 0.35f, 0.35f),
                StageRoot.transform);

            Transform hitPoint = LessonObjectFactory.CreateEmpty(
                "HitPoint",
                enemyRenderer.transform,
                enemyRenderer.transform.position + Vector3.up * 1.2f).transform;

            // 문서 정렬: AttackEffectRoot
            GameObject attackEffectRoot = LessonObjectFactory.CreateEmpty(
                "AttackEffectRoot",
                StageRoot.transform,
                new Vector3(0.2f, 1.1f, 0f));

            SpriteRenderer effectRenderer = LessonObjectFactory.CreateSpriteProxy(
                "HitFlash",
                attackEffectRoot.transform.position,
                Vector3.one * 0.9f,
                new Color(1f, 0.85f, 0.35f, 0.9f),
                attackEffectRoot.transform);

            AttackEffectTween attackEffect = attackEffectRoot.AddComponent<AttackEffectTween>();
            attackEffect.Configure(effectRenderer.transform, effectRenderer);
            LessonObjectFactory.SetObjectReference(attackEffect, "effectTransform", effectRenderer.transform);
            LessonObjectFactory.SetObjectReference(attackEffect, "effectRenderer", effectRenderer);

            HitFeedbackController hitFeedback = enemyRenderer.gameObject.AddComponent<HitFeedbackController>();
            hitFeedback.Configure(enemyRenderer.transform, enemyRenderer);
            LessonObjectFactory.SetObjectReference(hitFeedback, "enemyTransform", enemyRenderer.transform);
            LessonObjectFactory.SetObjectReference(hitFeedback, "enemyRenderer", enemyRenderer);

            GameObject hpObject = LessonObjectFactory.CreateEmpty("HPBarTween", StageRoot.transform);
            HPBarTween hpBar = hpObject.AddComponent<HPBarTween>();
            hpBar.Configure(hpFill, hpText, 100f);
            LessonObjectFactory.SetObjectReference(hpBar, "hpFill", hpFill);
            LessonObjectFactory.SetObjectReference(hpBar, "hpText", hpText);

            FloatingDamageText floatingPrefab = LessonPrefabFactory.LoadFloatingDamagePrefab();

            GameObject controllerObject = LessonObjectFactory.CreateEmpty(
                "HitFeedbackSceneController",
                StageRoot.transform);
            controller = controllerObject.AddComponent<HitFeedbackSceneController>();
            controller.Configure(
                hitFeedback,
                attackEffect,
                hpBar,
                cameraShake,
                floatingRoot.transform,
                floatingPrefab,
                hitPoint);

            LessonObjectFactory.SetObjectReference(controller, "hitFeedback", hitFeedback);
            LessonObjectFactory.SetObjectReference(controller, "attackEffect", attackEffect);
            LessonObjectFactory.SetObjectReference(controller, "hpBar", hpBar);
            LessonObjectFactory.SetObjectReference(controller, "cameraShake", cameraShake);
            LessonObjectFactory.SetObjectReference(controller, "floatingTextRoot", floatingRoot.transform);
            LessonObjectFactory.SetObjectReference(controller, "floatingDamagePrefab", floatingPrefab);
            LessonObjectFactory.SetObjectReference(controller, "hitPoint", hitPoint);

            LessonObjectFactory.BindButtonMethod(
                attackButton,
                controller,
                nameof(HitFeedbackSceneController.OnAttackClicked));

            _ = player;
            _ = hpBg;
        }
    }
}
