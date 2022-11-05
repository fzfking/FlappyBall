using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources.Codebase.Helpers
{
    public class EnemyBallsGenerator
    {
        private readonly EnemyBallsPool _enemyBallsPool;
        private readonly ICoroutineRunner _coroutineRunner;
        private const float StartingXAxis = 8f;
        private float _timeBetweenSpawns;
        private Difficulty _difficulty;
        private Coroutine _currentGeneratingRoutine;

        public EnemyBallsGenerator(EnemyBallsPool pool, ICoroutineRunner coroutineRunner)
        {
            _enemyBallsPool = pool;
            _coroutineRunner = coroutineRunner;
        }

        public void Enable(Difficulty difficulty)
        {
            _difficulty = difficulty;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    _timeBetweenSpawns = 8;
                    break;
                case Difficulty.Medium:
                    _timeBetweenSpawns = 3;
                    break;
                case Difficulty.Hard:
                    _timeBetweenSpawns = 1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }

            _currentGeneratingRoutine = _coroutineRunner.StartCoroutine(GenerateRandomObstacles());
        }

        public void Disable()
        {
            _coroutineRunner.StopCoroutine(_currentGeneratingRoutine);
            _enemyBallsPool.Reset();
        }

        private IEnumerator GenerateRandomObstacles()
        {
            while (true)
            {
                var actualEnemy = _enemyBallsPool.Spawn();
                actualEnemy.transform.position = new Vector3(StartingXAxis, Random.Range(-3, 3));
                actualEnemy.SetDifficulty(_difficulty);
                actualEnemy.gameObject.SetActive(true);
                yield return new WaitForSeconds(_timeBetweenSpawns);
            }

        }
    }
}