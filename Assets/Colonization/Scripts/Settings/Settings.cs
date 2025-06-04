using System;
using UnityEngine;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class Settings
    {
        [SerializeField] private Profile _profile = new();
        [SerializeField] private AudioMixer<MixerId> _mixer = new();

        private ProjectStorage _storage;

        public AudioMixer<MixerId> Volumes => _mixer;
        public Profile Profile => _profile;

        public void Init(YandexSDK ysdk, ProjectStorage storage)
        {
            _storage = storage;
            _profile.Init(ysdk);

            storage.SetAndBindAudioMixer(_mixer);
            storage.SetAndBindProfile(_profile);

            Cancel();
        }

        public void Apply()
        {
            _profile.Apply();
            _mixer.Apply();
        }

        public void ApplyAndSave(Action<bool> callback = null)
        {
            _profile.Apply();
            _mixer.Apply();

            _storage.Save(callback);
        }

        public void Cancel()
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
