using UnityEngine;

namespace Vurbiri
{
	public class WaitAudio
	{
        private readonly AudioSource _audioSource;

        public object Current => null;

        public WaitAudio(AudioSource audioSource) => _audioSource = audioSource;

        public bool MoveNext() => _audioSource.isPlaying;

        public void Play() => _audioSource.Play();
        public void PlayOneShot(AudioClip audioClip) => _audioSource.PlayOneShot(audioClip);
        public void Stop() => _audioSource.Stop();

        public void Reset()
        {
            _audioSource.Stop();
            _audioSource.Play();
        }
    }
}
