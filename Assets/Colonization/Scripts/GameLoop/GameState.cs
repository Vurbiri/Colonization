using System;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    using static SAVE_KEYS;

    public partial class GameState : IReactive<GameState>
    {
        private bool _isLoad;
        private int _maxScore;
        private bool _isFirstStart;

        private ProjectStorage _storage;
        private readonly Subscription<GameState> _eventChanged = new();

        public bool IsLoad
        {
            get => _isLoad;
            set { if (!value) Reset(); else _isLoad = true; }
        }
        public int MaxScore
        {
            get => _maxScore;
            set
            {
                if(value > _maxScore)
                {
                    _maxScore = value;
                    _storage.Set(GAME_STATE, this);
                }
            }
        }

        public bool IsFirstStart => _isFirstStart;
        public AStorage Storage => _storage;

        private GameState()
        {
            _isLoad = false;
            _isFirstStart = true;
        }
        private GameState(bool isLoad, int maxScore)
        {
            _isLoad = isLoad;
            _maxScore = maxScore;
            _isFirstStart = false;
        }

        public static GameState Create(ProjectStorage storage, DIContainer diContainer)
        {
            if (!storage.TryLoadAndBindPGameState(out var instance))
            {
                instance = new();
                storage.BindGameState(instance, true);
            }
            instance._storage = storage;

            return diContainer.AddInstance(instance);
        }

        public Unsubscription Subscribe(Action<GameState> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, this);

        public void Start()
        {
            _isLoad = true;
            _storage.Set(GAME_STATE, this);
        }

        public void Reset()
        {
            _isLoad = false;
            _isFirstStart = false;

            _storage.Clear();
            _storage.Save(GAME_STATE, this);
        }

        public bool TryGetGame(out Game game)
        {
            game = null;
            return _isLoad && _storage.TryGet(GAME, out game);
        }
        public int[] GetScoreData(int defaultSize)
        {
            if (_isLoad && _storage.TryGet(SCORE, out int[] data))
                return data;

            return new int[defaultSize];
        }
    }
}
