using System;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Storage;
using Vurbiri.International;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	[System.Serializable]
	public class PlayerNames : IDisposable
	{
		[SerializeField, Key(LangFiles.Main)] private string[] _nameKeys;
		private Array<string> _customNames;

		private readonly string[] _defaults = new string[PlayerId.HumansCount];
        private readonly string[] _names = new string[PlayerId.Count];
        private readonly VAction<ReadOnlyArray<string>> _namesChange = new();
		private readonly VAction<string>[] _nameChanges = new VAction<string>[PlayerId.Count];

		public string this[Id<PlayerId> id] { [Impl(256)] get => _names[id.Value]; }
		public string this[int index] { [Impl(256)] get => _names[index]; }

		public PlayerNames()
		{
			for (int i = 0; i < PlayerId.Count; ++i)
				_nameChanges[i] = new();
		}

		public PlayerNames Init(ProjectStorage storage)
		{
            if (!storage.TryLoadPlayerNames(out string[] customNames))
                customNames = new string[PlayerId.HumansCount];

            _customNames = customNames;

            Localization.Subscribe(SetNames);
			storage.BindPlayerNames(this);

			return this;
		}

		public string GetDefaultName(int index) => Localization.Instance.GetText(LangFiles.Main, _nameKeys[index]);

		[Impl(256)] public Subscription Subscribe(Action<ReadOnlyArray<string>> action, bool instantGetValue = true) => _namesChange.Add(action, _names, instantGetValue);

		[Impl(256)] public Subscription Subscribe(int playerId, Action<string> action, bool instant = true) => _nameChanges[playerId].Add(action, _names[playerId], instant);
		[Impl(256)] public void Unsubscribe(int playerId, Action<string> action) => _nameChanges[playerId].Remove(action);

        public void ResetCustomName(int id)
        {
            if(!string.IsNullOrWhiteSpace(_customNames[id]))
			{
                _customNames[id] = null;
                SetName(id, _defaults[id]);
            }
        }

        public string SetCustomName(int id, string name)
		{
            name = name.Trim();

            if (string.IsNullOrWhiteSpace(name))
			{
				ResetCustomName(id);
            }
			else if (_customNames[id] != name)
			{
				_customNames[id] = name;
				SetName(id, name);
            }
			return _names[id];
        }

        public void Dispose()
        {
            _namesChange.Clear();
            Localization.Unsubscribe(SetNames);
        }

        private void SetNames(Localization localization)
		{
			int index = 0;
			if (ProjectContainer.YSDK.TryGetPlayerName(out string name))
			{
				SetName(PlayerId.Person, name, _customNames[PlayerId.Person]);
                index = PlayerId.AI_01;
			}

			for (; index < PlayerId.HumansCount; ++index)
                SetName(index, localization.GetText(LangFiles.Main, _nameKeys[index], true), _customNames[index]);

			name = localization.GetText(LangFiles.Main, _nameKeys[PlayerId.Satan], true);
            _names[PlayerId.Satan] = name;
            _nameChanges[PlayerId.Satan].Invoke(name);
        }

        private void SetName(int id, string name, string customName)
		{
            _defaults[id] = name;

            if (!string.IsNullOrWhiteSpace(customName))
				name = customName;

            _names[id] = name;
            _nameChanges[id].Invoke(name);
        }

        [Impl(256)] private void SetName(int id, string name)
        {
            _names[id] = name;
            _nameChanges[id].Invoke(name);
            _namesChange.Invoke(_customNames);
        }

#if UNITY_EDITOR
        public void OnValidate()
		{
			_nameKeys ??= PlayerId.Names_Ed;
		}
#endif
	}
}
