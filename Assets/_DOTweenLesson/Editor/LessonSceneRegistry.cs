namespace DOTweenLesson.Editor
{
    /// <summary>
    /// 수업 Scene 메타데이터입니다.
    /// </summary>
    public readonly struct LessonSceneInfo
    {
        public readonly string SceneName;
        public readonly string Title;
        public readonly string Description;
        public readonly string NextSceneName;

        public LessonSceneInfo(string sceneName, string title, string description, string nextSceneName)
        {
            SceneName = sceneName;
            Title = title;
            Description = description;
            NextSceneName = nextSceneName;
        }
    }

    /// <summary>
    /// 00~11 Scene 목록과 경로 상수를 보관합니다.
    /// </summary>
    public static class LessonSceneRegistry
    {
        public const string RootFolder = "Assets/_DOTweenLesson";
        public const string ScenesFolder = RootFolder + "/Scenes";
        public const string PrefabsFolder = RootFolder + "/Prefabs";
        public const string MaterialsFolder = RootFolder + "/Materials";
        public const string DemoHudPrefabPath = PrefabsFolder + "/Common/Canvas_DemoHUD.prefab";
        public const string FloatingDamagePrefabPath = PrefabsFolder + "/Effects/FloatingDamageText.prefab";
        public const string LessonFontAssetPath =
            RootFolder + "/ArtResource/Font/Cloudsofa_namgim-Regular SDF.asset";

        public static readonly LessonSceneInfo[] AllScenes =
        {
            new LessonSceneInfo(
                "00_DemoHub",
                "DOTween Lesson Hub",
                "예제 Scene을 선택하세요.",
                "01_Interpolation_Lerp"),
            new LessonSceneInfo(
                "01_Interpolation_Lerp",
                "01. Interpolation Lerp",
                "Duration Lerp와 Damping Lerp의 차이를 비교합니다. Play로 두 Mover를 실행하세요.",
                "02_EaseGraph_Comparison"),
            new LessonSceneInfo(
                "02_EaseGraph_Comparison",
                "02. Ease Graph Comparison",
                "같은 거리·시간에서도 Ease에 따라 체감이 달라집니다.",
                "03_DOTween_TransformBasic"),
            new LessonSceneInfo(
                "03_DOTween_TransformBasic",
                "03. Transform Basic",
                "DOMove / DOScale / DORotate / DOFade 기본 API를 확인합니다.",
                "04_UI_ButtonFeedback"),
            new LessonSceneInfo(
                "04_UI_ButtonFeedback",
                "04. UI Button Feedback",
                "버튼 Scale 피드백과 Invalid Shake를 체험합니다.",
                "05_UI_Popup"),
            new LessonSceneInfo(
                "05_UI_Popup",
                "05. UI Popup",
                "CanvasGroup Fade + Scale Sequence로 팝업을 열고 닫습니다.",
                "06_Game_ItemCollect"),
            new LessonSceneInfo(
                "06_Game_ItemCollect",
                "06. Game Item Collect",
                "코인을 수집하면 Player로 이동하며 재화 UI가 반응합니다.",
                "07_Game_HitFeedback"),
            new LessonSceneInfo(
                "07_Game_HitFeedback",
                "07. Game Hit Feedback",
                "Attack으로 Shake, Hit Flash, HP Bar, Floating Damage를 확인합니다.",
                "08_Game_QuestMarker"),
            new LessonSceneInfo(
                "08_Game_QuestMarker",
                "08. Game Quest Marker",
                "무한 Yoyo Tween으로 시선 유도 마커를 만듭니다.",
                "09_Game_RewardSequence"),
            new LessonSceneInfo(
                "09_Game_RewardSequence",
                "09. Game Reward Sequence",
                "상자 열림 → 팝업 → 재화 증가 Sequence를 구성합니다.",
                "10_CardHover_Select"),
            new LessonSceneInfo(
                "10_CardHover_Select",
                "10. Card Hover Select",
                "카드 Hover / Select Tween을 확인합니다.",
                "11_CameraShake_TimeScale"),
            new LessonSceneInfo(
                "11_CameraShake_TimeScale",
                "11. Camera Shake & TimeScale",
                "Camera Shake와 Time.timeScale=0에서도 동작하는 UI Tween(SetUpdate)을 확인합니다.",
                "00_DemoHub"),
        };

        public static string GetScenePath(string sceneName)
        {
            return $"{ScenesFolder}/{sceneName}.unity";
        }
    }
}
