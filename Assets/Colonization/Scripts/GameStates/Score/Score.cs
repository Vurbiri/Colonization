using System;
using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
	public class Score : IReactive<int[]>
    {
        private readonly int[] _values;
        private readonly ScoreSettings _settings;
        private readonly Subscription<int[]> _eventChanged = new();

        public Score(GameStorage storage)
        {
            _settings = SettingsFile.Load<ScoreSettings>();
            _values = storage.GetScoreData(PlayerId.HumansCount);
            storage.BindScore(this);
        }

        public Unsubscription Subscribe(Action<int[]> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, _values);

        public void ForKillingDemon(int playerId, int demonId) => Add(playerId, _settings.killDemon[demonId]);
        public void ForKillingWarrior(int playerId, int warriorId) => Add(playerId, _settings.killWarrior[warriorId]);
        public void ForBuilding(int playerId, int edificeId) => Add(playerId, _settings.buildEdifice[edificeId]);
        public void ForWall(int playerId) => Add(playerId, _settings.perWall);
        public void ForRoad(int playerId) => Add(playerId, _settings.perRoad);
        public void ForAddingOrder(int playerId, int order) => Add(playerId, order * _settings.perOrder);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Add(int playerId, int value)
        {
            _values[playerId] += value;
            _eventChanged.Invoke(_values);
        }
    }
}
