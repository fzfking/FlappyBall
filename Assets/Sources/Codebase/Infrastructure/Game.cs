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
        private TimeCounter _timeCounter;
        private int _attemptsCount;
        
        private UIProvider _uiProvider;
        private Level _level;

        private void Start()
        {
            _attemptsCount = PlayerPrefs.GetInt(AttemptsKey, 0);
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = BackgroundAudioClip;
            audioSource.Play();
            _level = new Level(this, FlappyBallPrefab, LinePrefab, EnemyBallPrefab, EndGame);
            _level.InstallGameLevel();
            _uiProvider = new UIProvider();
            _uiProvider.InstallMainMenu(MainMenuRootPrefab, ChangeDifficulty, StartLevel);
            _uiProvider.InstallGameLoopUI(GameLoopUIRootPrefab, _level.LevelRoot.transform);
            _uiProvider.GameLoopUIButtonUp.OnButtonDown += () => _level.FlappyBall.GetUp();
            _uiProvider.GameLoopUIButtonUp.OnButtonUp += () => _level.FlappyBall.GetDown();
            _uiProvider.InstallEndGameUI(EndGameUIRootPrefab, RestartLevelAndDisableEndGameUI, GoToMainMenu);
            _timeCounter = new TimeCounter(this);
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
            _level.ResetAndEnableLevel();
            _timeCounter.OnTimePassed += _uiProvider.SetTimePassedToGameLoopUI;
            _timeCounter.Start();
        }

        private void EndGame()
        {
            _level.DisableLevel();
            _attemptsCount++;
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
                _level.UpdateDifficultyForLines(_selectedDifficulty);
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