using DG.Tweening;
using UnityEngine;

/// <summary>
/// 11_CameraShake_TimeScale Scene 컨트롤러입니다.
/// Shake / Pause(TimeScale) / Pause Popup을 제어합니다.
/// </summary>
public class CameraShakeTimeScaleController : MonoBehaviour, IPlayableDemo, IResettableDemo
{
    [SerializeField] private CameraShakeTween cameraShake;
    [SerializeField] private PausePopupTween pausePopup;
    [SerializeField] private Transform movingDemoTarget;

    private Vector3 movingDemoStartPosition;
    private bool isPaused;

    private void Awake()
    {
        if (movingDemoTarget != null)
        {
            movingDemoStartPosition = movingDemoTarget.position;
        }
    }

    public void Configure(CameraShakeTween shake, PausePopupTween popup, Transform movingTarget)
    {
        cameraShake = shake;
        pausePopup = popup;
        movingDemoTarget = movingTarget;

        if (movingDemoTarget != null)
        {
            movingDemoStartPosition = movingDemoTarget.position;
        }
    }

    public void Play()
    {
        OnShakeClicked();
    }

    public void ResetDemo()
    {
        isPaused = false;
        Time.timeScale = 1f;
        cameraShake?.ResetShake();
        pausePopup?.HideInstant();

        if (movingDemoTarget != null)
        {
            movingDemoTarget.DOKill();
            movingDemoTarget.position = movingDemoStartPosition;
        }
    }

    public void OnShakeClicked()
    {
        cameraShake?.PlayShake();
    }

    public void OnPauseToggleClicked()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void OnShowPausePopupClicked()
    {
        Time.timeScale = 0f;
        isPaused = true;
        pausePopup?.Show();
    }

    public void OnHidePausePopupClicked()
    {
        pausePopup?.Hide();
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        // TimeScale 영향을 보여주기 위한 단순 왕복 이동
        if (movingDemoTarget == null || isPaused)
        {
            return;
        }

        float offsetX = Mathf.Sin(Time.time * 2f) * 1.5f;
        movingDemoTarget.position = movingDemoStartPosition + new Vector3(offsetX, 0f, 0f);
    }
}
