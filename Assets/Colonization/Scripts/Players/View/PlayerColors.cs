//Assets\Colonization\Scripts\Players\View\PlayerColors.cs
using System;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public partial class PlayerColors : IReactive<PlayerColors>
	{
        [SerializeField] private Color[] _colors;// = { new(0.38f, 0.21f, 0.77f), new(0.64f, 0.89f, 0.08f), new(0.35f, 0.91f, 0.43f), new(0.5686275f, 0f, 0f) };

        private readonly Signer<PlayerColors> _eventThisChanged = new();
        private readonly Signer<Color> _eventColorChanged = new();

        public Color this[int index] 
        { 
            get => _colors[index];
            set
            {
                if (_colors[index] != value)
                {
                    _colors[index] = value;
                    _eventColorChanged.Invoke(value);
                    _eventThisChanged.Invoke(this);
                }
            }
        }
        public Color this[Id<PlayerId> id] => this[id.Value];

        public Unsubscriber Subscribe(Action<PlayerColors> action, bool instantGetValue = true) => _eventThisChanged.Add(action, instantGetValue, this);
        public Unsubscriber Subscribe(int index, Action<Color> action, bool instantGetValue = true)
        {
            return _eventColorChanged.Add(action, instantGetValue, _colors[index]);
        }
    }
}
