using System;
using UnityEngine;
using Vurbiri.Colonization.Storage;
using Vurbiri.International;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class PlayerNames : IReactive<string[]>, IEquatable<string[]>, IDisposable
	{
        [SerializeField, Key(Files.Main)] private string[] _nameKeys;
        private string[] _customNames;
        private YandexSDK _ysdk;
        private Localization _localization;

        private readonly string[] _names = new string[PlayerId.Count];
        private readonly Subscription<string[]> _eventThisChanged = new();
        private Unsubscription _unsubscription;

        public string this[Id<PlayerId> id] => _names[id.Value];
        public string this[int index] => _names[index];

        public string[] CustomNames
        {
            get => _customNames;
            set
            {
                if (!this.Equals(value))
                {
                    _customNames = value;
                    SetNames();
                    _eventThisChanged.Invoke(_customNames);
                }
            }
        }

        public PlayerNames Init(ProjectStorage storage, YandexSDK ysdk)
        {
            _localization = Localization.Instance;
            _ysdk = ysdk;

            bool notLoad = !storage.TryLoadPlayerNames(out _customNames);
            if (notLoad) _customNames = new string[PlayerId.Count];
            storage.BindPlayerNames(this);

            _unsubscription = _localization.Subscribe(_ => SetNames());

            return this;
        }

        public string GetDefaultName(int index) => _localization.GetText(Files.Main, _nameKeys[index]);

        public Unsubscription Subscribe(Action<string[]> action, bool instantGetValue = true) => _eventThisChanged.Add(action, instantGetValue, _customNames);

        public bool Equals(string[] customNames)
        {
            for (int i = 0; i < PlayerId.Count; i++)
                if (_customNames[i] != customNames[i])
                    return false;

            return true;
        }
        public void Dispose()
        {
            _unsubscription?.Unsubscribe();
        }

        private void SetNames()
        {
            for (int i = 0; i < PlayerId.Count; i++)
                _names[i] = GetName(i);

            // Local 
            string GetName(int index)
            {
                if (!string.IsNullOrEmpty(_customNames[index]))
                    return _customNames[index];

                if (index == PlayerId.Player & _ysdk.IsLogOn)
                {
                    string name = _ysdk.PlayerName;
                    if (!string.IsNullOrEmpty(name))
                        return name;
                }

                return _localization.GetText(Files.Main, _nameKeys[index]);
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
        {

            _nameKeys ??= PlayerId.PositiveNames;
        }
#endif
    }
}
