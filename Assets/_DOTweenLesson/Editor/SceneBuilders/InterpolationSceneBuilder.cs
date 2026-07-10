using UnityEngine;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 01_Interpolation_Lerp Scene Builder입니다.
    /// </summary>
    public sealed class InterpolationSceneBuilder : LessonSceneBuilderBase
    {
        private InterpolationSceneController controller;

        protected override MonoBehaviour GetDemoBehaviour()
        {
            return controller;
        }

        protected override void BuildContent()
        {
            Transform startPoint = LessonObjectFactory.CreateEmpty("StartPoint", StageRoot.transform, new Vector3(-3f, 0.5f, 0f)).transform;
            Transform endPoint = LessonObjectFactory.CreateEmpty("EndPoint", StageRoot.transform, new Vector3(3f, 0.5f, 0f)).transform;

            LessonObjectFactory.CreatePrimitive("Marker_Start", PrimitiveType.Sphere, startPoint.position, Vector3.one * 0.3f, Color.green, StageRoot.transform);
            LessonObjectFactory.CreatePrimitive("Marker_End", PrimitiveType.Sphere, endPoint.position, Vector3.one * 0.3f, Color.red, StageRoot.transform);

            // 경로 표시용 Quad
            GameObject pathLine = LessonObjectFactory.CreatePrimitive(
                "Line_Path",
                PrimitiveType.Cube,
                new Vector3(0f, 0.05f, 0f),
                new Vector3(6f, 0.05f, 0.15f),
                new Color(0.4f, 0.4f, 0.45f),
                StageRoot.transform);

            GameObject durationMoverObject = LessonObjectFactory.CreatePrimitive(
                "Mover_DurationLerp",
                PrimitiveType.Cube,
                startPoint.position + Vector3.up * 0.4f,
                Vector3.one * 0.6f,
                new Color(0.2f, 0.7f, 1f),
                StageRoot.transform);
            durationMoverObject.transform.position = startPoint.position;

            GameObject dampingMoverObject = LessonObjectFactory.CreatePrimitive(
                "Mover_DampingLerp",
                PrimitiveType.Sphere,
                startPoint.position + Vector3.forward * 1.2f,
                Vector3.one * 0.6f,
                new Color(1f, 0.75f, 0.2f),
                StageRoot.transform);
            dampingMoverObject.transform.position = startPoint.position + Vector3.forward * 1.2f;

            // EndPoint를 damping용으로도 공유하되 Z 오프셋 유지
            Transform dampingEnd = LessonObjectFactory.CreateEmpty(
                "EndPoint_Damping",
                StageRoot.transform,
                endPoint.position + Vector3.forward * 1.2f).transform;

            DurationLerpMover durationMover = durationMoverObject.AddComponent<DurationLerpMover>();
            durationMover.Configure(startPoint, endPoint, 2f);

            DampingLerpMover dampingMover = dampingMoverObject.AddComponent<DampingLerpMover>();
            dampingMover.Configure(startPoint, dampingEnd, 2f);

            GameObject controllerObject = LessonObjectFactory.CreateEmpty("InterpolationSceneController", StageRoot.transform);
            controller = controllerObject.AddComponent<InterpolationSceneController>();
            controller.Configure(durationMover, dampingMover);

            LessonObjectFactory.SetObjectReference(durationMover, "startPoint", startPoint);
            LessonObjectFactory.SetObjectReference(durationMover, "endPoint", endPoint);
            LessonObjectFactory.SetObjectReference(dampingMover, "startPoint", startPoint);
            LessonObjectFactory.SetObjectReference(dampingMover, "endPoint", dampingEnd);
            LessonObjectFactory.SetObjectReference(controller, "durationLerpMover", durationMover);
            LessonObjectFactory.SetObjectReference(controller, "dampingLerpMover", dampingMover);

            _ = pathLine;
        }
    }
}
