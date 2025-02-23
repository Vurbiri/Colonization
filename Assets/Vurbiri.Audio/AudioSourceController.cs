//Assets\Vurbiri.Audio\AudioSourceController.cs
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Audio
{
    [RequireComponent (typeof(AudioSource))]
	public class AudioSourceController : MonoBehaviour
	{
		[SerializeField] private Id<AudioTypeId> _audioType;
		[Space]
        [SerializeField] private AudioSource _audio;

		private IUnsubscriber _unsubscriber;
		
		private void Start() => _unsubscriber = AudioController.Instance.Subscribe(_audioType, v => _audio.volume = v);
        private void OnDestroy() => _unsubscriber.Unsubscribe();

#if UNITY_EDITOR
        private void OnValidate()
        {
			if(_audio == null)
				_audio = GetComponent<AudioSource>();
        }
#endif
	}
}
