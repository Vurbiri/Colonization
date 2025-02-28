//Assets\Colonization\Scripts\Settings\Settings.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public partial class Settings : IReactive<IReadOnlyList<int>, IReadOnlyList<float>>
    {
        [SerializeField] private Profile _profile;
        [SerializeField] private AudioMixer<MixerId> _mixer;

        private Localization _localization;
        private Subscriber<IReadOnlyList<int>, IReadOnlyList<float>> _subscriber;

        public IVolume<MixerId> Volume { get => _mixer; }
        public int Language { get => _localization.CurrentId; set => _localization.SwitchLanguage(value); }
        public int Quality { get => QualitySettings.GetQualityLevel(); set => QualitySettings.SetQualityLevel(value); }
        
        public int QualityCount => QualitySettings.count;

        public void Init(YandexSDK ysdk, ProjectSaveData saveData)
        {
            _localization = Localization.Instance;
            if (ysdk.IsInitialize && _localization.TryIdFromCode(ysdk.Lang, out int id))
                _profile.idLang = id;

            bool loaded;
            if(loaded = saveData.TryGetSettingsData(out int[] profile, out float[] volumes))
            {
                _profile.FromArray(profile); _mixer.FromArray(volumes);
            }
            saveData.SettingsBind(this, !loaded);

            Cancel();
        }

        public void Apply()
        {
            _profile.idLang = _localization.CurrentId;
            _profile.quality = QualitySettings.GetQualityLevel();
            _mixer.Apply();

            _subscriber.Invoke(_profile.ToArray(), _mixer.ToArray());
        }

        public void Cancel()
        {
            _localization.SwitchLanguage(_profile.idLang);
            QualitySettings.SetQualityLevel(_profile.quality);
            _mixer.Cancel();
        }

        public Unsubscriber Subscribe(Action<IReadOnlyList<int>, IReadOnlyList<float>> action, bool calling = true)
        {
            if (calling) action(_profile.ToArray(), _mixer.ToArray());
            return _subscriber.Add(action);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            _mixer.OnValidate();
        }
#endif
    }
}
