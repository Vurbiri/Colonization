using System;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.Reactive;
using Vurbiri.Yandex;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public partial class Profile : IReactive<Profile>
    {
        [SerializeField] private SystemLanguage _idLang = SystemLanguage.Russian;
        [SerializeField] private int _quality = 2;

        private readonly VAction<Profile> _eventChanged = new();

        public SystemLanguage Language { [Impl(256)] get => _idLang; [Impl(256)] set => Localization.Instance.CurrentId = value; }
        public int Quality { [Impl(256)] get => _quality; [Impl(256)] set => QualitySettings.SetQualityLevel(value); }

        public Localization Localization { [Impl(256)] get => Localization.Instance; }
        public static int MaxQuality { [Impl(256)] get => QualitySettings.count - 1; }

        [Impl(256)] public void Init(YandexSDK ysdk)
        {
            if (ysdk.IsInitialize)
                _idLang = Localization.Instance.IdFromCode(ysdk.Lang);

            //_idLang = ysdk.IsInitialize ? _localization.IdFromCode(ysdk.Lang) : Application.systemLanguage;
        }

        [Impl(256)] public Subscription Subscribe(Action<Profile> action, bool instantGetValue = true) => _eventChanged.Add(action, this, instantGetValue);

        public void Apply()
        {
            var localization = Localization.Instance;
            bool changed = _idLang != localization.CurrentId;
            _idLang = localization.CurrentId;

            int level = QualitySettings.GetQualityLevel();
            changed |= _quality != level;
            _quality = level;

            if (changed) 
                _eventChanged.Invoke(this);
        }

        [Impl(256)] public void Cancel()
        {
            Localization.Instance.CurrentId = _idLang;
            QualitySettings.SetQualityLevel(_quality);
        }
    }
}
