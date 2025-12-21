using System;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class PlayerUINames : IDisposable
	{
        private readonly UIName[] _names = new UIName[PlayerId.Count];
        private readonly Subscription _unsub;

        public string this[Id<PlayerId> id] { [Impl(256)] get => _names[id.Value]; }
        public string this[int index] { [Impl(256)] get => _names[index];}

        public PlayerUINames(PlayerNames names, PlayerColors colors)
        {
            for (int i = 0; i < PlayerId.Count; i++)
                _names[i] = new(i, names, colors, ref _unsub);
        }

        public void Dispose() => _unsub.Dispose();

        // ******************** Nested *******************
        private class UIName
        {
            private readonly int _id;
            private string _name;

            public string Name { [Impl(256)] get => _name; }

            public UIName(int id, PlayerNames names, PlayerColors colors, ref Subscription unsub)
            {
                _id = id;
                unsub += names.Subscribe(id, Set, false);
                unsub += colors.Subscribe(id, Set, false);

                Set(names[id], colors[id]);
            }

            [Impl(256)] public static implicit operator string(UIName self) => self._name;

            private void Set(Color color) => Set(ProjectContainer.UI.PlayerNames[_id], color);
            private void Set(string name) => Set(name, ProjectContainer.UI.PlayerColors[_id]);

            [Impl(256)] private void Set(string name, Color color) => this._name = $"<{color.ToHex()}>{name}</color>";
        }
    }
}
