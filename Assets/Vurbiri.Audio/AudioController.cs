//Assets\Vurbiri.Audio\AudioController.cs
using System;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Audio
{
    public class AudioController : IReactiveBase<Id<AudioTypeId>, Action<float>>
    {
        private static readonly AudioController _instance;

        private readonly IdArray<AudioTypeId, Action<float>> actionsValues = new();
        private readonly IdArray<AudioTypeId, float> _volumes = new();

        public static AudioController Instance => _instance;

        public bool Mute { get => AudioListener.pause; set => AudioListener.pause = value; }
        public float Volume { get => AudioListener.volume; set => AudioListener.volume = value; }

        public float this[Id<AudioTypeId> type]
        {
            get => _volumes[type];
            set
            {
                _volumes[type] = value;
                actionsValues[type]?.Invoke(value);
            }
        }
        public float this[int id]
        {
            get => _volumes[id];
            set
            {
                _volumes[id] = value;
                actionsValues[id]?.Invoke(value);
            }
        }

        static AudioController() => _instance = new();
        private AudioController() { }

        public IUnsubscriber Subscribe(Id<AudioTypeId> id, Action<float> action, bool calling = true)
        {
            actionsValues[id] += action;
            if (calling) action(_volumes[id]);

            return new Unsubscriber<Id<AudioTypeId>, Action<float>>(this, id, action);
        }

        public void Unsubscribe(Id<AudioTypeId> id, Action<float> action) => actionsValues[id] -= action;
    }
}
