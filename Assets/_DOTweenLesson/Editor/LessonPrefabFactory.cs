using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// DemoHUD / FloatingDamageText 공통 Prefab을 생성합니다.
    /// </summary>
    public static class LessonPrefabFactory
    {
        public static void EnsureFolders()
        {
            CreateFolderRecursive(LessonSceneRegistry.RootFolder);
            CreateFolderRecursive(LessonSceneRegistry.ScenesFolder);
            CreateFolderRecursive(LessonSceneRegistry.PrefabsFolder);
            CreateFolderRecursive(LessonSceneRegistry.PrefabsFolder + "/Common");
            CreateFolderRecursive(LessonSceneRegistry.PrefabsFolder + "/UI");
            CreateFolderRecursive(LessonSceneRegistry.PrefabsFolder + "/Effects");
            CreateFolderRecursive(LessonSceneRegistry.MaterialsFolder);
            CreateFolderRecursive(LessonSceneRegistry.RootFolder + "/Scripts");
            CreateFolderRecursive(LessonSceneRegistry.RootFolder + "/Editor");
            CreateFolderRecursive(LessonSceneRegistry.RootFolder + "/Editor/SceneBuilders");
            AssetDatabase.Refresh();
        }

        public static string GenerateCommonPrefabs()
        {
            EnsureFolders();
            string hudPath = CreateDemoHudPrefab();
            string floatingPath = CreateFloatingDamagePrefab();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return $"DemoHUD: {hudPath}\nFloatingDamage: {floatingPath}";
        }

        public static GameObject LoadDemoHudPrefab()
        {
            return AssetDatabase.LoadAssetAtPath<GameObject>(LessonSceneRegistry.DemoHudPrefabPath);
        }

        public static FloatingDamageText LoadFloatingDamagePrefab()
        {
            return AssetDatabase.LoadAssetAtPath<FloatingDamageText>(LessonSceneRegistry.FloatingDamagePrefabPath);
        }

        private static string CreateDemoHudPrefab()
        {
            Canvas canvas = LessonObjectFactory.CreateCanvas("Canvas_DemoHUD", 100);

            // 상단 타이틀 / 설명
            TMP_Text title = LessonObjectFactory.CreateTmpText(
                canvas.transform,
                "Text_Title",
                "Demo Title",
                new Vector2(0f, 460f),
                new Vector2(1400f, 80f),
                48,
                TextAlignmentOptions.Center,
                Color.white);

            LessonObjectFactory.SetAnchors(
                title.rectTransform,
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0f, -40f),
                new Vector2(1400f, 80f));

            TMP_Text description = LessonObjectFactory.CreateTmpText(
                canvas.transform,
                "Text_Description",
                "Description",
                new Vector2(0f, 380f),
                new Vector2(1500f, 70f),
                26,
                TextAlignmentOptions.Center,
                new Color(0.85f, 0.88f, 0.95f));

            LessonObjectFactory.SetAnchors(
                description.rectTransform,
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f),
                new Vector2(0f, -120f),
                new Vector2(1500f, 70f));

            // 하단 버튼 바
            Button playButton = LessonObjectFactory.CreateButton(
                canvas.transform,
                "Button_Play",
                "Play",
                new Vector2(-220f, -460f),
                new Vector2(200f, 70f),
                new Color(0.2f, 0.65f, 0.35f));

            Button resetButton = LessonObjectFactory.CreateButton(
                canvas.transform,
                "Button_Reset",
                "Reset",
                new Vector2(0f, -460f),
                new Vector2(200f, 70f),
                new Color(0.65f, 0.45f, 0.15f));

            Button nextButton = LessonObjectFactory.CreateButton(
                canvas.transform,
                "Button_NextScene",
                "Next",
                new Vector2(220f, -460f),
                new Vector2(200f, 70f),
                new Color(0.2f, 0.45f, 0.75f));

            LessonObjectFactory.SetAnchors(
                playButton.GetComponent<RectTransform>(),
                new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f),
                new Vector2(-220f, 40f),
                new Vector2(200f, 70f));

            LessonObjectFactory.SetAnchors(
                resetButton.GetComponent<RectTransform>(),
                new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f),
                new Vector2(0f, 40f),
                new Vector2(200f, 70f));

            LessonObjectFactory.SetAnchors(
                nextButton.GetComponent<RectTransform>(),
                new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f),
                new Vector2(220f, 40f),
                new Vector2(200f, 70f));

            string path = LessonSceneRegistry.DemoHudPrefabPath;
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(canvas.gameObject, path);
            Object.DestroyImmediate(canvas.gameObject);
            return path;
        }

        private static string CreateFloatingDamagePrefab()
        {
            GameObject root = new GameObject("FloatingDamageText", typeof(RectTransform));
            RectTransform rootRect = root.GetComponent<RectTransform>();
            rootRect.sizeDelta = new Vector2(160f, 80f);

            CanvasGroup canvasGroup = root.AddComponent<CanvasGroup>();

            TMP_Text text = LessonObjectFactory.CreateTmpText(
                root.transform,
                "Text_Damage",
                "0",
                Vector2.zero,
                new Vector2(160f, 80f),
                42,
                TextAlignmentOptions.Center,
                new Color(1f, 0.35f, 0.25f));

            FloatingDamageText component = root.AddComponent<FloatingDamageText>();
            LessonObjectFactory.SetObjectReference(component, "damageText", text);
            LessonObjectFactory.SetObjectReference(component, "canvasGroup", canvasGroup);
            LessonObjectFactory.SetObjectReference(component, "rectTransform", rootRect);

            string path = LessonSceneRegistry.FloatingDamagePrefabPath;
            PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);
            return path;
        }

        private static void CreateFolderRecursive(string assetPath)
        {
            if (AssetDatabase.IsValidFolder(assetPath))
            {
                return;
            }

            string[] parts = assetPath.Split('/');
            string current = parts[0];
            for (int index = 1; index < parts.Length; index++)
            {
                string next = current + "/" + parts[index];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[index]);
                }

                current = next;
            }
        }
    }
}
