using System;
using Sources.Codebase.GameEntities;
using Sources.Codebase.Helpers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sources.Codebase.Infrastructure
{
    public class Level
    {
        private readonly GameObject _levelRoot;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly FlappyBall _flappyBallPrefab;
        private readonly Line _linePrefab;
        private readonly EnemyBall _enemyBallPrefab;
        private readonly Action _collidedWithObstacleCallback;
        private FlappyBall _flappyBall;
        private EnemyBallsGenerator _enemyBallsGenerator;
        private readonly Line[] _lines = new Line[2];
        private Difficulty _selectedDifficulty;
        public FlappyBall FlappyBall => _flappyBall;
        public GameObject LevelRoot => _levelRoot;
        
        public Level(ICoroutineRunner coroutineRunner, FlappyBall flappyBallPrefab, Line linePrefab, EnemyBall enemyBallPrefab, Action collidedWithObstacleCallback)
        {
            _coroutineRunner = coroutineRunner;
            _flappyBallPrefab = flappyBallPrefab;
            _linePrefab = linePrefab;
            _enemyBallPrefab = enemyBallPrefab;
            _collidedWithObstacleCallback = collidedWithObstacleCallback;
            _levelRoot = new GameObject("Level Root");
        }

        public void InstallGameLevel()
        {
            var linePositionOffset = new Vector3(0, 4.5f, 0);
            _levelRoot.transform.position = Vector3.zero;
            _flappyBall = Object.Instantiate(_flappyBallPrefab, _levelRoot.transform);
            _lines[0] = Object.Instantiate(_linePrefab, _levelRoot.transform);
            _lines[0].Transform.position = linePositionOffset;
            _lines[1] = Object.Instantiate(_linePrefab, _levelRoot.transform);
            _lines[1].Transform.position = -linePositionOffset;
            InstallEnemyGenerator();
            _levelRoot.gameObject.SetActive(false);
        }
        
        public void ResetAndEnableLevel()
        {
            _flappyBall.Reset();
            _flappyBall.OnCollidedWithObstacle += BallCollidedWithObstacleHandler;
            _levelRoot.gameObject.SetActive(true);
            _enemyBallsGenerator.Enable(_selectedDifficulty);
        }
        
        public void DisableLevel()
        {
            _enemyBallsGenerator.Disable();
            _levelRoot.gameObject.SetActive(false);
        }

        public void UpdateDifficultyForLines(Difficulty difficulty)
        {
            foreach (var line in _lines)
            {
                line.SetSpeed(difficulty);
            }

            _selectedDifficulty = difficulty;
        }

        private void BallCollidedWithObstacleHandler()
        {
            _flappyBall.OnCollidedWithObstacle -= BallCollidedWithObstacleHandler;
            _collidedWithObstacleCallback?.Invoke();
        }

        private void InstallEnemyGenerator()
        {
            var enemyBallsPool = new EnemyBallsPool(_enemyBallPrefab, _levelRoot.transform);
            _enemyBallsGenerator = new EnemyBallsGenerator(enemyBallsPool, _coroutineRunner);
        }
    }
}