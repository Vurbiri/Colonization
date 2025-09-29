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
            _subscription += mixer.Subscribe(self => _storage.Set(VOLUMES, self, _mixerConverter), false);
        }
        public void SetAndBindProfile(Profile profile)
        {
            _storage.TryPopulate<Profile>(PROFILE, new Profile.Converter(profile));
            _subscription += profile.Subscribe(self => _storage.Set(PROFILE, self, _profileConverter), false);
        }
        public void SetAndBindPlayerColors(PlayerColors colors)
        {
            _storage.TryPopulate<PlayerColors>(COLORS, new PlayerColors.Converter(colors));
            _subscription += colors.Subscribe(self => _storage.Set(COLORS, self, _colorsConverter), false);
        }

        public bool TryLoadPlayerNames(out string[] names) => _storage.TryGet(NAMES, out names);
        public void BindPlayerNames(IReactive<PlayerNames> reactive)
        {
            _subscription += reactive.Subscribe(names => _storage.Set(NAMES, names.CustomNames), false);
        }

        public GameSettings LoadGameSettings()
        {
            bool notLoad = !_storage.TryGet(GAME_SETTINGS, out GameSettings settings);
            if (notLoad) settings = new();
 
            _subscription += settings.Subscribe(BindGameSettings, notLoad);
            return settings;
        }
        private void BindGameSettings(GameSettings settings, bool isSave)
        {
            if (isSave)
            {
                _storage.Clear();
                _storage.Save(GAME_SETTINGS, settings);
            }
            else
            {
                _storage.Set(GAME_SETTINGS, settings);
            }
        }
    }
}
