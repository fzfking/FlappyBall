using System;
using System.Collections;
using UnityEngine;

namespace Sources.Codebase.GameEntities
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class FlappyBall : MonoBehaviour
    {
        private const float SecondsBetweenIncrease = 15f;
        private const float SpeedIncreasingFactor = 0.25f;
        private const float VerticalSpeedDefault = 1f;
        private const float VerticalSpeedMultiplier = 0.01f;
        public event Action OnCollidedWithObstacle;
        private float _verticalSpeed = VerticalSpeedDefault;
        private Transform _cachedTransform;
        private Coroutine _currentMovementRoutine;
        private Coroutine _currentSpeedIncreasingRoutine;
        private bool _isDowning = true;

        public void GetUp()
        {
            _isDowning = false;
        }

        public void GetDown()
        {
            _isDowning = true;
        }

        public void Reset()
        {
            transform.position = Vector3.zero;
            _verticalSpeed = VerticalSpeedDefault;
            _isDowning = true;
        }

        private void Awake()
        {
            _cachedTransform = transform;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.transform.parent.TryGetComponent(out Line line))
            {
                OnCollidedWithObstacle?.Invoke();
                return;
            }

            if (collision.collider.TryGetComponent(out EnemyBall ball))
            {
                OnCollidedWithObstacle?.Invoke();
                return;
            }
        }

        private void OnEnable()
        {
            _currentMovementRoutine = StartCoroutine(Move());
            _currentSpeedIncreasingRoutine = StartCoroutine(SpeedIncrease());
        }

        private void MoveDown()
        {
            _cachedTransform.position = _cachedTransform.position + Vector3.down * (VerticalSpeedMultiplier * _verticalSpeed);
        }

        private void MoveUp()
        {
            _cachedTransform.position = _cachedTransform.position + Vector3.up * (VerticalSpeedMultiplier * _verticalSpeed);
        }

        private IEnumerator Move()
        {
            while (true)
            {
                if (_isDowning)
                {
                    MoveDown();
                }
                else
                {
                    MoveUp();
                }

                yield return null;
            }
        }

        private IEnumerator SpeedIncrease()
        {
            while (true)
            {
                yield return new WaitForSeconds(SecondsBetweenIncrease);
                _verticalSpeed += SpeedIncreasingFactor;
            }
        }

        private void OnDisable()
        {
            StopCoroutine(_currentMovementRoutine);
            StopCoroutine(_currentSpeedIncreasingRoutine);
        }
    }
}