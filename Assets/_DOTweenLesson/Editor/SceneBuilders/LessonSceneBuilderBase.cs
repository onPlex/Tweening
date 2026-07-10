using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// Scene Builder 공통 베이스입니다.
    /// Camera / Light / EventSystem / DemoHUD / StageRoot를 구성합니다.
    /// </summary>
    public abstract class LessonSceneBuilderBase
    {
        protected LessonSceneInfo SceneInfo { get; private set; }
        protected GameObject StageRoot { get; private set; }
        protected DemoSceneController DemoController { get; private set; }
        protected SceneLoader SceneLoader { get; private set; }
        protected TMP_Text TitleText { get; private set; }
        protected TMP_Text DescriptionText { get; private set; }
        protected Button PlayButton { get; private set; }
        protected Button ResetButton { get; private set; }
        protected Button NextButton { get; private set; }
        protected Camera MainCamera { get; private set; }

        public void Build(LessonSceneInfo info, bool overwrite)
        {
            SceneInfo = info;
            string scenePath = LessonSceneRegistry.GetScenePath(info.SceneName);

            if (!overwrite && FileExists(scenePath))
            {
                Debug.Log($"[LessonBuilder] Skip existing: {scenePath}");
                return;
            }

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            BuildCommon(info);
            BuildContent();
            WireDemoController(info);

            EditorSceneManager.SaveScene(scene, scenePath);
            Debug.Log($"[LessonBuilder] Saved: {scenePath}");
        }

        /// <summary>
        /// Scene별 Stage 콘텐츠를 구현합니다.
        /// </summary>
        protected abstract void BuildContent();

        /// <summary>
        /// DemoSceneController에 연결할 예제 MonoBehaviour를 반환합니다.
        /// Hub처럼 Demo가 없으면 null을 반환해도 됩니다.
        /// </summary>
        protected abstract MonoBehaviour GetDemoBehaviour();

        protected virtual Vector3 GetCameraPosition()
        {
            return new Vector3(0f, 2f, -8f);
        }

        protected virtual Vector3 GetCameraEuler()
        {
            return new Vector3(10f, 0f, 0f);
        }

        protected virtual bool IncludeDemoHud()
        {
            return true;
        }

        private void BuildCommon(LessonSceneInfo info)
        {
            GameObject cameraObject = LessonObjectFactory.CreateMainCamera(GetCameraPosition(), GetCameraEuler());
            MainCamera = cameraObject.GetComponent<Camera>();
            LessonObjectFactory.CreateDirectionalLight();
            LessonObjectFactory.EnsureEventSystem();

            GameObject controllers = LessonObjectFactory.CreateEmpty("DemoControllers");
            SceneLoader = controllers.AddComponent<SceneLoader>();
            DemoController = controllers.AddComponent<DemoSceneController>();
            StageRoot = LessonObjectFactory.CreateEmpty("StageRoot");

            if (IncludeDemoHud())
            {
                CreateOrInstantiateDemoHud(info);
            }
        }

        private void CreateOrInstantiateDemoHud(LessonSceneInfo info)
        {
            GameObject hudPrefab = LessonPrefabFactory.LoadDemoHudPrefab();
            GameObject hudInstance;

            if (hudPrefab != null)
            {
                hudInstance = (GameObject)PrefabUtility.InstantiatePrefab(hudPrefab);
                hudInstance.name = "Canvas_DemoHUD";
            }
            else
            {
                // Prefab이 없으면 즉시 Canvas를 구성합니다.
                Canvas canvas = LessonObjectFactory.CreateCanvas("Canvas_DemoHUD", 100);
                hudInstance = canvas.gameObject;

                LessonObjectFactory.CreateTmpText(
                    canvas.transform,
                    "Text_Title",
                    info.Title,
                    new Vector2(0f, 460f),
                    new Vector2(1400f, 80f),
                    48,
                    TextAlignmentOptions.Center,
                    Color.white);

                LessonObjectFactory.CreateTmpText(
                    canvas.transform,
                    "Text_Description",
                    info.Description,
                    new Vector2(0f, 380f),
                    new Vector2(1500f, 70f),
                    26,
                    TextAlignmentOptions.Center,
                    new Color(0.85f, 0.88f, 0.95f));

                LessonObjectFactory.CreateButton(
                    canvas.transform,
                    "Button_Play",
                    "Play",
                    new Vector2(-220f, -460f),
                    new Vector2(200f, 70f),
                    new Color(0.2f, 0.65f, 0.35f));

                LessonObjectFactory.CreateButton(
                    canvas.transform,
                    "Button_Reset",
                    "Reset",
                    new Vector2(0f, -460f),
                    new Vector2(200f, 70f),
                    new Color(0.65f, 0.45f, 0.15f));

                LessonObjectFactory.CreateButton(
                    canvas.transform,
                    "Button_NextScene",
                    "Next",
                    new Vector2(220f, -460f),
                    new Vector2(200f, 70f),
                    new Color(0.2f, 0.45f, 0.75f));
            }

            TitleText = FindChildComponent<TMP_Text>(hudInstance.transform, "Text_Title");
            DescriptionText = FindChildComponent<TMP_Text>(hudInstance.transform, "Text_Description");
            PlayButton = FindChildComponent<Button>(hudInstance.transform, "Button_Play");
            ResetButton = FindChildComponent<Button>(hudInstance.transform, "Button_Reset");
            NextButton = FindChildComponent<Button>(hudInstance.transform, "Button_NextScene");

            if (TitleText != null)
            {
                TitleText.text = info.Title;
            }

            if (DescriptionText != null)
            {
                DescriptionText.text = info.Description;
            }
        }

        private void WireDemoController(LessonSceneInfo info)
        {
            MonoBehaviour demo = GetDemoBehaviour();

            LessonObjectFactory.SetString(DemoController, "demoTitle", info.Title);
            LessonObjectFactory.SetString(DemoController, "demoDescription", info.Description);
            LessonObjectFactory.SetString(DemoController, "nextSceneName", info.NextSceneName);
            LessonObjectFactory.SetObjectReference(DemoController, "demoBehaviour", demo);
            LessonObjectFactory.SetObjectReference(DemoController, "stageRoot", StageRoot != null ? StageRoot.transform : null);
            LessonObjectFactory.SetObjectReference(DemoController, "sceneLoader", SceneLoader);
            LessonObjectFactory.SetObjectReference(DemoController, "titleText", TitleText);
            LessonObjectFactory.SetObjectReference(DemoController, "descriptionText", DescriptionText);
            LessonObjectFactory.SetObjectReference(DemoController, "playButton", PlayButton);
            LessonObjectFactory.SetObjectReference(DemoController, "resetButton", ResetButton);
            LessonObjectFactory.SetObjectReference(DemoController, "nextSceneButton", NextButton);

            if (PlayButton != null)
            {
                LessonObjectFactory.BindButtonMethod(PlayButton, DemoController, nameof(DemoSceneController.OnPlayClicked));
            }

            if (ResetButton != null)
            {
                LessonObjectFactory.BindButtonMethod(ResetButton, DemoController, nameof(DemoSceneController.OnResetClicked));
            }

            if (NextButton != null)
            {
                LessonObjectFactory.BindButtonMethod(NextButton, DemoController, nameof(DemoSceneController.OnNextSceneClicked));
            }
        }

        protected static T FindChildComponent<T>(Transform root, string childName) where T : Component
        {
            Transform[] children = root.GetComponentsInChildren<Transform>(true);
            for (int index = 0; index < children.Length; index++)
            {
                if (children[index].name == childName)
                {
                    return children[index].GetComponent<T>();
                }
            }

            return null;
        }

        private static bool FileExists(string assetPath)
        {
            return System.IO.File.Exists(assetPath);
        }
    }
}
