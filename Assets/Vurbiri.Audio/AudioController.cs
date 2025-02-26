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

        private readonly IdArray<AudioTypeId, Subscriber<float>> subscribers = new();
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
                subscribers[type].Invoke(value);
            }
        }
        public float this[int id]
        {
            get => _volumes[id];
            set
            {
                _volumes[id] = value;
                subscribers[id].Invoke(value);
            }
        }

        static AudioController() => _instance = new();
        private AudioController() 
        {
            for (int i = 0; i < AudioTypeId.Count; i++)
                subscribers[i] = new();
        }

        public Unsubscriber Subscribe(Id<AudioTypeId> id, Action<float> action, bool calling = true)
        {
            if (calling)
                action(_volumes[id]);

            return subscribers[id].Add(action);
        }
    }
}
