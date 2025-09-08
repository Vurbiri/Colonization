using System.Collections;
using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public class WaitParticle : IEnumerator
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public object Current => null;
        public ParticleSystem ParticleSystem => _particleSystem;
        public bool IsWait => _particleSystem.isPlaying;

        public WaitParticle(ParticleSystem particleSystem)
        {
            _particleSystem = particleSystem;
        }

        public bool MoveNext() => _particleSystem.isPlaying;

        public IEnumerator Play()
        {
            Reset(); _particleSystem.Play();
            return this;
        }
        public void Stop() => _particleSystem.Stop();

        public void Reset()
        {
            if (_particleSystem.isPlaying)
            {
                _particleSystem.Stop();
                _particleSystem.Clear();
            }
        }
    }
}
