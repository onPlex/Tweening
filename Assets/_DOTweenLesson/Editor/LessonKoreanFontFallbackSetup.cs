using System.IO;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// Cloudsofa(라틴 전용 TTF)에 한글 Dynamic Fallback Font를 연결합니다.
    /// Cloudsofa에는 Hangul 글리프가 없으므로, 시스템 한글 폰트 Fallback이 필요합니다.
    /// </summary>
    public static class LessonKoreanFontFallbackSetup
    {
        public const string KoreanFallbackFontPath =
            LessonSceneRegistry.RootFolder + "/ArtResource/Font/LessonKoreanFallback SDF.asset";

        private static readonly string[] CandidateSystemFonts =
        {
            @"C:\Windows\Fonts\malgun.ttf",
            @"C:\Windows\Fonts\malgunbd.ttf",
            @"C:\Windows\Fonts\NanumGothic.ttf",
            @"C:\Windows\Fonts\NanumBarunGothic.ttf",
            @"C:\Windows\Fonts\NotoSansKR-Regular.otf",
            @"C:\Windows\Fonts\NotoSansCJKkr-Regular.otf",
        };

        [MenuItem("Tools/DOTween Lesson/Setup Korean Font Fallback")]
        public static void SetupFromMenu()
        {
            string result = Setup();
            EditorUtility.DisplayDialog("Korean Font Fallback", result, "OK");
        }

        /// <summary>
        /// 배치: -executeMethod DOTweenLesson.Editor.LessonKoreanFontFallbackSetup.SetupAndQuit
        /// </summary>
        public static void SetupAndQuit()
        {
            try
            {
                string result = Setup();
                Debug.Log($"[LessonKoreanFontFallback] {result}");
                EditorApplication.Exit(0);
            }
            catch (System.Exception exception)
            {
                Debug.LogException(exception);
                EditorApplication.Exit(1);
            }
        }

        public static string Setup()
        {
            StringBuilder log = new StringBuilder();

            TMP_FontAsset primaryFont = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                LessonSceneRegistry.LessonFontAssetPath);
            if (primaryFont == null)
            {
                throw new System.InvalidOperationException(
                    $"Primary font missing: {LessonSceneRegistry.LessonFontAssetPath}");
            }

            string systemFontPath = FindKoreanSystemFont();
            if (string.IsNullOrEmpty(systemFontPath))
            {
                throw new System.InvalidOperationException(
                    "한글 시스템 폰트(malgun/Nanum/Noto)를 찾을 수 없습니다.");
            }

            log.AppendLine($"System font: {systemFontPath}");

            TMP_FontAsset fallbackFont = EnsureKoreanFallbackFont(systemFontPath, log);
            AttachFallback(primaryFont, fallbackFont);
            TunePrimaryFont(primaryFont);
            PrewarmKoreanCharacters(fallbackFont, log);

            EditorUtility.SetDirty(primaryFont);
            EditorUtility.SetDirty(fallbackFont);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            log.AppendLine($"Primary: {LessonSceneRegistry.LessonFontAssetPath}");
            log.AppendLine($"Fallback: {KoreanFallbackFontPath}");
            log.AppendLine("한글은 Fallback(Dynamic)에서 렌더링됩니다. Cloudsofa 라틴 스타일은 유지됩니다.");
            return log.ToString();
        }

        private static string FindKoreanSystemFont()
        {
            for (int index = 0; index < CandidateSystemFonts.Length; index++)
            {
                if (File.Exists(CandidateSystemFonts[index]))
                {
                    return CandidateSystemFonts[index];
                }
            }

            return null;
        }

        private static TMP_FontAsset EnsureKoreanFallbackFont(string systemFontPath, StringBuilder log)
        {
            TMP_FontAsset existing = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(KoreanFallbackFontPath);
            if (existing != null)
            {
                existing.atlasPopulationMode = AtlasPopulationMode.Dynamic;
                existing.isMultiAtlasTexturesEnabled = true;
                SetClearDynamicDataOnBuild(existing, false);
                EditorUtility.SetDirty(existing);
                log.AppendLine("기존 LessonKoreanFallback SDF를 재사용했습니다.");
                return existing;
            }

            // public API: CreateFontAsset(path, faceIndex, pointSize, padding, renderMode, width, height)
            TMP_FontAsset created = TMP_FontAsset.CreateFontAsset(
                systemFontPath,
                0,
                90,
                9,
                GlyphRenderMode.SDFAA,
                1024,
                1024);

            if (created == null)
            {
                throw new System.InvalidOperationException(
                    $"TMP_FontAsset.CreateFontAsset failed for {systemFontPath}");
            }

            created.name = "LessonKoreanFallback SDF";
            created.atlasPopulationMode = AtlasPopulationMode.Dynamic;
            created.isMultiAtlasTexturesEnabled = true;
            SetClearDynamicDataOnBuild(created, false);

            AssetDatabase.CreateAsset(created, KoreanFallbackFontPath);

            if (created.material != null)
            {
                created.material.name = "LessonKoreanFallback Atlas Material";
                AssetDatabase.AddObjectToAsset(created.material, created);
            }

            if (created.atlasTextures != null)
            {
                for (int index = 0; index < created.atlasTextures.Length; index++)
                {
                    Texture2D atlas = created.atlasTextures[index];
                    if (atlas == null)
                    {
                        continue;
                    }

                    atlas.name = index == 0
                        ? "LessonKoreanFallback Atlas"
                        : $"LessonKoreanFallback Atlas {index}";
                    AssetDatabase.AddObjectToAsset(atlas, created);
                }
            }

            EditorUtility.SetDirty(created);
            log.AppendLine($"Created: {KoreanFallbackFontPath}");
            return created;
        }

        private static void SetClearDynamicDataOnBuild(TMP_FontAsset fontAsset, bool value)
        {
            // clearDynamicDataOnBuild는 internal이므로 SerializedObject로 설정합니다.
            SerializedObject serializedObject = new SerializedObject(fontAsset);
            SerializedProperty property = serializedObject.FindProperty("m_ClearDynamicDataOnBuild");
            if (property == null)
            {
                return;
            }

            property.boolValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void AttachFallback(TMP_FontAsset primaryFont, TMP_FontAsset fallbackFont)
        {
            if (primaryFont.fallbackFontAssetTable == null)
            {
                primaryFont.fallbackFontAssetTable = new System.Collections.Generic.List<TMP_FontAsset>();
            }

            primaryFont.fallbackFontAssetTable.RemoveAll(asset => asset == null || asset == fallbackFont);
            primaryFont.fallbackFontAssetTable.Insert(0, fallbackFont);
        }

        private static void TunePrimaryFont(TMP_FontAsset primaryFont)
        {
            primaryFont.isMultiAtlasTexturesEnabled = true;
            SetClearDynamicDataOnBuild(primaryFont, false);
        }

        private static void PrewarmKoreanCharacters(TMP_FontAsset fallbackFont, StringBuilder log)
        {
            var uniqueCharacters = new System.Collections.Generic.HashSet<char>();
            CollectCharactersFromFolder(LessonSceneRegistry.RootFolder + "/Scripts", uniqueCharacters);
            CollectCharactersFromFolder(LessonSceneRegistry.ScenesFolder, uniqueCharacters);
            CollectCharactersFromFolder(LessonSceneRegistry.PrefabsFolder, uniqueCharacters);

            StringBuilder koreanOnly = new StringBuilder();
            foreach (char character in uniqueCharacters)
            {
                if (character >= 0xAC00 && character <= 0xD7A3)
                {
                    koreanOnly.Append(character);
                }
            }

            if (koreanOnly.Length == 0)
            {
                log.AppendLine("Prewarm: 학습 문구에서 한글을 찾지 못했습니다.");
                return;
            }

            string missing;
            bool added = fallbackFont.TryAddCharacters(koreanOnly.ToString(), out missing);
            log.AppendLine(
                $"Prewarm Hangul chars: requested={koreanOnly.Length}, success={added}, missing={(missing ?? string.Empty).Length}");
        }

        private static void CollectCharactersFromFolder(
            string folder,
            System.Collections.Generic.HashSet<char> uniqueCharacters)
        {
            if (!Directory.Exists(folder))
            {
                return;
            }

            string[] files = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories);
            for (int fileIndex = 0; fileIndex < files.Length; fileIndex++)
            {
                string path = files[fileIndex];
                if (!(path.EndsWith(".cs") || path.EndsWith(".unity") || path.EndsWith(".prefab")))
                {
                    continue;
                }

                string content = File.ReadAllText(path, Encoding.UTF8);
                for (int index = 0; index < content.Length; index++)
                {
                    uniqueCharacters.Add(content[index]);
                }
            }
        }
    }
}
