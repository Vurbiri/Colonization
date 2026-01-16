using UnityEngine;
using Vurbiri.Colonization.Storage;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class Settings
	{
		[SerializeField] private Profile _profile = new();
		[SerializeField] private AudioMixer<MixerId> _mixer = new();

		public AudioMixer<MixerId> Volumes { [Impl(256)] get => _mixer; }
		public Profile Profile { [Impl(256)] get => _profile; }

#if YSDK
        public void Init(Vurbiri.Yandex.YandexSDK ysdk, ProjectStorage storage)
		{
            _profile.Init(ysdk);

            storage.SetAndBindAudioMixer(_mixer);
			storage.SetAndBindProfile(_profile);

			Cancel();
		}
#else
		public void Init(ProjectStorage storage)
        {
            _profile.Init();

            storage.SetAndBindAudioMixer(_mixer);
            storage.SetAndBindProfile(_profile);

            Cancel();
        }
#endif

        [Impl(256)] public void Apply()
		{
			_profile.Apply();
			_mixer.Apply();
		}

		[Impl(256)] public void Cancel()
		{
			_profile.Cancel();
			_mixer.Cancel();
		}

#if UNITY_EDITOR
		public void OnValidate()
		{
			_mixer.OnValidate();
		}
#endif
	}
}
