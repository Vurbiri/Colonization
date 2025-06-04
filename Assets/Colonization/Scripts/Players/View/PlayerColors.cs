using System;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public partial class PlayerColors : IReactive<PlayerColors>, IEquatable<Color[]>
    {
        [SerializeField] private Color[] _colors;

        private readonly Subscription<PlayerColors> _eventThisChanged = new();

        public Color this[int index] => _colors[index];
        public Color this[Id<PlayerId> id] => this[id.Value];

        public Color[] Colors
        {
            get => _colors;
            set
            {
                if(!this.Equals(value))
                {
                    _colors = value;
                    _eventThisChanged.Invoke(this);
                }
            }
        }

        public Unsubscription Subscribe(Action<PlayerColors> action, bool instantGetValue = true) => _eventThisChanged.Add(action, instantGetValue, this);

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
            _colors ??= new Color[] { new(0.38f, 0.21f, 0.77f), new(0.64f, 0.89f, 0.08f), new(0.35f, 0.72f, 0.43f), new(0.57f, 0f, 0f) };

        }
#endif
    }
}
