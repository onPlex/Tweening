using System.Text;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 기존 Scene / Prefab의 TextMeshPro 텍스트에 수업 폰트를 일괄 적용합니다.
    /// </summary>
    public static class LessonFontApplier
    {
        private const string FontGuid = "fc7dadeb1496a0945831302388161bfe";
        private const long FontFileId = 11400000;
        private const long MaterialFileId = -6361300573907671309;

        [MenuItem("Tools/DOTween Lesson/Apply Lesson Font To All")]
        public static void ApplyFromMenu()
        {
            if (!EditorUtility.DisplayDialog(
                    "Apply Lesson Font",
                    "모든 수업 Scene과 Prefab의 TMP Font를 Cloudsofa_namgim-Regular로 변경합니다.",
                    "Apply",
                    "Cancel"))
            {
                return;
            }

            string result = ApplyToAll();
            EditorUtility.DisplayDialog("Apply Lesson Font", result, "OK");
        }

        /// <summary>
        /// 배치: -executeMethod DOTweenLesson.Editor.LessonFontApplier.ApplyToAllAndQuit
        /// </summary>
        public static void ApplyToAllAndQuit()
        {
            try
            {
                string result = ApplyToAll();
                Debug.Log($"[LessonFontApplier] {result}");
                EditorApplication.Exit(0);
            }
            catch (System.Exception exception)
            {
                Debug.LogException(exception);
                EditorApplication.Exit(1);
            }
        }

        public static string ApplyToAll()
        {
            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                LessonSceneRegistry.LessonFontAssetPath);
            if (fontAsset == null)
            {
                throw new System.InvalidOperationException(
                    $"Font asset missing: {LessonSceneRegistry.LessonFontAssetPath}");
            }

            StringBuilder log = new StringBuilder();
            int prefabTextCount = ApplyToPrefabs(fontAsset, log);
            int sceneTextCount = ApplyToScenes(fontAsset, log);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            string summary =
                $"Font applied: Cloudsofa_namgim-Regular\n" +
                $"Prefab TMP texts: {prefabTextCount}\n" +
                $"Scene TMP texts: {sceneTextCount}\n" +
                log;
            Debug.Log($"[LessonFontApplier]\n{summary}");
            return summary;
        }

        private static int ApplyToPrefabs(TMP_FontAsset fontAsset, StringBuilder log)
        {
            string[] prefabPaths =
            {
                LessonSceneRegistry.DemoHudPrefabPath,
                LessonSceneRegistry.FloatingDamagePrefabPath,
            };

            int total = 0;
            for (int index = 0; index < prefabPaths.Length; index++)
            {
                string path = prefabPaths[index];
                GameObject root = PrefabUtility.LoadPrefabContents(path);
                try
                {
                    int count = ApplyFontInHierarchy(root, fontAsset);
                    PrefabUtility.SaveAsPrefabAsset(root, path);
                    total += count;
                    log.AppendLine($"Prefab {path}: {count}");
                }
                finally
                {
                    PrefabUtility.UnloadPrefabContents(root);
                }
            }

            return total;
        }

        private static int ApplyToScenes(TMP_FontAsset fontAsset, StringBuilder log)
        {
            string previousScenePath = SceneManager.GetActiveScene().path;
            int total = 0;

            LessonSceneInfo[] scenes = LessonSceneRegistry.AllScenes;
            for (int index = 0; index < scenes.Length; index++)
            {
                string scenePath = LessonSceneRegistry.GetScenePath(scenes[index].SceneName);
                Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                int count = 0;

                GameObject[] roots = scene.GetRootGameObjects();
                for (int rootIndex = 0; rootIndex < roots.Length; rootIndex++)
                {
                    count += ApplyFontInHierarchy(roots[rootIndex], fontAsset);
                }

                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);
                total += count;
                log.AppendLine($"Scene {scenes[index].SceneName}: {count}");
            }

            if (!string.IsNullOrEmpty(previousScenePath) && System.IO.File.Exists(previousScenePath))
            {
                EditorSceneManager.OpenScene(previousScenePath, OpenSceneMode.Single);
            }

            return total;
        }

        private static int ApplyFontInHierarchy(GameObject root, TMP_FontAsset fontAsset)
        {
            TMP_Text[] texts = root.GetComponentsInChildren<TMP_Text>(true);
            for (int index = 0; index < texts.Length; index++)
            {
                TMP_Text text = texts[index];
                text.font = fontAsset;
                if (fontAsset.material != null)
                {
                    text.fontSharedMaterial = fontAsset.material;
                }

                EditorUtility.SetDirty(text);
            }

            return texts.Length;
        }

        /// <summary>
        /// Unity가 닫혀 있을 때 YAML을 직접 패치하는 보조 API입니다.
        /// </summary>
        public static void PatchYamlReferencesInFolder(string folder)
        {
            string fontRef = $"{{fileID: {FontFileId}, guid: {FontGuid}, type: 2}}";
            string materialRef = $"{{fileID: {MaterialFileId}, guid: {FontGuid}, type: 2}}";
            string[] guids = AssetDatabase.FindAssets("t:Scene t:Prefab", new[] { folder });
            for (int index = 0; index < guids.Length; index++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[index]);
                string text = System.IO.File.ReadAllText(path);
                string updated = text
                    .Replace("m_fontAsset: {fileID: 0}", $"m_fontAsset: {fontRef}")
                    .Replace("m_sharedMaterial: {fileID: 0}", $"m_sharedMaterial: {materialRef}");

                if (updated != text)
                {
                    System.IO.File.WriteAllText(path, updated);
                }
            }

            AssetDatabase.Refresh();
        }
    }
}
