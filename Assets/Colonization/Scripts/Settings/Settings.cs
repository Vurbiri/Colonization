//Assets\Colonization\Scripts\Settings\Settings.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Audio;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public partial class Settings : AReactive<IReadOnlyList<int>>
    {
        [SerializeField] private Profile _profile;
        
        private readonly AudioController _audio = AudioController.Instance;
        private Localization _localization;

        public AudioController Audio { get => _audio; }
        public int Language { get => _localization.CurrentId; set => _localization.SwitchLanguage(value); }
        public int Quality { get => QualitySettings.GetQualityLevel(); set => QualitySettings.SetQualityLevel(value); }
        
        public int QualityCount => QualitySettings.count;

        public override IReadOnlyList<int> Value { get => _profile.ToArray(); protected set { } }

        public void Init(YandexSDK ysdk, int[] data)
        {
            _localization = Localization.Instance;

            if (ysdk.IsInitialize && _localization.TryIdFromCode(ysdk.Lang, out int id))
                _profile.idLang = id;
            _profile.FromArray(data);

            Cancel();
        }

        public void Apply()
        {
            for (int i = 0; i < AudioTypeId.Count; i++)
                _profile.volumes[i] = _audio[i];
            _profile.volumeGeneric = _audio.Volume;
            _profile.mute = _audio.Mute;

            _profile.idLang = _localization.CurrentId;
            _profile.quality = QualitySettings.GetQualityLevel();

            _subscriber.Invoke(_profile.ToArray());
        }

        public void Cancel()
        {
            for (int i = 0; i < AudioTypeId.Count; i++)
                _audio[i] = _profile.volumes[i];
            _audio.Volume = _profile.volumeGeneric;
            _audio.Mute = _profile.mute;

            _localization.SwitchLanguage(_profile.idLang);
            QualitySettings.SetQualityLevel(_profile.quality);
        }
    }
}
