using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 배치 모드용 일괄 생성 진입점입니다.
    /// Unity -executeMethod DOTweenLesson.Editor.LessonBatchGenerator.GenerateAll
    /// </summary>
    public static class LessonBatchGenerator
    {
        public static void GenerateAll()
        {
            try
            {
                StringBuilder log = new StringBuilder();
                log.AppendLine("[LessonBatchGenerator] Start");

                string setupResult = LessonProjectSetup.Run();
                log.AppendLine(setupResult);

                string prefabResult = LessonPrefabFactory.GenerateCommonPrefabs();
                log.AppendLine(prefabResult);

                LessonSceneInfo[] scenes = LessonSceneRegistry.AllScenes;
                for (int index = 0; index < scenes.Length; index++)
                {
                    LessonSceneBuilderBase builder = CreateBuilder(index);
                    builder.Build(scenes[index], true);
                    log.AppendLine($"Built: {scenes[index].SceneName}");
                }

                LessonBuildSettingsUtility.RegisterAllLessonScenes();
                log.AppendLine("Build Settings registered");
                log.AppendLine("[LessonBatchGenerator] Done");

                Debug.Log(log.ToString());
                EditorApplication.Exit(0);
            }
            catch (System.Exception exception)
            {
                Debug.LogException(exception);
                EditorApplication.Exit(1);
            }
        }

        /// <summary>
        /// 보강된 Scene만 재생성합니다. (04, 05, 06, 07, 09)
        /// </summary>
        public static void GenerateHardenedScenes()
        {
            try
            {
                StringBuilder log = new StringBuilder();
                log.AppendLine("[LessonBatchGenerator] GenerateHardenedScenes Start");

                if (LessonPrefabFactory.LoadDemoHudPrefab() == null ||
                    LessonPrefabFactory.LoadFloatingDamagePrefab() == null)
                {
                    log.AppendLine(LessonPrefabFactory.GenerateCommonPrefabs());
                }

                // UI 겹침/Tween 안전성 수정이 반영된 Scene들
                int[] indices = { 4, 5, 6, 7, 9, 10, 11 };
                LessonSceneInfo[] scenes = LessonSceneRegistry.AllScenes;

                for (int i = 0; i < indices.Length; i++)
                {
                    int index = indices[i];
                    LessonSceneBuilderBase builder = CreateBuilder(index);
                    builder.Build(scenes[index], true);
                    log.AppendLine($"Built: {scenes[index].SceneName}");
                }

                LessonBuildSettingsUtility.RegisterAllLessonScenes();
                log.AppendLine("[LessonBatchGenerator] GenerateHardenedScenes Done");
                Debug.Log(log.ToString());
                EditorApplication.Exit(0);
            }
            catch (System.Exception exception)
            {
                Debug.LogException(exception);
                EditorApplication.Exit(1);
            }
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
                    throw new System.ArgumentOutOfRangeException(nameof(index));
            }
        }
    }
}
