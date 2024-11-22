//Assets\Colonization\Scripts\Data\SettingsData.cs
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Localization;

namespace Vurbiri.Colonization.Data
{
    using static JSON_KEYS;

    public class SettingsData
    {
        private readonly float _audioMinValue = 0.0f;
        private readonly float _audioMaxValue = 1.0f;
        //private float _audioMinValue = 0.01f;
        //private float _audioMaxValue = 1.5845f;

        private Profile _profileCurrent = null;
        private readonly Profile _profileDefault;

        public float MinValue => _audioMinValue;
        public float MaxValue => _audioMaxValue;
        public bool IsFirstStart { get; set; } = true;

        private readonly IStorageService _storage;
        private readonly YandexSDK _ysdk;
        private readonly Language _localization;
        private readonly Dictionary<AudioType, IVolume> _volumes = new(Enum<AudioType>.Count);

        public SettingsData(IReadOnlyDIContainer container, Profile defaultProfile)
        {
            _profileDefault = defaultProfile;

            _ysdk = container.Get<YandexSDK>();
            _storage = container.Get<IStorageService>();
            _localization = container.Get<Language>();

            _volumes[AudioType.Music] = MusicSingleton.Instance;
            _volumes[AudioType.SFX] = SoundSingleton.Instance;

            SetDefaultProfile();
            Load();
            Apply();
        }

        public void SetVolume(AudioType type, float volume) => _volumes[type].Volume = volume;

        public float GetVolume(AudioType type) => _profileCurrent.volumes[(int)type];

        public IEnumerator Save_Coroutine(Action<bool> callback = null)
        {
            _profileCurrent.idLang = _localization.CurrentId;
            _profileCurrent.quality = QualitySettings.GetQualityLevel();
            foreach (var type in Enum<AudioType>.Values)
                _profileCurrent.volumes[(int)type] = _volumes[type].Volume;

            return _storage.Save_Coroutine(SAVE_KEYS.SETTINGS, _profileCurrent, true, callback);
        }

        public void Cancel()
        {
            if (!Load())
                SetDefaultProfile();

            Apply();
        }

        private bool Load()
        {
            if (_storage.TryGet(SAVE_KEYS.PROJECT, out Profile data))
            {
                _profileCurrent.Copy(data);
                return true;
            }

            return false;
        }

        private void SetDefaultProfile()
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
        public class Profile
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
