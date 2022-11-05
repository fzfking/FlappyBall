using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.Codebase.Helpers
{
    public class EnemyBallsPool
    {
        private const int DefaultCapacity = 3;
        private List<EnemyBall> _balls = new List<EnemyBall>(DefaultCapacity);
        private readonly EnemyBall _prefab;
        private readonly Transform _parent;
        public EnemyBallsPool(EnemyBall prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = new GameObject("EnemyBalls").transform;
            _parent.parent = parent;
            AppendList();
        }

        public EnemyBall Spawn()
        {
            var firstDisabledBall = _balls.FirstOrDefault(x => x.gameObject.activeSelf == false);
            if (firstDisabledBall == null)
            {
                AppendList();
                return Spawn();
            }

            return firstDisabledBall;
        }

        public void Reset()
        {
            foreach (var ball in _balls.Where(x=> x.gameObject.activeSelf))
            {
                ball.gameObject.SetActive(false);
            }
        }
        
        private void AppendList()
        {
            for (int i = 0; i < DefaultCapacity; i++)
            {
                var ball = Object.Instantiate(_prefab, _parent);
                ball.gameObject.SetActive(false);
                _balls.Add(ball);
            }
            
        }
    }
}