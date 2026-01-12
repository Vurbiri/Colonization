using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	[System.Serializable]
	sealed public class WaitAudio : AWait
	{
		[SerializeField] private AudioSource _audioSource;

		public AudioSource AudioSource { [Impl(256)] get => _audioSource; }
		public override bool IsWait { [Impl(256)] get => _audioSource.isPlaying; }

		[Impl(256)] public WaitAudio(AudioSource audioSource) => _audioSource = audioSource;

		public override bool MoveNext() => _audioSource.isPlaying;

		[Impl(256)] public WaitAudio Play()
		{
            _audioSource.Stop(); 
			_audioSource.Play();
			return this;
		}
		[Impl(256)] public WaitAudio PlayOneShot(AudioClip audioClip)
		{
            _audioSource.Stop(); 
			_audioSource.PlayOneShot(audioClip);
			return this;
		}
		[Impl(256)] public void Stop() => _audioSource.Stop();
	}
}
