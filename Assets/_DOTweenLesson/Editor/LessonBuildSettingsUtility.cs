using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// Build Settings에 수업 Scene을 순서대로 등록합니다.
    /// </summary>
    public static class LessonBuildSettingsUtility
    {
        public static void RegisterAllLessonScenes()
        {
            List<EditorBuildSettingsScene> buildScenes = EditorBuildSettings.scenes.ToList();

            // 기존 수업 Scene 엔트리 제거 후 앞에 재등록
            HashSet<string> lessonPaths = new HashSet<string>();
            for (int index = 0; index < LessonSceneRegistry.AllScenes.Length; index++)
            {
                lessonPaths.Add(LessonSceneRegistry.GetScenePath(LessonSceneRegistry.AllScenes[index].SceneName));
            }

            buildScenes.RemoveAll(scene => lessonPaths.Contains(scene.path));

            List<EditorBuildSettingsScene> lessonScenes = new List<EditorBuildSettingsScene>();
            for (int index = 0; index < LessonSceneRegistry.AllScenes.Length; index++)
            {
                string path = LessonSceneRegistry.GetScenePath(LessonSceneRegistry.AllScenes[index].SceneName);
                if (!System.IO.File.Exists(path))
                {
                    Debug.LogWarning($"[LessonBuildSettings] Scene 파일이 아직 없습니다: {path}");
                    continue;
                }

                lessonScenes.Add(new EditorBuildSettingsScene(path, true));
            }

            // SampleScene 등 기존 Scene은 뒤에 유지하되 비활성화
            for (int index = 0; index < buildScenes.Count; index++)
            {
                if (buildScenes[index].path.Contains("SampleScene"))
                {
                    buildScenes[index] = new EditorBuildSettingsScene(buildScenes[index].path, false);
                }
            }

            List<EditorBuildSettingsScene> finalList = new List<EditorBuildSettingsScene>();
            finalList.AddRange(lessonScenes);
            finalList.AddRange(buildScenes);
            EditorBuildSettings.scenes = finalList.ToArray();

            Debug.Log($"[LessonBuildSettings] 수업 Scene {lessonScenes.Count}개 등록 완료");
        }
    }
}
