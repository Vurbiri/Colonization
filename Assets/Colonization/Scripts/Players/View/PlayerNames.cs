using System;
using UnityEngine;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.Colonization.Storage;
using Vurbiri.International;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class PlayerNames : IReactive<PlayerNames>, IEquatable<string[]>, IDisposable
	{
        [SerializeField, Key(LangFiles.Main)] private string[] _nameKeys;
        private string[] _customNames;

        private readonly string[] _names = new string[PlayerId.Count];
        private readonly Subscription<PlayerNames> _eventThisChanged = new();
        private Unsubscription _unsubscription;

        public string this[Id<PlayerId> id] => _names[id.Value];
        public string this[int index] => _names[index];

        public string[] CustomNames
        {
            get => _customNames;
            set
            {
                if (!Equals(value))
                {
                    _customNames = value;
                    SetNames(Localization.Instance);
                }
            }
        }

        public PlayerNames Init(ProjectStorage storage)
        {
            bool notLoad = !storage.TryLoadPlayerNames(out _customNames);
            if (notLoad) 
                _customNames = new string[PlayerId.Count];

            _unsubscription = Localization.Instance.Subscribe(SetNames);
            storage.BindPlayerNames(this);

            return this;
        }

        public string GetDefaultName(int index) => Localization.Instance.GetText(LangFiles.Main, _nameKeys[index]);

        public Unsubscription Subscribe(Action<PlayerNames> action, bool instantGetValue = true) => _eventThisChanged.Add(action, instantGetValue, this);

        public bool Equals(string[] customNames)
        {
            for (int i = 0; i < PlayerId.Count; i++)
                if (_customNames[i] != customNames[i])
                    return false;

            return true;
        }
        public void Dispose()
        {
            _unsubscription?.Dispose();
        }

        private void SetNames(Localization localization)
        {
            string temp;
            for (int index = 0; index < PlayerId.Count; index++)
            {
                if (string.IsNullOrEmpty(temp = _customNames[index]))
                {
                    if (index != PlayerId.Person || !ProjectContainer.YSDK.IsLogOn || string.IsNullOrEmpty(temp = ProjectContainer.YSDK.PlayerName))
                        temp = localization.GetText(LangFiles.Main, _nameKeys[index]);
                }
                _names[index] = temp;
            }

            _eventThisChanged.Invoke(this);
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            _nameKeys ??= PlayerId.PositiveNames_Ed;
        }
#endif
    }
}
