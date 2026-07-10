using UnityEngine;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 08_Game_QuestMarker Scene Builder입니다.
    /// </summary>
    public sealed class QuestMarkerSceneBuilder : LessonSceneBuilderBase
    {
        private QuestMarkerSceneController controller;

        protected override MonoBehaviour GetDemoBehaviour()
        {
            return controller;
        }

        protected override void BuildContent()
        {
            LessonObjectFactory.CreatePrimitive(
                "Player",
                PrimitiveType.Capsule,
                new Vector3(-3.5f, 0.9f, 0f),
                new Vector3(0.8f, 0.9f, 0.8f),
                new Color(0.3f, 0.75f, 1f),
                StageRoot.transform);

            // NPC + Quest Marker
            GameObject npc = LessonObjectFactory.CreatePrimitive(
                "NPC_QuestGiver",
                PrimitiveType.Capsule,
                new Vector3(0f, 0.9f, 0f),
                new Vector3(0.8f, 0.9f, 0.8f),
                new Color(0.45f, 0.85f, 0.4f),
                StageRoot.transform);

            GameObject questMarkerRoot = LessonObjectFactory.CreateEmpty(
                "QuestMarker",
                npc.transform,
                npc.transform.position + Vector3.up * 2.2f);

            SpriteRenderer glow = LessonObjectFactory.CreateSpriteProxy(
                "Glow",
                questMarkerRoot.transform.position,
                new Vector3(1.2f, 1.2f, 1f),
                new Color(1f, 0.85f, 0.2f, 0.7f),
                questMarkerRoot.transform);
            glow.sortingOrder = 0;

            GameObject icon = LessonObjectFactory.CreateEmpty(
                "Icon_Exclamation",
                questMarkerRoot.transform,
                questMarkerRoot.transform.position);

            SpriteRenderer iconBar = LessonObjectFactory.CreateSpriteProxy(
                "Icon_Bar",
                icon.transform.position,
                new Vector3(0.14f, 0.5f, 1f),
                new Color(1f, 0.9f, 0.2f),
                icon.transform);
            iconBar.transform.localPosition = new Vector3(0f, 0.12f, 0f);
            iconBar.sortingOrder = 2;

            SpriteRenderer iconDot = LessonObjectFactory.CreateSpriteProxy(
                "Icon_Dot",
                icon.transform.position,
                new Vector3(0.18f, 0.18f, 1f),
                new Color(1f, 0.9f, 0.2f),
                icon.transform);
            iconDot.transform.localPosition = new Vector3(0f, -0.34f, 0f);
            iconDot.sortingOrder = 2;

            QuestMarkerTween questMarker = questMarkerRoot.AddComponent<QuestMarkerTween>();
            questMarker.Configure(icon.transform, glow);
            LessonObjectFactory.SetObjectReference(questMarker, "icon", icon.transform);
            LessonObjectFactory.SetObjectReference(questMarker, "glowRenderer", glow);

            // Chest
            GameObject chest = LessonObjectFactory.CreatePrimitive(
                "Chest_Interactable",
                PrimitiveType.Cube,
                new Vector3(2.5f, 0.4f, 0f),
                new Vector3(1f, 0.7f, 0.7f),
                new Color(0.7f, 0.45f, 0.2f),
                StageRoot.transform);

            GameObject chestMarker = LessonObjectFactory.CreateEmpty(
                "InteractMarker",
                chest.transform,
                chest.transform.position + Vector3.up * 1.2f);
            SpriteRenderer chestIcon = LessonObjectFactory.CreateSpriteProxy(
                "Icon",
                chestMarker.transform.position,
                Vector3.one * 0.5f,
                Color.cyan,
                chestMarker.transform);
            AttentionPulseTween chestPulse = chestMarker.AddComponent<AttentionPulseTween>();
            chestPulse.Configure(chestIcon.transform);
            LessonObjectFactory.SetObjectReference(chestPulse, "target", chestIcon.transform);

            // Portal
            GameObject portal = LessonObjectFactory.CreatePrimitive(
                "Portal",
                PrimitiveType.Cylinder,
                new Vector3(5f, 1.2f, 0f),
                new Vector3(1.2f, 0.1f, 1.2f),
                new Color(0.5f, 0.3f, 1f),
                StageRoot.transform);
            portal.transform.eulerAngles = new Vector3(90f, 0f, 0f);

            GameObject portalMarker = LessonObjectFactory.CreateEmpty(
                "PortalMarker",
                portal.transform,
                portal.transform.position + Vector3.up * 0.5f);
            SpriteRenderer portalIcon = LessonObjectFactory.CreateSpriteProxy(
                "Icon",
                portalMarker.transform.position,
                Vector3.one * 0.55f,
                new Color(0.7f, 0.5f, 1f),
                portalMarker.transform);
            AttentionPulseTween portalPulse = portalMarker.AddComponent<AttentionPulseTween>();
            portalPulse.Configure(portalIcon.transform);
            LessonObjectFactory.SetObjectReference(portalPulse, "target", portalIcon.transform);

            GameObject controllerObject = LessonObjectFactory.CreateEmpty("QuestMarkerSceneController", StageRoot.transform);
            controller = controllerObject.AddComponent<QuestMarkerSceneController>();

            QuestMarkerTween[] markers = { questMarker };
            AttentionPulseTween[] pulses = { chestPulse, portalPulse };
            controller.Configure(markers, pulses);
            LessonObjectFactory.SetObjectReferenceArray(controller, "questMarkers", markers);
            LessonObjectFactory.SetObjectReferenceArray(controller, "attentionPulses", pulses);
        }
    }
}
