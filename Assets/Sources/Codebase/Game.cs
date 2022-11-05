using System;
using System.Linq;
using Sources.Codebase.Helpers;
using Unity.Mathematics;
using UnityEngine;

namespace Sources.Codebase
{
    public class Game : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private MainMenuRoot MainMenuRootPrefab;
        [SerializeField] private GameLoopUIRoot GameLoopUIRootPrefab;
        [SerializeField] private EndGameUIRoot EndGameUIRootPrefab;
        [SerializeField] private FlappyBall FlappyBallPrefab;
        [SerializeField] private Line LinePrefab;
        private Camera _mainCamera;
        private MainMenuRoot _mainMenuRoot;
        private GameLoopUIRoot _gameLoopUIRoot;
        private EndGameUIRoot _endGameUIRoot;
        private Difficulty _selectedDifficulty = Difficulty.Easy;
        private int[] _allDifficultiesInts;
        private int[] AllDifficultiesInts => _allDifficultiesInts ??= ((int[])Enum.GetValues(typeof(Difficulty)));
        private FlappyBall _flappyBall;
        private GameObject _levelRoot;
        private Line[] _lines = new Line[2];
        private TimeCounter _timeCounter;

        private void Start()
        {
            _mainCamera = Camera.main;
            InstallMainMenu();
            InstallGameLoopUI();
            InstallEndGameUI();
            _timeCounter = new TimeCounter(this);
        }

        private void InstallMainMenu()
        {
            _mainMenuRoot = Instantiate(MainMenuRootPrefab);
            _mainMenuRoot.DifficultyDropdown.onValueChanged.AddListener(ChangeDifficulty);
            _mainMenuRoot.StartButton.onClick.AddListener(StartLevel);
            _mainMenuRoot.Canvas.worldCamera = _mainCamera;
        }

        private void InstallGameLevel()
        {
            _levelRoot = new GameObject("LevelRoot");
            _levelRoot.transform.position = Vector3.zero;
            _flappyBall = Instantiate(FlappyBallPrefab, _levelRoot.transform);
            _lines[0] = Instantiate(LinePrefab, _levelRoot.transform);
            _lines[0].Transform.position = new Vector3(5, 4.5f, 0);
            _lines[1] = Instantiate(LinePrefab, _levelRoot.transform);
            _lines[1].Transform.position = new Vector3(5, -4.5f, 0);
            foreach (var line in _lines)
            {
                line.SetSpeed(_selectedDifficulty);
            }

            _gameLoopUIRoot.UpButton.OnButtonDown += () => _flappyBall.GetUp();
            _gameLoopUIRoot.UpButton.OnButtonUp += () => _flappyBall.GetDown();

            _flappyBall.OnCollidedWithLine += BallCollidedWithLineHandler;

            _timeCounter.OnTimePassed += UpdatePassedTimeLabel;
            _timeCounter.Start();
        }

        private void BallCollidedWithLineHandler()
        {
            _flappyBall.OnCollidedWithLine -= BallCollidedWithLineHandler; 
            EndGame();
        }

        private void UpdatePassedTimeLabel(TimeSpan timeSpan)
        {
            _gameLoopUIRoot.SetTimePassedLabel(timeSpan);
        }

        private void InstallEndGameUI()
        {
            _endGameUIRoot = Instantiate(EndGameUIRootPrefab);
            _endGameUIRoot.Canvas.worldCamera = _mainCamera;
            _endGameUIRoot.gameObject.SetActive(false);
        }

        private void EndGame()
        {
            _levelRoot.gameObject.SetActive(false);
            _gameLoopUIRoot.gameObject.SetActive(false);
            _endGameUIRoot.gameObject.SetActive(true);
            _timeCounter.OnTimePassed -= UpdatePassedTimeLabel;
            _endGameUIRoot.SetLastAttemptTime(_timeCounter.Stop());
        }

        private void InstallGameLoopUI()
        {
            _gameLoopUIRoot = Instantiate(GameLoopUIRootPrefab);
            _gameLoopUIRoot.Canvas.worldCamera = _mainCamera;
            _gameLoopUIRoot.gameObject.SetActive(false);
        }

        private void ChangeDifficulty(int dropdownIndex)
        {
            if (AllDifficultiesInts.Contains(dropdownIndex))
            {
                _selectedDifficulty = (Difficulty)dropdownIndex;
                Debug.Log("Player selected difficulty: " + _selectedDifficulty);
                return;
            }

            Debug.LogError("Player selected dropdownItem that not exists in Difficulty enum");
        }

        private void StartLevel()
        {
            Debug.Log($"Player started game with {_selectedDifficulty} difficulty");
            _mainMenuRoot.gameObject.SetActive(false);
            _gameLoopUIRoot.gameObject.SetActive(true);
            InstallGameLevel();
        }

        public void StopCoroutine()
        {
            throw new NotImplementedException();
        }
    }
}