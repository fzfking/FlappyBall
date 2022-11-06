using System;
using UnityEngine;

namespace Sources.Codebase.GameEntities
{
    public class FireLauncher : MonoBehaviour
    {
        [SerializeField] private ParticleSystem ParticleSystem;
        [SerializeField] private SpriteRenderer FireSprite;
        private ParticleSystem.MainModule _mainParticles;

        private void Awake()
        {
            if (ParticleSystem == null)
            {
                Debug.Log("Particle system not installed on FireLauncher prefab");
                return;
            }

            if (FireSprite == null)
            {
                Debug.Log("Fire sprite not installed on FireLauncher prefab");
                return;
            }

            _mainParticles = ParticleSystem.main;
        }

        public void Enable()
        {
            FireSprite.gameObject.SetActive(true);
            _mainParticles.loop = true;
            ParticleSystem.Play();
        }

        public void Disable()
        {
            FireSprite.gameObject.SetActive(false);
            _mainParticles.loop = false;
            //ParticleSystem.Stop();

        }
    }
}