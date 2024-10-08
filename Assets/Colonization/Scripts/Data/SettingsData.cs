using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Localization;
using static Vurbiri.Colonization.JSON_KEYS;

namespace Vurbiri.Colonization
{
    public class SettingsData : ASingleton<SettingsData>
    {
        [Space]
        [SerializeField] private string _keySave = "std";
        [Space]
        [SerializeField] private Profile _profileDefault = new();
        [Space]
        [SerializeField] private float _audioMinValue = 0.0f;
        [SerializeField] private float _audioMaxValue = 1.0f;
        //[SerializeField] private float _audioMinValue = 0.01f;
        //[SerializeField] private float _audioMaxValue = 1.5845f;

        private Profile _profileCurrent = null;

        public float MinValue => _audioMinValue;
        public float MaxValue => _audioMaxValue;
        public bool IsFirstStart { get; set; } = true;

        private YandexSDK _ysdk;
        private Language _localization;
        private readonly Dictionary<AudioType, IVolume> _volumes = new(Enum<AudioType>.Count);

        protected override void Awake()
        {
            base.Awake();

            _ysdk = YandexSDK.Instance;
            _localization = Language.Instance;

            _volumes[AudioType.Music] = MusicSingleton.Instance;
            _volumes[AudioType.SFX] = SoundSingleton.Instance;
        }

        public bool Init(bool isLoad)
        {
            DefaultProfile();

            bool result = isLoad && Load();
            Apply();

            return result;
        }

        public void SetVolume(AudioType type, float volume) => _volumes[type].Volume = volume;

        public float GetVolume(AudioType type) => _profileCurrent.volumes[(int)type];

        public void Save(Action<bool> callback = null)
        {
            _profileCurrent.idLang = _localization.CurrentId;
            _profileCurrent.quality = QualitySettings.GetQualityLevel();
            foreach (var type in Enum<AudioType>.Values)
                _profileCurrent.volumes[(int)type] = _volumes[type].Volume;

            StartCoroutine(Storage.Save_Coroutine(_keySave, _profileCurrent, true, callback));
        }
        private bool Load()
        {
            Return<Profile> data = Storage.Load<Profile>(_keySave);
            if (data.Result)
                _profileCurrent.Copy(data.Value);

            return data.Result;
        }

        public void Cancel()
        {
            if (!Load())
                DefaultProfile();

            Apply();
        }

        private void DefaultProfile()
        {
            _profileCurrent = _profileDefault.Clone();

            if (_ysdk.IsInitialize)
                if (_localization.TryIdFromCode(_ysdk.Lang, out int id))
                    _profileCurrent.idLang = id;
        }

        private void Apply()
        {
            _localization.SwitchLanguage(_profileCurrent.idLang);
            QualitySettings.SetQualityLevel(_profileCurrent.quality);
            foreach (var type in Enum<AudioType>.Values)
                _volumes[type].Volume = _profileCurrent.volumes[(int)type];
        }

        #region Nested: Profile
        //*******************************************************
        [System.Serializable]
        [JsonObject(MemberSerialization.OptIn)]
        private class Profile
        {
            [JsonProperty(S_LANG)]
            public int idLang = 1;
            [JsonProperty(S_QUALITY)]
            public int quality = 2;
            [JsonProperty(S_VOLUMES)]
            public float[] volumes = { 0.6f, 0.6f };

            [JsonConstructor]
            public Profile(int idLang, int quality, float[] volumes)
            {
                this.idLang = idLang;
                this.quality = quality;
                volumes.CopyTo(this.volumes, 0);
            }

            public Profile() { }

            public void Copy(Profile profile)
            {
                if (profile == null) return;

                idLang = profile.idLang;
                quality = profile.quality;
                profile.volumes.CopyTo(volumes, 0);
            }

            public Profile Clone() => new(idLang, quality, volumes);

        }
        #endregion
    }
}
