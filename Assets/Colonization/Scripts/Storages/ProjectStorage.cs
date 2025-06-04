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

        public void SetAndBindAudioMixer(AudioMixer<MixerId> mixer)
        {
            _storage.TryPopulate<AudioMixer<MixerId>>(VOLUMES, new AudioMixer<MixerId>.Converter(mixer));
            _unsubscribers += mixer.Subscribe(self => _storage.Set(VOLUMES, self, _mixerConverter), false);
        }
        public void SetAndBindProfile(Profile profile)
        {
            _storage.TryPopulate<Profile>(PROFILE, new Profile.Converter(profile));
            _unsubscribers += profile.Subscribe(self => _storage.Set(PROFILE, self, _profileConverter), false);
        }
        public void SetAndBindPlayerColors(PlayerColors colors)
        {
            _storage.TryPopulate<PlayerColors>(COLORS, new PlayerColors.Converter(colors));
            _unsubscribers += colors.Subscribe(self => _storage.Set(COLORS, self, _colorsConverter), false);
        }

        public bool TryLoadPlayerNames(out string[] names) => _storage.TryGet(NAMES, out names);
        public void BindPlayerNames(IReactive<string[]> reactive)
        {
            _unsubscribers += reactive.Subscribe(names => _storage.Set(NAMES, names), false);
        }


        public bool TryLoadAndBindGameState(out GameState state)
        {
            if(_storage.TryGet(GAME_STATE, out state))
            {
                BindGameState(state, false);
                return true;
            }
            return false;
        }
        public void BindGameState(IReactive<GameState> reactive, bool instantGetValue)
        {
            _unsubscribers += reactive.Subscribe(self => _storage.Set(GAME_STATE, self), instantGetValue);
        }
    }
}
