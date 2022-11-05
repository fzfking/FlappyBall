using System;
using Sources.Codebase.Helpers;
using UnityEngine;

namespace Sources.Codebase
{
    [RequireComponent(typeof(Animation))]
    public class Line : MonoBehaviour
    {
        [SerializeField] private Transform LineElement;
        public Transform Transform => LineElement;
        private int _speed;

        public void SetSpeed(Difficulty difficulty)
        {
            var animationComponent = GetComponent<Animation>();
            
            switch (difficulty)
            {
                case Difficulty.Easy:
                    foreach (AnimationState state in animationComponent)
                    {
                        state.speed = 1f;
                    }
                    break;
                case Difficulty.Medium:
                    foreach (AnimationState state in animationComponent)
                    {
                        state.speed = 3f;
                    }
                    break;
                case Difficulty.Hard:
                    foreach (AnimationState state in animationComponent)
                    {
                        state.speed = 8f;
                    }
                    break;
            }
        }
    }
}