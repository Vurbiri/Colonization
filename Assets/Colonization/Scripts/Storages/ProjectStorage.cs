namespace Vurbiri.Colonization.Storage
{
    using static SAVE_KEYS;

    public class ProjectStorage : System.IDisposable
    {
        private readonly IStorageService _storage;
        private readonly AudioMixer<MixerId>.Converter _mixerConverter = new();
        private readonly Profile.Converter _profileConverter = new();
        private readonly PlayerColors.Converter _colorsConverter = new();
        private Subscription _subscription;

        public ProjectStorage(IStorageService storage) => _storage = storage;

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
        public void BindPlayerNames(PlayerNames reactive)
        {
            _subscription += reactive.Subscribe(values => _storage.Set(NAMES, values), false);
        }

        public GameSettings LoadGameSettings()
        {
            bool notLoad = !_storage.TryGet(GAME_SETTINGS, out GameSettings settings);
            if (notLoad) settings = new();
 
            _subscription += settings.Subscribe(BindGameSettings, notLoad);
            return settings;
        }
        private void BindGameSettings(GameSettings settings, bool isClear)
        {
            _storage.Set(GAME_SETTINGS, settings);

            if (isClear)
                _storage.Clear(NotClear);
        }

        public void Save() => _storage.Save();

        public void Dispose() => _subscription?.Dispose();
    }
}
