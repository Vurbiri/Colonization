using System.Collections;
using UnityEngine;

namespace Vurbiri
{
    public class WaitParticle : IEnumerator
    {
        private readonly ParticleSystem _particleSystem;

        public object Current => null;

        public WaitParticle(ParticleSystem particleSystem)
        {
            _particleSystem = particleSystem;
        }

        public bool MoveNext() => _particleSystem.isPlaying;

        public void Play() => _particleSystem.Play();
        public void Stop() => _particleSystem.Stop();

        public void Reset()
        {
            _particleSystem.Stop();
            _particleSystem.Clear();
            _particleSystem.Play();
        }
    }
}
