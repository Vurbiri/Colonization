//Assets\Colonization\Scripts\Storages\ProjectStorage.cs
using System;
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

        public void Save(Action<bool> callback = null) => _storage.Save(callback);

        public void Clear() => _storage.Clear(SAVE_KEYS.PROFILE, SAVE_KEYS.VOLUMES);

        public bool SetAndBindAudioMixer(AudioMixer<MixerId> mixer)
        {
            bool calling = !_storage.TryPopulate<AudioMixer<MixerId>>(SAVE_KEYS.VOLUMES, new AudioMixer<MixerId>.Converter(mixer));
            _unsubscribers += mixer.Subscribe(volumes => _storage.Set(SAVE_KEYS.VOLUMES, volumes, _mixerConverter), calling);
            return calling;
        }

        public bool SetAndBindProfile(Profile profile)
        {
            bool calling = !_storage.TryPopulate<Profile>(SAVE_KEYS.PROFILE, new Profile.Converter(profile));
            _unsubscribers += profile.Subscribe(values => _storage.Set(SAVE_KEYS.PROFILE, values, _profileConverter), calling);
            return calling;
        }
       
        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }
    }
}
