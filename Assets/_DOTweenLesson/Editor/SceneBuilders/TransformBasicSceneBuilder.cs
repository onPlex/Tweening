using UnityEngine;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 03_DOTween_TransformBasic Scene Builder입니다.
    /// </summary>
    public sealed class TransformBasicSceneBuilder : LessonSceneBuilderBase
    {
        private TransformBasicSceneController controller;

        protected override MonoBehaviour GetDemoBehaviour()
        {
            return controller;
        }

        protected override void BuildContent()
        {
            // Station_Move
            GameObject stationMove = LessonObjectFactory.CreateEmpty("Station_Move", StageRoot.transform, new Vector3(-3.5f, 0f, 0f));
            Transform moveStart = LessonObjectFactory.CreateEmpty("StartPoint", stationMove.transform, new Vector3(-4.5f, 0.5f, 0f)).transform;
            Transform moveEnd = LessonObjectFactory.CreateEmpty("EndPoint", stationMove.transform, new Vector3(-2.5f, 0.5f, 0f)).transform;
            GameObject cubeMove = LessonObjectFactory.CreatePrimitive(
                "Cube_Move",
                PrimitiveType.Cube,
                moveStart.position,
                Vector3.one * 0.7f,
                new Color(0.3f, 0.7f, 1f),
                stationMove.transform);

            // Station_Scale
            GameObject stationScale = LessonObjectFactory.CreateEmpty("Station_Scale", StageRoot.transform, new Vector3(-1f, 0f, 0f));
            GameObject cubeScale = LessonObjectFactory.CreatePrimitive(
                "Cube_Scale",
                PrimitiveType.Cube,
                new Vector3(-1f, 0.5f, 0f),
                Vector3.one * 0.7f,
                new Color(0.4f, 1f, 0.5f),
                stationScale.transform);

            // Station_Rotate
            GameObject stationRotate = LessonObjectFactory.CreateEmpty("Station_Rotate", StageRoot.transform, new Vector3(1.5f, 0f, 0f));
            GameObject coinRotate = LessonObjectFactory.CreatePrimitive(
                "Coin_Rotate",
                PrimitiveType.Cylinder,
                new Vector3(1.5f, 0.5f, 0f),
                new Vector3(0.7f, 0.08f, 0.7f),
                new Color(1f, 0.85f, 0.2f),
                stationRotate.transform);
            coinRotate.transform.eulerAngles = new Vector3(90f, 0f, 0f);

            // Station_Fade
            GameObject stationFade = LessonObjectFactory.CreateEmpty("Station_Fade", StageRoot.transform, new Vector3(4f, 0f, 0f));
            SpriteRenderer fadeSprite = LessonObjectFactory.CreateSpriteProxy(
                "Sprite_Fade",
                new Vector3(4f, 0.5f, 0f),
                Vector3.one * 1.2f,
                new Color(1f, 0.45f, 0.75f, 1f),
                stationFade.transform);

            GameObject controllerObject = LessonObjectFactory.CreateEmpty("TransformBasicSceneController", StageRoot.transform);
            controller = controllerObject.AddComponent<TransformBasicSceneController>();
            controller.Configure(
                cubeMove.transform,
                moveStart,
                moveEnd,
                cubeScale.transform,
                coinRotate.transform,
                fadeSprite);

            LessonObjectFactory.SetObjectReference(controller, "moveTarget", cubeMove.transform);
            LessonObjectFactory.SetObjectReference(controller, "moveStart", moveStart);
            LessonObjectFactory.SetObjectReference(controller, "moveEnd", moveEnd);
            LessonObjectFactory.SetObjectReference(controller, "scaleTarget", cubeScale.transform);
            LessonObjectFactory.SetObjectReference(controller, "rotateTarget", coinRotate.transform);
            LessonObjectFactory.SetObjectReference(controller, "fadeTarget", fadeSprite);
        }
    }
}
