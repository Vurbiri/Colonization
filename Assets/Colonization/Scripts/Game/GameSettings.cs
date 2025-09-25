using System;
using Vurbiri.Colonization.EntryPoint;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    using static SAVE_KEYS;

    public partial class GameSettings : IReactive<GameSettings>
    {
        private bool _isLoad;
        private int _maxScore;
        private bool _isTutorial;

        private ProjectStorage _storage;
        private readonly VAction<GameSettings> _eventChanged = new();

        public bool IsLoad
        {
            get => _isLoad;
            set { if (!value) Reset(0); else _isLoad = true; }
        }
        public int MaxScore => _maxScore;

        public bool IsTutorial => _isTutorial;

        private GameSettings()
        {
            _isLoad = false;
            _isTutorial = true;
        }
        private GameSettings(bool isLoad, int maxScore)
        {
            _isLoad = isLoad;
            _maxScore = maxScore;
            _isTutorial = false;
        }

        public static void Create(ProjectContent content)
        {
            if (!content.projectStorage.TryLoadAndBindGameState(out var instance))
            {
                instance = new();
                content.projectStorage.BindGameState(instance, true);
            }
            instance._storage = content.projectStorage;

            content.gameSettings = instance;
        }

        public Subscription Subscribe(Action<GameSettings> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, this);

        public void Start()
        {
            _isLoad = true;
            _storage.Set(GAME_STATE, this);
        }

        public void Reset()
        {
            int score = 0;
            if (_isLoad && _storage.TryGet(SCORE, out int[] scores))
                score = scores[PlayerId.Person];

            Reset(score);
        }
        public void Reset(int score)
        {
            _isLoad = false;
            if (score > _maxScore)  _maxScore = score;

            _storage.Clear();
            _storage.Save(GAME_STATE, this);
        }

    }
}
