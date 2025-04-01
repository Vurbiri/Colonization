//Assets\Colonization\Scripts\Settings\Profile.cs
using System;
using UnityEngine;
using Vurbiri.Reactive;
using Vurbiri.TextLocalization;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public partial class Profile : IReactive<Profile>
    {
        [SerializeField] private int _idLang = 0;
        [SerializeField] private int _quality = 2;

        private readonly Subscriber<Profile> _subscriber = new();
        private Localization _localization;

        public int Language { get => _idLang; set => _localization.SwitchLanguage(value); }
        public int Quality { get => _quality; set => QualitySettings.SetQualityLevel(value); }

        public int QualityCount => QualitySettings.count;

        public void Init(YandexSDK ysdk)
        {
            _localization = Localization.Instance;
            if (ysdk.IsInitialize && _localization.TryIdFromCode(ysdk.Lang, out int id))
                _idLang = id;
        }

        public Unsubscriber Subscribe(Action<Profile> action, bool sendCallback = true)=> _subscriber.Add(action, sendCallback, this);

        public void Apply()
        {
            bool changed = false; int value;

            value = _localization.CurrentId;
            changed |= _idLang != value;
            _idLang = value;

            value = QualitySettings.GetQualityLevel();
            changed |= _quality != value;
            _quality = value;

            _idLang = _localization.CurrentId;
            _quality = QualitySettings.GetQualityLevel();

            if (changed) _subscriber.Invoke(this);
        }

        public void Cancel()
        {
            _localization.SwitchLanguage(_idLang);
            QualitySettings.SetQualityLevel(_quality);
        }
    }
}
