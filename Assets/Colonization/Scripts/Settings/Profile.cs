//Assets\Colonization\Scripts\Settings\Profile.cs
using System;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public partial class Profile : IReactive<Profile>
    {
        [SerializeField] private SystemLanguage _idLang = SystemLanguage.Russian;
        [SerializeField] private int _quality = 2;

        private readonly Signer<Profile> _signer = new();
        private Localization _localization;

        public SystemLanguage Language { get => _idLang; set => _localization.SwitchLanguage(value); }
        public int Quality { get => _quality; set => QualitySettings.SetQualityLevel(value); }

        public Localization Localization => _localization;
        public int QualityCount => QualitySettings.count;

        public void Init(YandexSDK ysdk)
        {
            _localization = Localization.Instance;
            if (ysdk.IsInitialize)
                _idLang = _localization.IdFromCode(ysdk.Lang);

            //_idLang = ysdk.IsInitialize ? _localization.IdFromCode(ysdk.Lang) : Application.systemLanguage;
        }

        public Unsubscriber Subscribe(Action<Profile> action, bool instantGetValue = true) => _signer.Add(action, instantGetValue, this);

        public void Apply()
        {
            bool changed = _idLang != _localization.CurrentId;
            _idLang = _localization.CurrentId;

            int level = QualitySettings.GetQualityLevel();
            changed |= _quality != level;
            _quality = level;

            if (changed) _signer.Invoke(this);
        }

        public void Cancel()
        {
            _localization.SwitchLanguage(_idLang);
            QualitySettings.SetQualityLevel(_quality);
        }
    }
}
