using System;
using System.Collections;
using UnityEngine;

namespace Sources.Codebase
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class FlappyBall : MonoBehaviour
    {
        public event Action OnCollidedWithLine;
        private float _verticalSpeed = 1f;
        private Transform _cachedTransform;
        private Coroutine _currentMovementRoutine;
        private Coroutine _currentSpeedIncreasingRoutine;
        private CircleCollider2D _collider;
        private bool _isDowning = true;

        public void GetUp()
        {
            _isDowning = false;
        }

        public void GetDown()
        {
            _isDowning = true;
        }

        private void Awake()
        {
            _cachedTransform = transform;
            _collider = GetComponent<CircleCollider2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.transform.parent.TryGetComponent(out Line line))
            {
                OnCollidedWithLine?.Invoke();
            }
        }

        private void OnEnable()
        {
            _currentMovementRoutine = StartCoroutine(Move());
            _currentSpeedIncreasingRoutine = StartCoroutine(SpeedIncrease());
        }

        private void MoveDown()
        {
            _cachedTransform.position = _cachedTransform.position + Vector3.down * (0.01f * _verticalSpeed);
        }

        private void MoveUp()
        {
            _cachedTransform.position = _cachedTransform.position + Vector3.up * (0.01f * _verticalSpeed);
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
                yield return new WaitForSeconds(15f);
                _verticalSpeed += 0.25f;
            }
        }

        private void OnDisable()
        {
            StopCoroutine(_currentMovementRoutine);
            StopCoroutine(_currentSpeedIncreasingRoutine);
        }
    }
}