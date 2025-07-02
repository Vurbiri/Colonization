using System;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
	public class Score : IReactive<int[]>
    {
        private readonly int[] _values;
        private readonly ScoreSettings _settings;
        private readonly Subscription<int[]> _eventChanged = new();

        public Score(GameplayStorage storage)
        {
            _settings = SettingsFile.Load<ScoreSettings>();
            _values = storage.GetScoreData(PlayerId.HumansCount);
            storage.BindScore(this);
        }

        public Unsubscription Subscribe(Action<int[]> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, _values);

        public void OnDemonKill(int playerId, int demonId) => Add(playerId, _settings.killDemon[demonId]);
        public void OnWarriorKill(int playerId, int warriorId) => Add(playerId, _settings.killWarrior[warriorId]);
        public void OnBuild(int playerId, int edificeId) => Add(playerId, _settings.buildEdifice[edificeId]);
        public void OnAddOrder(int playerId, int order) => Add(playerId, order * _settings.perOrder);

        private void Add(int playerId, int value)
        {
            _values[playerId] += value;
            _eventChanged.Invoke(_values);
        }
    }
}
