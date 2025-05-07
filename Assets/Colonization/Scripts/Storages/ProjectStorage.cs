//Assets\Colonization\Scripts\Storages\ProjectStorage.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Storage
{
    public class ProjectStorage : IDisposable
    {
        private readonly IStorageService _storage;
        private readonly AudioMixer<MixerId>.Converter _mixerConverter = new();
        private readonly Profile.Converter _profileConverter = new();
        private Unsubscribers _unsubscribers = new();

        public ProjectStorage(IStorageService storage)
        {
            _storage = storage;
        }

        public void Save(Action<bool> callback = null) => _storage.SaveAll(callback);

        public void Clear() => _storage.Clear(SAVE_KEYS.PROFILE, SAVE_KEYS.VOLUMES);

        public bool SetAndBindAudioMixer(AudioMixer<MixerId> mixer)
        {
            bool instantGetValue = !_storage.TryPopulate<AudioMixer<MixerId>>(SAVE_KEYS.VOLUMES, new AudioMixer<MixerId>.Converter(mixer));
            _unsubscribers += mixer.Subscribe(volumes => _storage.Set(SAVE_KEYS.VOLUMES, volumes, _mixerConverter), instantGetValue);
            return instantGetValue;
        }

        public bool SetAndBindProfile(Profile profile)
        {
            bool instantGetValue = !_storage.TryPopulate<Profile>(SAVE_KEYS.PROFILE, new Profile.Converter(profile));
            _unsubscribers += profile.Subscribe(values => _storage.Set(SAVE_KEYS.PROFILE, values, _profileConverter), instantGetValue);
            return instantGetValue;
        }

        public bool TryGetScoreData(out int[] data)
        {
            if(_storage.TryGet(SAVE_KEYS.SCORE, out data))
                return true;
            
            data = new int[PlayerId.HumansCount];
            return false;
        }
        public void ScoreBind(IReactive<IReadOnlyList<int>> reactive, bool instantGetValue)
        {
            _unsubscribers += reactive.Subscribe(score => _storage.Set(SAVE_KEYS.SCORE, score), instantGetValue);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }
    }
}
