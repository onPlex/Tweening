using DG.Tweening;
using UnityEngine;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 02_EaseGraph_Comparison Scene Builder입니다.
    /// </summary>
    public sealed class EaseGraphSceneBuilder : LessonSceneBuilderBase
    {
        private EaseGraphSceneController controller;

        protected override Vector3 GetCameraPosition()
        {
            return new Vector3(0f, 3f, -12f);
        }

        protected override MonoBehaviour GetDemoBehaviour()
        {
            return controller;
        }

        protected override void BuildContent()
        {
            Ease[] easings =
            {
                Ease.Linear,
                Ease.InQuad,
                Ease.OutQuad,
                Ease.InOutQuad,
                Ease.OutBack,
                Ease.OutBounce,
            };

            string[] names =
            {
                "Linear",
                "InQuad",
                "OutQuad",
                "InOutQuad",
                "OutBack",
                "OutBounce",
            };

            Color[] colors =
            {
                Color.white,
                new Color(0.3f, 0.8f, 1f),
                new Color(0.3f, 1f, 0.5f),
                new Color(1f, 0.85f, 0.2f),
                new Color(1f, 0.45f, 0.8f),
                new Color(1f, 0.4f, 0.3f),
            };

            EaseTrackMover[] movers = new EaseTrackMover[easings.Length];
            float trackSpacing = 1.3f;
            float startY = 3.2f;

            for (int index = 0; index < easings.Length; index++)
            {
                float y = startY - index * trackSpacing;
                GameObject trackRoot = LessonObjectFactory.CreateEmpty($"Track_{names[index]}", StageRoot.transform, new Vector3(0f, y, 0f));

                Transform start = LessonObjectFactory.CreateEmpty("StartPoint", trackRoot.transform, new Vector3(-4f, y, 0f)).transform;
                Transform end = LessonObjectFactory.CreateEmpty("EndPoint", trackRoot.transform, new Vector3(4f, y, 0f)).transform;

                LessonObjectFactory.CreatePrimitive(
                    "TrackLine",
                    PrimitiveType.Cube,
                    new Vector3(0f, y - 0.15f, 0f),
                    new Vector3(8f, 0.05f, 0.2f),
                    new Color(0.35f, 0.35f, 0.4f),
                    trackRoot.transform);

                GameObject moverObject = LessonObjectFactory.CreatePrimitive(
                    "Mover",
                    PrimitiveType.Sphere,
                    start.position,
                    Vector3.one * 0.45f,
                    colors[index],
                    trackRoot.transform);

                EaseTrackMover trackMover = trackRoot.AddComponent<EaseTrackMover>();
                trackMover.Configure(moverObject.transform, start, end, easings[index], 2f);
                LessonObjectFactory.SetObjectReference(trackMover, "mover", moverObject.transform);
                LessonObjectFactory.SetObjectReference(trackMover, "startPoint", start);
                LessonObjectFactory.SetObjectReference(trackMover, "endPoint", end);
                LessonObjectFactory.SetEnum(trackMover, "easeType", (int)easings[index]);
                movers[index] = trackMover;

                // Ease 그래프 (트랙 왼쪽)
                GameObject graphObject = LessonObjectFactory.CreateEmpty($"Graph_{names[index]}", StageRoot.transform, new Vector3(-6.5f, y - 0.4f, 0f));
                LineRenderer lineRenderer = graphObject.AddComponent<LineRenderer>();
                lineRenderer.widthMultiplier = 0.03f;
                EaseGraphDrawer drawer = graphObject.AddComponent<EaseGraphDrawer>();
                drawer.Configure(easings[index], colors[index], 1.6f, 0.9f, 40);
            }

            GameObject controllerObject = LessonObjectFactory.CreateEmpty("EaseGraphSceneController", StageRoot.transform);
            controller = controllerObject.AddComponent<EaseGraphSceneController>();
            controller.Configure(movers);
            LessonObjectFactory.SetObjectReferenceArray(controller, "trackMovers", movers);
        }
    }
}
