//Assets\Colonization\Scripts\Storages\ProjectStorage.cs
using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Storage
{
    using static SAVE_KEYS;
    using static Vurbiri.Colonization.GameState;

    public class ProjectStorage : IDisposable
    {
        private readonly IStorageService _storage;
        private readonly AudioMixer<MixerId>.Converter _mixerConverter = new();
        private readonly Profile.Converter _profileConverter = new();
        private readonly PlayerColors.Converter _colorsConverter = new();
        private Unsubscribers _unsubscribers = new();

        public ProjectStorage(IStorageService storage)
        {
            _storage = storage;
        }

        public void Save(Action<bool> callback = null) => _storage.SaveAll(callback);

        public void Clear() => _storage.Clear(PROFILE, VOLUMES, COLORS, GAME_DATA);

        public bool SetAndBindAudioMixer(AudioMixer<MixerId> mixer)
        {
            bool instantGetValue = !_storage.TryPopulate<AudioMixer<MixerId>>(VOLUMES, new AudioMixer<MixerId>.Converter(mixer));
            _unsubscribers += mixer.Subscribe(self => _storage.Set(VOLUMES, self, _mixerConverter), instantGetValue);
            return instantGetValue;
        }
        public bool SetAndBindProfile(Profile profile)
        {
            bool instantGetValue = !_storage.TryPopulate<Profile>(PROFILE, new Profile.Converter(profile));
            _unsubscribers += profile.Subscribe(self => _storage.Set(PROFILE, self, _profileConverter), instantGetValue);
            return instantGetValue;
        }
        public void SetAndBindPlayerColors(PlayerColors colors)
        {
            bool instantGetValue = !_storage.TryPopulate<PlayerColors>(COLORS, new PlayerColors.Converter(colors));
            _unsubscribers += colors.Subscribe(self => _storage.Set(COLORS, self, _colorsConverter), instantGetValue);
        }

        public bool TryGetScoreData(out int[] data) => _storage.TryGet(SCORE, out data);
        public void ScoreBind(IReactive<Score> reactive, bool instantGetValue)
        {
            _unsubscribers += reactive.Subscribe(score => _storage.Set(SCORE, score.Values), instantGetValue);
        }

        public bool TryLoadAndBindGameData(out GameData data)
        {
            if (_storage.TryGet(GAME_DATA, out data))
            {
                GameDataBind(data, false);
                return true;
            }
            return false;
        }
        public void GameDataBind(IReactive<GameData> reactive, bool instantGetValue)
        {
            _unsubscribers += reactive.Subscribe(self => _storage.Set(GAME_DATA, self), instantGetValue);
        }

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }
    }
}
