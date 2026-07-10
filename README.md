# Tweening

Unity DOTween examples for learning interpolation, easing, transform tweens, UI feedback, and common game feedback patterns.

## Overview

This project organizes each tweening topic into an independent Unity scene. The examples are designed so each scene can be opened, played, reset, and inspected on its own.

## Requirements

- Unity `6000.4.4f1`
- Universal Render Pipeline
- TextMesh Pro
- DOTween

DOTween is included under `Assets/Plugins/Demigiant/DOTween`.

## Scenes

The main entry scene is:

- `Assets/_DOTweenLesson/Scenes/00_DemoHub.unity`

Example scenes:

- `01_Interpolation_Lerp` - duration-based Lerp and damping-style Lerp comparison
- `02_EaseGraph_Comparison` - ease curve and movement comparison
- `03_DOTween_TransformBasic` - `DOMove`, `DOScale`, `DORotate`, `DOFade`, `SetEase`, `SetLoops`
- `04_UI_ButtonFeedback` - button click, reward, and invalid-input feedback
- `05_UI_Popup` - popup open and close sequence with `CanvasGroup`
- `06_Game_ItemCollect` - collectible item movement and currency UI feedback
- `07_Game_HitFeedback` - hit shake, color flash, HP bar tween, floating damage text, camera shake
- `08_Game_QuestMarker` - looping quest marker and interactable object attention effects
- `09_Game_RewardSequence` - reward popup and currency sequence
- `10_CardHover_Select` - card hover and selection tween
- `11_CameraShake_TimeScale` - camera shake and unscaled-time UI tween

## How to Run

1. Open the project in Unity `6000.4.4f1`.
2. Open `Assets/_DOTweenLesson/Scenes/00_DemoHub.unity`.
3. Enter Play Mode.
4. Use the hub buttons to move between examples.

Each example scene also includes a HUD with Play, Reset, and Next controls.

## Project Layout

```text
Assets/_DOTweenLesson
+-- Scenes
+-- Scripts
|   +-- Common
|   +-- EaseGraph
|   +-- GameFeedback
|   +-- Interpolation
|   +-- TransformBasic
|   +-- UI
+-- Prefabs
+-- Materials
+-- ScriptableObjects
```

## Validation

The C# project can be checked with:

```powershell
dotnet build Assembly-CSharp.csproj
```
