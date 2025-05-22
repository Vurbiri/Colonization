//Assets\Colonization\Scripts\GameState\GameState.cs
using System;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class GameState : IReactive<GameState>
    {
        private bool _isLoad;
        private int _maxScore;
        private readonly int[] _score;
        private readonly ScoreSettings _scoreSettings;
        private bool _isFirstStart;

        private ProjectStorage _storage;
        private readonly Subscription<GameState> _eventChanged = new();

        public bool IsLoad
        {
            get => _isLoad;
            set { if (!value) Reset(); else _isLoad = true; }
        }

        public bool IsFirstStart => _isFirstStart;

        private GameState()
        {
            _isLoad = false;
            _score = new int[PlayerId.HumansCount];
            _scoreSettings = SettingsFile.Load<ScoreSettings>();
            _isFirstStart = true;
        }
        private GameState(bool isLoad, int maxScore, int[] score)
        {
            _isLoad = isLoad;
            _maxScore = maxScore;
            _score = score;
            _scoreSettings = SettingsFile.Load<ScoreSettings>();
            _isFirstStart = false;
        }

        public static GameState Create(ProjectStorage storage, DIContainer diContainer)
        {
            if (!storage.TryLoadAndBindPGameState(out var instance))
            {
                instance = new();
                storage.GameStateBind(instance, true);
            }

            diContainer.AddInstance(instance);
            diContainer.AddFactory<Id<PlayerId>, PlayerScore>(instance.GetPlayerScore);
            instance._storage = storage;

            return instance;
        }

        public Unsubscription Subscribe(Action<GameState> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, this);


        public bool TryGetGame(out GameLoop game)
        {
            game = null;
            return _isLoad && _storage.TryGet(SAVE_KEYS.GAME, out game);
        }
        public void SaveGame(GameLoop game) => _storage.Save(SAVE_KEYS.GAME, game);

        public void Start()
        {
            _isLoad = true;
        }

        public void Reset()
        {
            _isLoad = false;
            _maxScore = Math.Max(_maxScore, _score[PlayerId.Player]);
            for (int i = 0; i < PlayerId.HumansCount; i++)
                _score[i] = 0;
            _isFirstStart = false;

            _storage.Clear();
            _storage.Save(SAVE_KEYS.GAME_STATE, this);
        }

        private PlayerScore GetPlayerScore(Id<PlayerId> id)
        {
            PlayerScore playerScore = new(id.Value, _score, _scoreSettings);
            playerScore.Subscribe(_ => _eventChanged.Invoke(this), false);
            return playerScore;
        }
    }
}
