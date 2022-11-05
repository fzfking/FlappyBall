using System;
using System.Collections;
using Sources.Codebase.Helpers;
using UnityEngine;

namespace Sources.Codebase
{
    public class EnemyBall : MonoBehaviour
    {
        [SerializeField] private Vector3 OffScreenEdge;
        [SerializeField] private float DefaultSpeedMultiplier;
        private Coroutine _currentMovementRoutine;
        private float _speed = 1f;

        public void SetDifficulty(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Difficulty.Easy:
                    _speed = 0.5f;
                    break;
                case Difficulty.Medium:
                    _speed = 1f;
                    break;
                case Difficulty.Hard:
                    _speed = 2f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }
        }

        private void OnEnable()
        {
            _currentMovementRoutine = StartCoroutine(Move());
        }

        private void OnDisable()
        {
            StopCoroutine(_currentMovementRoutine);
        }

        private IEnumerator Move()
        {
            var position = transform.position;
            while (true)
            {
                position.x -= _speed * DefaultSpeedMultiplier;
                transform.position = position;
                if (transform.position.x < OffScreenEdge.x)
                {
                    gameObject.SetActive(false);
                }

                yield return null;
            }
        }
    }
}