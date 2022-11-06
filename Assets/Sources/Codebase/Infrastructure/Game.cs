using System;
using System.Linq;
using Sources.Codebase.GameEntities;
using Sources.Codebase.Helpers;
using Sources.Codebase.Infrastructure.UI;
using UnityEngine;

namespace Sources.Codebase.Infrastructure
{
    [RequireComponent(typeof(AudioSource))]
    public class Game : MonoBehaviour, ICoroutineRunner
    {
        private const string AttemptsKey = "Attempts";
        [SerializeField] private MainMenuRoot MainMenuRootPrefab;
        [SerializeField] private GameLoopUIRoot GameLoopUIRootPrefab;
        [SerializeField] private EndGameUIRoot EndGameUIRootPrefab;
        [SerializeField] private FlappyBall FlappyBallPrefab;
        [SerializeField] private Line LinePrefab;
        [SerializeField] private EnemyBall EnemyBallPrefab;
        [SerializeField] private AudioClip BackgroundAudioClip;

        private Difficulty _selectedDifficulty = Difficulty.Easy;
        private int[] _allDifficultiesInts;
        private int[] AllDifficultiesInts => _allDifficultiesInts ??= ((int[])Enum.GetValues(typeof(Difficulty)));
        private FlappyBall _flappyBall;
        private GameObject _levelRoot;
        private readonly Line[] _lines = new Line[2];
        private TimeCounter _timeCounter;
        private int _attemptsCount;
        private EnemyBallsGenerator _enemyBallsGenerator;
        private UIProvider _uiProvider;

        private void Start()
        {
            _attemptsCount = PlayerPrefs.GetInt(AttemptsKey, 0);
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = BackgroundAudioClip;
            audioSource.Play();
            _uiProvider = new UIProvider();
            _uiProvider.InstallMainMenu(MainMenuRootPrefab, ChangeDifficulty, StartLevel);
            _levelRoot = new GameObject("Level Root");
            _uiProvider.InstallGameLoopUI(GameLoopUIRootPrefab, _levelRoot.transform);
            _uiProvider.InstallEndGameUI(EndGameUIRootPrefab, RestartLevelAndDisableEndGameUI, GoToMainMenu);
            InstallGameLevel();
            _timeCounter = new TimeCounter(this);
        }


        private void InstallGameLevel()
        {
            var linePositionOffset = new Vector3(0, 4.5f, 0);
            _levelRoot.transform.position = Vector3.zero;
            _flappyBall = Instantiate(FlappyBallPrefab, _levelRoot.transform);
            _lines[0] = Instantiate(LinePrefab, _levelRoot.transform);
            _lines[0].Transform.position = linePositionOffset;
            _lines[1] = Instantiate(LinePrefab, _levelRoot.transform);
            _lines[1].Transform.position = -linePositionOffset;

            _uiProvider.GameLoopUIButtonUp.OnButtonDown += () => _flappyBall.GetUp();
            _uiProvider.GameLoopUIButtonUp.OnButtonUp += () => _flappyBall.GetDown();
            InstallEnemyGenerator();
            _levelRoot.gameObject.SetActive(false);
        }

        private void InstallEnemyGenerator()
        {
            var enemyBallsPool = new EnemyBallsPool(EnemyBallPrefab, _levelRoot.transform);
            _enemyBallsGenerator = new EnemyBallsGenerator(enemyBallsPool, this);
        }

        private void UpdateDifficultyForLines()
        {
            foreach (var line in _lines)
            {
                line.SetSpeed(_selectedDifficulty);
            }
        }

        private void BallCollidedWithObstacleHandler()
        {
            _flappyBall.OnCollidedWithObstacle -= BallCollidedWithObstacleHandler;
            EndGame();
        }

        private void GoToMainMenu()
        {
            _uiProvider.DisableEndGameMenu();
            _uiProvider.EnableMainMenu();
        }

        private void RestartLevelAndDisableEndGameUI()
        {
            _uiProvider.DisableEndGameMenu();
            ResetAndEnableLevel();
        }

        private void ResetAndEnableLevel()
        {
            _flappyBall.Reset();
            _flappyBall.OnCollidedWithObstacle += BallCollidedWithObstacleHandler;
            _levelRoot.gameObject.SetActive(true);
            _enemyBallsGenerator.Enable(_selectedDifficulty);
            _timeCounter.OnTimePassed += _uiProvider.SetTimePassedToGameLoopUI;
            _timeCounter.Start();
        }

        private void EndGame()
        {
            _attemptsCount++;
            _enemyBallsGenerator.Disable();
            _levelRoot.gameObject.SetActive(false);
            _uiProvider.EnableEndGameMenu();
            _timeCounter.OnTimePassed -= _uiProvider.SetTimePassedToGameLoopUI;
            _uiProvider.SetLastAttemptTimeToEndMenu(_timeCounter.Stop());
            _uiProvider.SetAttemptsCount(_attemptsCount);
        }


        private void ChangeDifficulty(int dropdownIndex)
        {
            if (AllDifficultiesInts.Contains(dropdownIndex))
            {
                _selectedDifficulty = (Difficulty)dropdownIndex;
                UpdateDifficultyForLines();
            }
        }

        private void StartLevel()
        {
            _uiProvider.DisableMainMenu();
            ResetAndEnableLevel();
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt(AttemptsKey, _attemptsCount);
            PlayerPrefs.Save();
        }
    }
}