using System;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
	public class Score : IReactive<int>
    {
        private readonly int[] _values;
        private readonly ScoreSettings _settings;
        private readonly GameState _gameState;
        private readonly Subscription<int> _eventChanged = new();

        public int MaxScore => _gameState.MaxScore;

        public Score(GameState state)
        {
            _values = state.GetScoreData(PlayerId.HumansCount);
            _settings = SettingsFile.Load<ScoreSettings>();
            _gameState = state;
        }

        public Unsubscription Subscribe(Action<int> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, _values[PlayerId.Player]);

        public void DemonKill(int playerId, int demonId) => Add(playerId, _settings.killDemon[demonId]);
        public void WarriorKill(int playerId, int warriorId) => Add(playerId, _settings.killWarrior[warriorId]);
        public void Build(int playerId, int edificeId) => Add(playerId, _settings.buildEdifice[edificeId]);

        private void Add(int playerId, int value)
        {
            _values[playerId] += value;
            _gameState.Storage.Set(SAVE_KEYS.SCORE, _values);
            if (playerId == PlayerId.Player)
            {
                _gameState.MaxScore = _values[playerId];
                _eventChanged.Invoke(_values[playerId]);
            }
        }
    }
}
