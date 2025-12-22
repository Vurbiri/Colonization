using System;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Storage;
using Vurbiri.International;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	[Serializable]
	public class PlayerNames : IDisposable
    {
		[SerializeField, Key(LangFiles.Main)] private string[] _nameKeys;
		
        private readonly string[] _defaults = new string[PlayerId.HumansCount];
        private readonly Array<string> _names = new(PlayerId.Count);
        private readonly Array<string> _customs = new(PlayerId.HumansCount);

        private readonly VAction<ReadOnlyArray<string>> _namesChange = new();
        private readonly VAction<ReadOnlyArray<string>> _customsChange = new();

        public string this[Id<PlayerId> id] { [Impl(256)] get => _names[id.Value]; }
		public string this[int index] { [Impl(256)] get => _names[index]; }

        public PlayerNames Init(ProjectStorage storage)
		{
            if (storage.TryLoadPlayerNames(out string[] customs))
                _customs.Import(customs);

            Localization.Subscribe(SetNames);
			storage.BindPlayerNames(_customsChange);

			return this;
		}

		[Impl(256)] public Subscription Subscribe(Action<ReadOnlyArray<string>> action, bool instantGetValue = true) => _namesChange.Add(action, _names, instantGetValue);
        [Impl(256)] public void Unsubscribe(Action<ReadOnlyArray<string>> action) => _namesChange.Remove(action);

        [Impl(256)] public string GetDefault(int id) => _defaults[id];

        public void TrySetCustomNames(string[] names)
        {
            bool change = false; string name;
            for (int i = 0; i < PlayerId.HumansCount; ++i)
            {
                name = names[i].Trim();
                if(!string.IsNullOrEmpty(name))
                {
                    if(_defaults[i] == name)
                    {
                        if (!string.IsNullOrEmpty(_customs[i]))
                        {
                            _customs[i] = string.Empty;
                            _names[i] = _defaults[i];

                            change = true;
                        }
                    }
                    else
                    {
                        if (_customs[i] != name)
                        {
                            _customs[i] = name;
                            _names[i] = name;

                            change = true;
                        }
                    }
                }
            }

            if (change)
            {
                _customsChange.Invoke(_customs);
                _namesChange.Invoke(_names);
            }
        }

        public void Dispose()
        {
            _namesChange.Clear();
            Localization.Unsubscribe(SetNames);
        }

        [Impl(256)] public static implicit operator ReadOnlyArray<string>(PlayerNames playerNames) => playerNames._names;

        private void SetNames(Localization localization)
		{
			int index = 0;
			if (ProjectContainer.YSDK.TryGetPlayerName(out string name))
			{
				SetName(PlayerId.Person, name, _customs[PlayerId.Person]);
                index = PlayerId.AI_01;
			}

			for (; index < PlayerId.HumansCount; ++index)
                SetName(index, localization.GetText(LangFiles.Main, _nameKeys[index], true), _customs[index]);

			name = localization.GetText(LangFiles.Main, _nameKeys[PlayerId.Satan], true);
            _names[PlayerId.Satan] = name;

            _namesChange.Invoke(_names);

            // ------------ Local -----------------
            [Impl(256)] void SetName(int id, string name, string custom)
            {
                _defaults[id] = name;
                if (!string.IsNullOrEmpty(custom))
                    name = custom;
                _names[id] = name;
            }
        }

#if UNITY_EDITOR
        public void OnValidate()
		{
			_nameKeys ??= PlayerId.Names_Ed;
		}
#endif
	}
}
