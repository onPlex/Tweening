using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// DOTween 모듈 설정, Define Symbol, Input System UI 호환을 처리합니다.
    /// </summary>
    public static class LessonProjectSetup
    {
        public static string Run()
        {
            List<string> logs = new List<string>();

            EnsureInputSystemBoth();
            logs.Add("Input System: Both (구 Input Manager + Input System)");

            EnableDoTweenTmpModule();
            logs.Add("DOTweenSettings: textMeshProEnabled = true");

            AddDefineSymbols(new[] { "DOTWEEN_TMP" });
            logs.Add("Scripting Define: DOTWEEN_TMP 추가");

            LessonPrefabFactory.EnsureFolders();
            logs.Add("폴더 구조 생성 완료");

            try
            {
                logs.Add(LessonKoreanFontFallbackSetup.Setup());
            }
            catch (System.Exception exception)
            {
                logs.Add($"Korean Font Fallback Setup 실패: {exception.Message}");
                Debug.LogException(exception);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return string.Join("\n", logs);
        }

        private static void EnsureInputSystemBoth()
        {
            // activeInputHandler: 0=Old, 1=New, 2=Both
            // StandaloneInputModule과 Input System UI를 모두 허용합니다.
            SerializedObject playerSettings = new SerializedObject(
                AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
            SerializedProperty activeInputHandler = playerSettings.FindProperty("activeInputHandler");
            if (activeInputHandler != null && activeInputHandler.intValue != 2)
            {
                activeInputHandler.intValue = 2;
                playerSettings.ApplyModifiedProperties();
                Debug.Log("[LessonProjectSetup] activeInputHandler → Both");
            }
        }

        private static void EnableDoTweenTmpModule()
        {
            Object settings = Resources.Load("DOTweenSettings");
            if (settings == null)
            {
                settings = AssetDatabase.LoadAssetAtPath<Object>("Assets/Resources/DOTweenSettings.asset");
            }

            if (settings == null)
            {
                Debug.LogWarning("[LessonProjectSetup] DOTweenSettings.asset을 찾을 수 없습니다.");
                return;
            }

            SerializedObject serialized = new SerializedObject(settings);
            SerializedProperty modules = serialized.FindProperty("modules");
            if (modules == null)
            {
                return;
            }

            SerializedProperty tmpEnabled = modules.FindPropertyRelative("textMeshProEnabled");
            SerializedProperty uiEnabled = modules.FindPropertyRelative("uiEnabled");
            SerializedProperty spriteEnabled = modules.FindPropertyRelative("spriteEnabled");

            if (tmpEnabled != null)
            {
                tmpEnabled.boolValue = true;
            }

            if (uiEnabled != null)
            {
                uiEnabled.boolValue = true;
            }

            if (spriteEnabled != null)
            {
                spriteEnabled.boolValue = true;
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(settings);
        }

        private static void AddDefineSymbols(string[] symbolsToAdd)
        {
            NamedBuildTarget[] targets =
            {
                NamedBuildTarget.Standalone,
                NamedBuildTarget.Android,
                NamedBuildTarget.iOS,
                NamedBuildTarget.WebGL,
            };

            for (int targetIndex = 0; targetIndex < targets.Length; targetIndex++)
            {
                NamedBuildTarget target = targets[targetIndex];
                string defines;
                try
                {
                    defines = PlayerSettings.GetScriptingDefineSymbols(target);
                }
                catch
                {
                    continue;
                }

                List<string> list = defines
                    .Split(';')
                    .Where(symbol => !string.IsNullOrWhiteSpace(symbol))
                    .ToList();

                bool changed = false;
                for (int index = 0; index < symbolsToAdd.Length; index++)
                {
                    if (!list.Contains(symbolsToAdd[index]))
                    {
                        list.Add(symbolsToAdd[index]);
                        changed = true;
                    }
                }

                if (changed)
                {
                    PlayerSettings.SetScriptingDefineSymbols(target, string.Join(";", list));
                }
            }
        }
    }
}
