using System.Collections;
using UnityEngine;

namespace Sources.Codebase.Helpers
{
    public interface ICoroutineRunner
    {
        public Coroutine StartCoroutine(IEnumerator enumerator);
        public void StopCoroutine(Coroutine coroutine);
    }
}