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
        private bool _isTutarial;

        private ProjectStorage _storage;
        private readonly Subscription<GameState> _eventChanged = new();

        public bool IsLoad
        {
            get => _isLoad;
            set { if (!value) Reset(0); else _isLoad = true; }
        }
        public int MaxScore => _maxScore;

        public bool IsFirstStart => _isTutarial;

        private GameState()
        {
            _isLoad = false;
            _isTutarial = true;
        }
        private GameState(bool isLoad, int maxScore)
        {
            _isLoad = isLoad;
            _maxScore = maxScore;
            _isTutarial = false;
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
            int score = 0;
            if (_isLoad && _storage.TryGet(SCORE, out int[] scores))
                score = scores[PlayerId.Player];

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
