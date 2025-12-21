using System;
using UnityEngine;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public partial class PlayerColors : IReactive<PlayerColors>, IEquatable<Color[]>
    {
        [SerializeField] private Color[] _colors;

        private readonly VAction<PlayerColors> _colorsChange = new();
        private readonly VAction<Color>[] _colorChanges = new VAction<Color>[PlayerId.Count];

        public Color this[int index] { [Impl(256)] get => _colors[index]; }
        public Color this[Id<PlayerId> id] { [Impl(256)] get => _colors[id.Value]; }

        public Color[] Colors { [Impl(256)] get => _colors; }

        public PlayerColors()
        {
            for (int i = 0; i < PlayerId.Count; ++i)
                _colorChanges[i] = new();
        }

        public Subscription Subscribe(Action<PlayerColors> action, bool instantGetValue = true) => _colorsChange.Add(action, this, instantGetValue);

        [Impl(256)] public Subscription Subscribe(int playerId, Action<Color> action, bool instant = true) => _colorChanges[playerId].Add(action, _colors[playerId], instant);
        [Impl(256)] public void Unsubscribe(int playerId, Action<Color> action) => _colorChanges[playerId].Remove(action);

        public bool Equals(Color[] colors)
        {
            for(int i = 0; i < PlayerId.Count; i++)
                if(_colors[i] != colors[i])
                    return false;

            return true;
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            _colors ??= new Color[] { new(0.38f, 0.21f, 0.77f), new(0.7f, 0.89f, 0.18f), new(0.25f, 0.59f, 0.15f), new(0.57f, 0f, 0f) };

        }
#endif
    }
}
