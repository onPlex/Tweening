using System.Text;
using UnityEditor;
using UnityEngine;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// DOTween 수업 Scene을 절차적으로 생성하는 Editor Window입니다.
    /// Tools > DOTween Lesson > Scene Builder
    /// </summary>
    public sealed class DOTweenLessonBuilderWindow : EditorWindow
    {
        private Vector2 scrollPosition;
        private string statusLog = "Ready.";
        private bool overwriteExisting = true;

        [MenuItem("Tools/DOTween Lesson/Scene Builder")]
        public static void Open()
        {
            DOTweenLessonBuilderWindow window = GetWindow<DOTweenLessonBuilderWindow>();
            window.titleContent = new GUIContent("DOTween Lesson Builder");
            window.minSize = new Vector2(420f, 520f);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField("DOTween Lesson Scene Builder", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "1) Setup Project → 2) Generate Prefabs → 3) Generate All Scenes 순서로 실행하세요.\n" +
                "Scene을 Hierarchy에 수동 배치하지 않고, 이 도구가 절차적으로 생성합니다.",
                MessageType.Info);

            overwriteExisting = EditorGUILayout.ToggleLeft("Overwrite existing scenes", overwriteExisting);

            EditorGUILayout.Space(8f);

            if (GUILayout.Button("1. Setup Project", GUILayout.Height(36f)))
            {
                RunSetupProject();
            }

            if (GUILayout.Button("2. Generate Common Prefabs", GUILayout.Height(36f)))
            {
                RunGeneratePrefabs();
            }

            if (GUILayout.Button("3. Generate All Scenes", GUILayout.Height(40f)))
            {
                RunGenerateAllScenes();
            }

            EditorGUILayout.Space(12f);
            EditorGUILayout.LabelField("Individual Scenes", EditorStyles.boldLabel);

            LessonSceneInfo[] scenes = LessonSceneRegistry.AllScenes;
            for (int index = 0; index < scenes.Length; index++)
            {
                LessonSceneInfo info = scenes[index];
                if (GUILayout.Button($"Generate {info.SceneName}"))
                {
                    RunGenerateSingle(index);
                }
            }

            EditorGUILayout.Space(12f);
            if (GUILayout.Button("Apply Lesson Font To All"))
            {
                AppendLog(LessonFontApplier.ApplyToAll());
            }

            if (GUILayout.Button("Setup Korean Font Fallback"))
            {
                AppendLog(LessonKoreanFontFallbackSetup.Setup());
            }

            if (GUILayout.Button("Register Build Settings"))
            {
                LessonBuildSettingsUtility.RegisterAllLessonScenes();
                AppendLog("Build Settings 등록 완료");
            }

            EditorGUILayout.Space(12f);
            EditorGUILayout.LabelField("Status Log", EditorStyles.boldLabel);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(160f));
            EditorGUILayout.HelpBox(statusLog, MessageType.None);
            EditorGUILayout.EndScrollView();
        }

        private void RunSetupProject()
        {
            try
            {
                LessonPrefabFactory.EnsureFolders();
                string setupResult = LessonProjectSetup.Run();
                AppendLog(setupResult);
            }
            catch (System.Exception exception)
            {
                AppendLog($"Setup 실패: {exception.Message}");
                Debug.LogException(exception);
            }
        }

        private void RunGeneratePrefabs()
        {
            try
            {
                string result = LessonPrefabFactory.GenerateCommonPrefabs();
                AppendLog("Prefab 생성 완료\n" + result);
            }
            catch (System.Exception exception)
            {
                AppendLog($"Prefab 생성 실패: {exception.Message}");
                Debug.LogException(exception);
            }
        }

        private void RunGenerateAllScenes()
        {
            if (overwriteExisting)
            {
                if (!EditorUtility.DisplayDialog(
                        "Generate All Scenes",
                        "기존 수업 Scene을 덮어씁니다. 계속할까요?",
                        "Generate",
                        "Cancel"))
                {
                    return;
                }
            }

            try
            {
                LessonPrefabFactory.EnsureFolders();
                if (LessonPrefabFactory.LoadDemoHudPrefab() == null)
                {
                    LessonPrefabFactory.GenerateCommonPrefabs();
                }

                LessonSceneInfo[] scenes = LessonSceneRegistry.AllScenes;
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("Generate All Scenes");

                for (int index = 0; index < scenes.Length; index++)
                {
                    float progress = index / (float)scenes.Length;
                    EditorUtility.DisplayProgressBar(
                        "DOTween Lesson Builder",
                        $"Building {scenes[index].SceneName}",
                        progress);

                    BuildSceneAtIndex(index, overwriteExisting);
                    builder.AppendLine($"✓ {scenes[index].SceneName}");
                }

                EditorUtility.ClearProgressBar();
                LessonBuildSettingsUtility.RegisterAllLessonScenes();
                builder.AppendLine("Build Settings 등록 완료");
                AppendLog(builder.ToString());
                EditorUtility.DisplayDialog("Done", "12개 Scene 생성 및 Build Settings 등록이 완료되었습니다.", "OK");
            }
            catch (System.Exception exception)
            {
                EditorUtility.ClearProgressBar();
                AppendLog($"Generate All 실패: {exception.Message}");
                Debug.LogException(exception);
            }
        }

        private void RunGenerateSingle(int index)
        {
            try
            {
                LessonPrefabFactory.EnsureFolders();
                if (LessonPrefabFactory.LoadDemoHudPrefab() == null)
                {
                    LessonPrefabFactory.GenerateCommonPrefabs();
                }

                BuildSceneAtIndex(index, overwriteExisting);
                LessonBuildSettingsUtility.RegisterAllLessonScenes();
                AppendLog($"Generated: {LessonSceneRegistry.AllScenes[index].SceneName}");
            }
            catch (System.Exception exception)
            {
                AppendLog($"Generate 실패: {exception.Message}");
                Debug.LogException(exception);
            }
        }

        private static void BuildSceneAtIndex(int index, bool overwrite)
        {
            LessonSceneInfo info = LessonSceneRegistry.AllScenes[index];
            LessonSceneBuilderBase builder = CreateBuilder(index);
            builder.Build(info, overwrite);
        }

        private static LessonSceneBuilderBase CreateBuilder(int index)
        {
            switch (index)
            {
                case 0: return new DemoHubSceneBuilder();
                case 1: return new InterpolationSceneBuilder();
                case 2: return new EaseGraphSceneBuilder();
                case 3: return new TransformBasicSceneBuilder();
                case 4: return new ButtonFeedbackSceneBuilder();
                case 5: return new PopupSceneBuilder();
                case 6: return new ItemCollectSceneBuilder();
                case 7: return new HitFeedbackSceneBuilder();
                case 8: return new QuestMarkerSceneBuilder();
                case 9: return new RewardSequenceSceneBuilder();
                case 10: return new CardHoverSceneBuilder();
                case 11: return new CameraShakeTimeScaleSceneBuilder();
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(index), index, "Unknown scene index");
            }
        }

        private void AppendLog(string message)
        {
            statusLog = $"[{System.DateTime.Now:HH:mm:ss}]\n{message}";
            Repaint();
            Debug.Log($"[DOTweenLessonBuilder] {message}");
        }
    }
}
