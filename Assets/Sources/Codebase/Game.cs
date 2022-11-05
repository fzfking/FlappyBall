using System;
using System.Linq;
using Sources.Codebase.Helpers;
using Unity.Mathematics;
using UnityEngine;

namespace Sources.Codebase
{
    public class Game : MonoBehaviour, ICoroutineRunner
    {
        private const string AttemptsKey = "Attempts";
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
        private readonly Line[] _lines = new Line[2];
        private TimeCounter _timeCounter;
        private int _attemptsCount;

        private void Start()
        {
            _attemptsCount = PlayerPrefs.GetInt(AttemptsKey, 0);
            _mainCamera = Camera.main;
            InstallMainMenu();
            InstallGameLoopUI();
            InstallEndGameUI();
            InstallGameLevel();
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
            _levelRoot.transform.position = Vector3.zero;
            _flappyBall = Instantiate(FlappyBallPrefab, _levelRoot.transform);
            _lines[0] = Instantiate(LinePrefab, _levelRoot.transform);
            _lines[0].Transform.position = new Vector3(5, 4.5f, 0);
            _lines[1] = Instantiate(LinePrefab, _levelRoot.transform);
            _lines[1].Transform.position = new Vector3(5, -4.5f, 0);

            _gameLoopUIRoot.UpButton.OnButtonDown += () => _flappyBall.GetUp();
            _gameLoopUIRoot.UpButton.OnButtonUp += () => _flappyBall.GetDown();

            _levelRoot.gameObject.SetActive(false);

        }

        private void UpdateDifficultyForLines()
        {
            foreach (var line in _lines)
            {
                line.SetSpeed(_selectedDifficulty);
            }
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
            _endGameUIRoot.OnRestartClicked += RestartLevelAndDisableEndGameUI;
            _endGameUIRoot.OnChangeDifficultyClicked += GoToMainMenu;
            _endGameUIRoot.gameObject.SetActive(false);
        }

        private void GoToMainMenu()
        {
            _endGameUIRoot.gameObject.SetActive(false);
            _mainMenuRoot.gameObject.SetActive(true);
        }

        private void RestartLevelAndDisableEndGameUI()
        {
            _endGameUIRoot.gameObject.SetActive(false);
            ResetAndEnableLevel();
        }

        private void ResetAndEnableLevel()
        {
            _flappyBall.Reset();
            _flappyBall.OnCollidedWithLine += BallCollidedWithLineHandler;
            _levelRoot.gameObject.SetActive(true);
            _timeCounter.OnTimePassed += UpdatePassedTimeLabel;
            _timeCounter.Start();
        }

        private void EndGame()
        {
            _attemptsCount++;
            _levelRoot.gameObject.SetActive(false);
            _endGameUIRoot.gameObject.SetActive(true);
            _timeCounter.OnTimePassed -= UpdatePassedTimeLabel;
            _endGameUIRoot.SetLastAttemptTime(_timeCounter.Stop());
            _endGameUIRoot.SetAttemptsCount(_attemptsCount);
        }

        private void InstallGameLoopUI()
        {
            _levelRoot = new GameObject("LevelRoot");
            _gameLoopUIRoot = Instantiate(GameLoopUIRootPrefab, _levelRoot.transform);
            _gameLoopUIRoot.Canvas.worldCamera = _mainCamera;
            //_gameLoopUIRoot.gameObject.SetActive(false);
        }

        private void ChangeDifficulty(int dropdownIndex)
        {
            if (AllDifficultiesInts.Contains(dropdownIndex))
            {
                _selectedDifficulty = (Difficulty)dropdownIndex;
                Debug.Log("Player selected difficulty: " + _selectedDifficulty);
                UpdateDifficultyForLines();
                return;
            }

            Debug.LogError("Player selected dropdownItem that not exists in Difficulty enum");
        }

        private void StartLevel()
        {
            Debug.Log($"Player started game with {_selectedDifficulty} difficulty");
            _mainMenuRoot.gameObject.SetActive(false);
            _gameLoopUIRoot.gameObject.SetActive(true);
            ResetAndEnableLevel();
            
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt(AttemptsKey, _attemptsCount);
            PlayerPrefs.Save();
        }
    }
}