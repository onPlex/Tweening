using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DOTweenLesson.Editor
{
    /// <summary>
    /// Scene 구성용 Camera / Light / UI / Primitive 생성 헬퍼입니다.
    /// </summary>
    public static class LessonObjectFactory
    {
        private static Material cachedLitMaterial;
        private static Sprite cachedWhiteSprite;

        public static GameObject CreateMainCamera(Vector3 position, Vector3 eulerAngles)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = position;
            cameraObject.transform.eulerAngles = eulerAngles;

            Camera camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.12f, 0.13f, 0.16f, 1f);
            camera.orthographic = false;
            camera.fieldOfView = 60f;
            camera.nearClipPlane = 0.3f;
            camera.farClipPlane = 1000f;

            cameraObject.AddComponent<AudioListener>();
            return cameraObject;
        }

        public static GameObject CreateDirectionalLight()
        {
            GameObject lightObject = new GameObject("Directional Light");
            lightObject.transform.eulerAngles = new Vector3(50f, -30f, 0f);

            Light light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = Color.white;
            light.intensity = 1f;
            return lightObject;
        }

        public static GameObject EnsureEventSystem()
        {
            EventSystem existing = Object.FindAnyObjectByType<EventSystem>();
            if (existing != null)
            {
                return existing.gameObject;
            }

            GameObject eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();

            // Input System 패키지 우선, 실패 시 StandaloneInputModule 사용
            System.Type inputSystemUiType = System.Type.GetType(
                "UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");
            if (inputSystemUiType != null)
            {
                eventSystemObject.AddComponent(inputSystemUiType);
            }
            else
            {
                eventSystemObject.AddComponent<StandaloneInputModule>();
            }

            return eventSystemObject;
        }

        public static Canvas CreateCanvas(string name, int sortingOrder = 0)
        {
            GameObject canvasObject = new GameObject(name);
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            canvasObject.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        public static TMP_Text CreateTmpText(
            Transform parent,
            string name,
            string content,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            int fontSize,
            TextAlignmentOptions alignment,
            Color color)
        {
            GameObject textObject = new GameObject(name, typeof(RectTransform));
            textObject.transform.SetParent(parent, false);

            RectTransform rect = textObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;

            TextMeshProUGUI tmp = textObject.AddComponent<TextMeshProUGUI>();
            tmp.text = content;
            tmp.fontSize = fontSize;
            tmp.alignment = alignment;
            tmp.color = color;
            tmp.textWrappingMode = TextWrappingModes.Normal;
            tmp.raycastTarget = false;
            ApplyLessonFont(tmp);
            return tmp;
        }

        /// <summary>
        /// ArtResource/Font의 수업용 TMP Font Asset을 적용합니다.
        /// </summary>
        public static void ApplyLessonFont(TMP_Text tmpText)
        {
            if (tmpText == null)
            {
                return;
            }

            TMP_FontAsset fontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(
                LessonSceneRegistry.LessonFontAssetPath);
            if (fontAsset == null)
            {
                Debug.LogWarning(
                    $"[LessonObjectFactory] 수업 폰트를 찾을 수 없습니다: {LessonSceneRegistry.LessonFontAssetPath}");
                return;
            }

            tmpText.font = fontAsset;
            if (fontAsset.material != null)
            {
                tmpText.fontSharedMaterial = fontAsset.material;
            }
        }

        public static Button CreateButton(
            Transform parent,
            string name,
            string label,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            Color backgroundColor)
        {
            GameObject buttonObject = new GameObject(name, typeof(RectTransform));
            buttonObject.transform.SetParent(parent, false);

            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;

            Image image = buttonObject.AddComponent<Image>();
            image.color = backgroundColor;
            image.sprite = GetWhiteSprite();

            Button button = buttonObject.AddComponent<Button>();
            ColorBlock colors = button.colors;
            colors.highlightedColor = backgroundColor * 1.1f;
            colors.pressedColor = backgroundColor * 0.85f;
            button.colors = colors;

            CreateTmpText(
                buttonObject.transform,
                "Text",
                label,
                Vector2.zero,
                sizeDelta,
                28,
                TextAlignmentOptions.Center,
                Color.white);

            return button;
        }

        public static Image CreateImage(
            Transform parent,
            string name,
            Vector2 anchoredPosition,
            Vector2 sizeDelta,
            Color color)
        {
            GameObject imageObject = new GameObject(name, typeof(RectTransform));
            imageObject.transform.SetParent(parent, false);

            RectTransform rect = imageObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;

            Image image = imageObject.AddComponent<Image>();
            image.color = color;
            image.sprite = GetWhiteSprite();
            return image;
        }

        public static GameObject CreatePrimitive(
            string name,
            PrimitiveType type,
            Vector3 position,
            Vector3 scale,
            Color color,
            Transform parent = null)
        {
            GameObject primitive = GameObject.CreatePrimitive(type);
            primitive.name = name;
            primitive.transform.position = position;
            primitive.transform.localScale = scale;

            if (parent != null)
            {
                primitive.transform.SetParent(parent, true);
            }

            ApplyColor(primitive, color);
            return primitive;
        }

        public static GameObject CreateColoredQuad(
            string name,
            Vector3 position,
            Vector3 scale,
            Color color,
            Transform parent = null)
        {
            // Quad의 기본 법선이 +Z이므로 Sprite처럼 보이도록 사용합니다.
            GameObject quad = CreatePrimitive(name, PrimitiveType.Quad, position, scale, color, parent);
            return quad;
        }

        public static SpriteRenderer CreateSpriteProxy(
            string name,
            Vector3 position,
            Vector3 scale,
            Color color,
            Transform parent = null)
        {
            GameObject spriteObject = new GameObject(name);
            if (parent != null)
            {
                spriteObject.transform.SetParent(parent, false);
            }

            spriteObject.transform.position = position;
            spriteObject.transform.localScale = scale;

            SpriteRenderer renderer = spriteObject.AddComponent<SpriteRenderer>();
            renderer.sprite = GetWhiteSprite();
            renderer.color = color;
            return renderer;
        }

        public static GameObject CreateEmpty(string name, Transform parent = null, Vector3? worldPosition = null)
        {
            GameObject empty = new GameObject(name);
            if (parent != null)
            {
                empty.transform.SetParent(parent, false);
            }

            if (worldPosition.HasValue)
            {
                empty.transform.position = worldPosition.Value;
            }

            return empty;
        }

        public static void ApplyColor(GameObject target, Color color)
        {
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer == null)
            {
                return;
            }

            Material material = GetOrCreateLitMaterialInstance(color);
            renderer.sharedMaterial = material;
        }

        public static Material GetOrCreateLitMaterialInstance(Color color)
        {
            if (cachedLitMaterial == null)
            {
                Shader shader = Shader.Find("Universal Render Pipeline/Lit");
                if (shader == null)
                {
                    shader = Shader.Find("Standard");
                }

                cachedLitMaterial = new Material(shader);
                cachedLitMaterial.name = "LessonLitBase";
            }

            Material instance = new Material(cachedLitMaterial);
            instance.color = color;
            if (instance.HasProperty("_BaseColor"))
            {
                instance.SetColor("_BaseColor", color);
            }

            return instance;
        }

        public static Sprite GetWhiteSprite()
        {
            if (cachedWhiteSprite != null)
            {
                return cachedWhiteSprite;
            }

            Texture2D texture = Texture2D.whiteTexture;
            cachedWhiteSprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f);
            cachedWhiteSprite.name = "LessonWhiteSprite";
            return cachedWhiteSprite;
        }

        public static void SetAnchors(
            RectTransform rect,
            Vector2 anchorMin,
            Vector2 anchorMax,
            Vector2 pivot,
            Vector2 anchoredPosition,
            Vector2 sizeDelta)
        {
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;
        }

        public static void BindButtonMethod(Button button, Object target, string methodName)
        {
            if (button == null || target == null || string.IsNullOrEmpty(methodName))
            {
                return;
            }

            SerializedObject serializedButton = new SerializedObject(button);
            SerializedProperty onClickProperty = serializedButton.FindProperty("m_OnClick");
            SerializedProperty persistentCalls = onClickProperty.FindPropertyRelative("m_PersistentCalls.m_Calls");

            // 기존 persistent call 제거 후 재등록
            persistentCalls.ClearArray();
            persistentCalls.arraySize = 1;
            SerializedProperty call = persistentCalls.GetArrayElementAtIndex(0);
            call.FindPropertyRelative("m_Target").objectReferenceValue = target;
            call.FindPropertyRelative("m_MethodName").stringValue = methodName;
            call.FindPropertyRelative("m_Mode").enumValueIndex = 1; // Void
            call.FindPropertyRelative("m_CallState").enumValueIndex = 2; // RuntimeOnly
            serializedButton.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(button);
        }

        public static void BindButtonLoadScene(Button button, SceneLoader loader, string sceneName)
        {
            if (button == null || loader == null)
            {
                return;
            }

            SerializedObject serializedButton = new SerializedObject(button);
            SerializedProperty onClickProperty = serializedButton.FindProperty("m_OnClick");
            SerializedProperty persistentCalls = onClickProperty.FindPropertyRelative("m_PersistentCalls.m_Calls");

            persistentCalls.ClearArray();
            persistentCalls.arraySize = 1;
            SerializedProperty call = persistentCalls.GetArrayElementAtIndex(0);
            call.FindPropertyRelative("m_Target").objectReferenceValue = loader;
            call.FindPropertyRelative("m_TargetAssemblyTypeName").stringValue =
                typeof(SceneLoader).AssemblyQualifiedName;
            call.FindPropertyRelative("m_MethodName").stringValue = nameof(SceneLoader.LoadScene);
            call.FindPropertyRelative("m_Mode").enumValueIndex = 5; // String
            call.FindPropertyRelative("m_Arguments.m_StringArgument").stringValue = sceneName;
            call.FindPropertyRelative("m_CallState").enumValueIndex = 2;
            serializedButton.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(button);
        }

        public static void SetObjectReference(Object target, string propertyName, Object value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                Debug.LogWarning($"[LessonObjectFactory] Property not found: {target.GetType().Name}.{propertyName}");
                return;
            }

            property.objectReferenceValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }

        public static void SetString(Object target, string propertyName, string value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                return;
            }

            property.stringValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }

        public static void SetFloat(Object target, string propertyName, float value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                return;
            }

            property.floatValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }

        public static void SetEnum(Object target, string propertyName, int enumValue)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                return;
            }

            // DOTween Ease처럼 underlying int가 불연속인 enum은 intValue로 설정합니다.
            property.intValue = enumValue;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }

        public static void SetInt(Object target, string propertyName, int value)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property == null)
            {
                return;
            }

            property.intValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }

        public static void SetObjectReferenceArray(Object target, string propertyName, Object[] values)
        {
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty property = serializedObject.FindProperty(propertyName);
            if (property == null || !property.isArray)
            {
                Debug.LogWarning($"[LessonObjectFactory] Array property not found: {propertyName}");
                return;
            }

            property.arraySize = values.Length;
            for (int index = 0; index < values.Length; index++)
            {
                property.GetArrayElementAtIndex(index).objectReferenceValue = values[index];
            }

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(target);
        }
    }
}
