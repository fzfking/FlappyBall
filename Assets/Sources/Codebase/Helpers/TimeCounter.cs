using System;
using System.Collections;
using UnityEngine;

namespace Sources.Codebase.Helpers
{
    public class TimeCounter
    {
        public event Action<TimeSpan> OnTimePassed; 
        private float _timePassed;
        private Coroutine _currentStartedCounter;
        private readonly ICoroutineRunner _coroutineRunner;
        public TimeCounter(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Start()
        {
            _timePassed = 0f;
            _currentStartedCounter = _coroutineRunner.StartCoroutine(Count());
        }

        public TimeSpan Stop()
        {
            _coroutineRunner.StopCoroutine(_currentStartedCounter);
            return TimeSpan.FromSeconds(_timePassed);
        }

        private IEnumerator Count()
        {
            while (true)
            {
                yield return null;
                _timePassed += Time.deltaTime;
                OnTimePassed?.Invoke(TimeSpan.FromSeconds(_timePassed));
            }
        }
    }
}