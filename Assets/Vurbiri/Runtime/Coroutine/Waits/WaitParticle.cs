using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable]
	sealed public class WaitParticle : AWait
	{
		[SerializeField] private ParticleSystem _particleSystem;

		public ParticleSystem ParticleSystem { [Impl(256)] get => _particleSystem; }
		public override bool IsWait { [Impl(256)] get => _particleSystem.isPlaying; }

		public WaitParticle(ParticleSystem particleSystem)
		{
			_particleSystem = particleSystem;
		}

		public override bool MoveNext() => _particleSystem.isPlaying;

		[Impl(256)] public WaitParticle Play()
		{
            Clear(); _particleSystem.Play();
			return this;
		}
		[Impl(256)] public void Stop() => _particleSystem.Stop();

		[Impl(256)] public void Clear()
		{
			if (_particleSystem.isPlaying)
			{
				_particleSystem.Stop();
				_particleSystem.Clear();
			}
		}
	}
}
