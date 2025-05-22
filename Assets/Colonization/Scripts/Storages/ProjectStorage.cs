//Assets\Colonization\Scripts\Storages\ProjectStorage.cs
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Storage
{
    using static SAVE_KEYS;

    sealed public class ProjectStorage : AStorage
    {
        private readonly AudioMixer<MixerId>.Converter _mixerConverter = new();
        private readonly Profile.Converter _profileConverter = new();
        private readonly PlayerColors.Converter _colorsConverter = new();

        public ProjectStorage(IStorageService storage) : base(storage) { }

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


        public bool TryLoadAndBindPGameState(out GameState state)
        {
            if(_storage.TryGet(GAME_STATE, out state))
            {
                GameStateBind(state, false);
                return true;
            }
            return false;
        }
        public void GameStateBind(IReactive<GameState> reactive, bool instantGetValue)
        {
            _unsubscribers += reactive.Subscribe(self => _storage.Set(GAME_STATE, self), instantGetValue);
        }
    }
}
