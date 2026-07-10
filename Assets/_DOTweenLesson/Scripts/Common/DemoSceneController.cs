using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 공통 Demo HUD와 예제별 Controller를 연결합니다.
/// Play / Reset / NextScene 버튼 이벤트를 중계합니다.
/// </summary>
public class DemoSceneController : MonoBehaviour
{
    [Header("HUD References")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private Button playButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button nextSceneButton;

    [Header("Scene Info")]
    [SerializeField] private string demoTitle;
    [SerializeField] private string demoDescription;
    [SerializeField] private string nextSceneName;

    [Header("Demo Target")]
    [SerializeField] private MonoBehaviour demoBehaviour;
    [SerializeField] private Transform stageRoot;
    [SerializeField] private SceneLoader sceneLoader;

    private IPlayableDemo playableDemo;
    private IResettableDemo resettableDemo;

    private void Awake()
    {
        BindDemoInterfaces();
        ApplyHudText();
        WireButtons();
    }

    /// <summary>
    /// 에디터 도구에서 HUD와 Demo 참조를 주입할 때 사용합니다.
    /// </summary>
    public void Configure(
        string title,
        string description,
        string nextScene,
        MonoBehaviour demo,
        Transform stage,
        SceneLoader loader,
        TMP_Text titleLabel,
        TMP_Text descriptionLabel,
        Button play,
        Button reset,
        Button next)
    {
        demoTitle = title;
        demoDescription = description;
        nextSceneName = nextScene;
        demoBehaviour = demo;
        stageRoot = stage;
        sceneLoader = loader;
        titleText = titleLabel;
        descriptionText = descriptionLabel;
        playButton = play;
        resetButton = reset;
        nextSceneButton = next;

        BindDemoInterfaces();
        ApplyHudText();
    }

    private void BindDemoInterfaces()
    {
        playableDemo = demoBehaviour as IPlayableDemo;
        resettableDemo = demoBehaviour as IResettableDemo;
    }

    private void ApplyHudText()
    {
        if (titleText != null)
        {
            titleText.text = demoTitle;
        }

        if (descriptionText != null)
        {
            descriptionText.text = demoDescription;
        }
    }

    private void WireButtons()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveListener(OnPlayClicked);
            playButton.onClick.AddListener(OnPlayClicked);
        }

        if (resetButton != null)
        {
            resetButton.onClick.RemoveListener(OnResetClicked);
            resetButton.onClick.AddListener(OnResetClicked);
        }

        if (nextSceneButton != null)
        {
            nextSceneButton.onClick.RemoveListener(OnNextSceneClicked);
            nextSceneButton.onClick.AddListener(OnNextSceneClicked);
        }
    }

    public void OnPlayClicked()
    {
        if (playableDemo == null)
        {
            BindDemoInterfaces();
        }

        playableDemo?.Play();
    }

    public void OnResetClicked()
    {
        if (resettableDemo == null)
        {
            BindDemoInterfaces();
        }

        resettableDemo?.ResetDemo();
        TweenKillHelper.KillAllInHierarchy(stageRoot);
        TweenKillHelper.KillCanvasGroups(stageRoot);
        TweenKillHelper.KillSpriteRenderers(stageRoot);
    }

    public void OnNextSceneClicked()
    {
        // Scene 전환 전에 모든 Tween을 종료해 Destroy된 대상 접근 경고를 방지합니다.
        if (resettableDemo == null)
        {
            BindDemoInterfaces();
        }

        resettableDemo?.ResetDemo();
        TweenKillHelper.KillAllInHierarchy(stageRoot);
        TweenKillHelper.KillCanvasGroups(stageRoot);
        TweenKillHelper.KillSpriteRenderers(stageRoot);
        DOTween.KillAll();

        if (sceneLoader == null)
        {
            sceneLoader = GetComponent<SceneLoader>();
            if (sceneLoader == null)
            {
                sceneLoader = gameObject.AddComponent<SceneLoader>();
            }
        }

        sceneLoader.LoadScene(nextSceneName);
    }

    private void OnDestroy()
    {
        DOTween.Kill(gameObject);
        if (stageRoot != null)
        {
            TweenKillHelper.KillAllInHierarchy(stageRoot);
        }
    }
}
