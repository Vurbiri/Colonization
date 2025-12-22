using System;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [Serializable]
    public partial class PlayerColors : IDisposable
    {
        [SerializeField] private Color[] _colors;
        [SerializeField] private Color32[] _defaults;

        private readonly VAction<PlayerColors> _colorsChange = new();

        public Color this[int index] { [Impl(256)] get => _colors[index]; }
        public Color this[Id<PlayerId> id] { [Impl(256)] get => _colors[id.Value]; }

        public Color[] Colors { [Impl(256)] get => _colors; }

        [Impl(256)] public Subscription Subscribe(Action<PlayerColors> action, bool instantGetValue = true) => _colorsChange.Add(action, this, instantGetValue);
        [Impl(256)] public void Unsubscribe(Action<PlayerColors> action) => _colorsChange.Remove(action);

        public bool Equals(Color[] colors)
        {
            for(int i = 0; i < PlayerId.Count; i++)
                if(_colors[i] != colors[i])
                    return false;

            return true;
        }

        public void Dispose()
        {
            _colorsChange.Clear();
        }

#if UNITY_EDITOR
        public void OnValidate()
        {
            _colors ??= new Color[] { new(0.38f, 0.21f, 0.77f), new(0.7f, 0.89f, 0.18f), new(0.25f, 0.59f, 0.15f), new(0.57f, 0f, 0f) };
            _defaults ??= new Color32[] { new(97, 54, 196, 255), new(181, 227, 144, 255), new(13, 145, 58, 255), new(145, 0, 0, 255) };
        }
#endif
    }
}
