using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// DemoHub 및 NextScene 버튼에서 Scene을 로드합니다.
/// </summary>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// 지정한 Scene 이름으로 로드합니다.
    /// </summary>
    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("[SceneLoader] Scene 이름이 비어 있습니다.");
            return;
        }

        // Scene 전환 직전 잔존 Tween을 모두 종료합니다.
        DOTween.KillAll();
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Build Settings 인덱스로 Scene을 로드합니다.
    /// </summary>
    public void LoadSceneByIndex(int buildIndex)
    {
        if (buildIndex < 0 || buildIndex >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning($"[SceneLoader] 유효하지 않은 Build Index: {buildIndex}");
            return;
        }

        DOTween.KillAll();
        SceneManager.LoadScene(buildIndex);
    }
}
