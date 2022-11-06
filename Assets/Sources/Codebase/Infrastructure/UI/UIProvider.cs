using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace Sources.Codebase.Infrastructure.UI
{
    public class UIProvider
    {
        public PressableButton GameLoopUIButtonUp => _gameLoopUIRoot.UpButton;
        private MainMenuRoot _mainMenuRoot;
        private GameLoopUIRoot _gameLoopUIRoot;
        private EndGameUIRoot _endGameUIRoot;
        private readonly Camera _mainCamera;

        public UIProvider()
        {
            _mainCamera = Camera.main;
        }

        public void EnableMainMenu()
        {
            _mainMenuRoot.gameObject.SetActive(true);
        }

        public void DisableMainMenu()
        {
            _mainMenuRoot.gameObject.SetActive(false);
        }

        public void EnableEndGameMenu()
        {
            _endGameUIRoot.gameObject.SetActive(true);
        }

        public void DisableEndGameMenu()
        {
            _endGameUIRoot.gameObject.SetActive(false);
        }

        public void SetTimePassedToGameLoopUI(TimeSpan timeSpan)
        {
            _gameLoopUIRoot.SetTimePassedLabel(timeSpan);
        }

        public void InstallMainMenu(MainMenuRoot mainMenuRootPrefab, UnityAction<int> onValueChangedCallback,
            UnityAction onStartButtonClickCallback)
        {
            _mainMenuRoot = Object.Instantiate(mainMenuRootPrefab);
            _mainMenuRoot.DifficultyDropdown.onValueChanged.AddListener(onValueChangedCallback);
            _mainMenuRoot.StartButton.onClick.AddListener(onStartButtonClickCallback);
            _mainMenuRoot.Canvas.worldCamera = _mainCamera;
        }

        public void InstallGameLoopUI(GameLoopUIRoot gameLoopUIRootPrefab, Transform levelRoot)
        {
            _gameLoopUIRoot = Object.Instantiate(gameLoopUIRootPrefab, levelRoot.transform);
            _gameLoopUIRoot.Canvas.worldCamera = _mainCamera;
        }

        public void InstallEndGameUI(EndGameUIRoot endGameUIRootPrefab, Action onRestartClickedCallback,
            Action onChangeDifficultyClickedCallback)
        {
            _endGameUIRoot = Object.Instantiate(endGameUIRootPrefab);
            _endGameUIRoot.Canvas.worldCamera = _mainCamera;
            _endGameUIRoot.OnRestartClicked += onRestartClickedCallback;
            _endGameUIRoot.OnChangeDifficultyClicked += onChangeDifficultyClickedCallback;
            _endGameUIRoot.gameObject.SetActive(false);
        }

        public void SetLastAttemptTimeToEndMenu(TimeSpan timeSpan)
        {
            _endGameUIRoot.SetLastAttemptTime(timeSpan);
        }

        public void SetAttemptsCount(int attemptsCount)
        {
            _endGameUIRoot.SetAttemptsCount(attemptsCount);
        }
    }
}