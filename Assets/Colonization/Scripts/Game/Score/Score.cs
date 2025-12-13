using System;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public partial class Score : IReactive<Score>
    {
        private readonly int[] _values;
        private readonly ScoreSettings _settings;
        private readonly VAction<Score> _eventChanged = new();

        public int this[int index] { [Impl(256)] get => _values[index]; }

        [Impl(256)] private Score(int[] values)
        {
            _settings = SettingsFile.Load<ScoreSettings>();
            _values = values;
        }
        [Impl(256)] private Score() : this(new int[PlayerId.HumansCount]) { }

        public static Score Create(GameStorage storage)
        {
            if (!storage.TryGetScore(out Score instance))
                instance = new();

            storage.BindScore(instance);
            return instance;
        }

        [Impl(256)] public Subscription Subscribe(Action<Score> action, bool instantGetValue = true) => _eventChanged.Add(action, this, instantGetValue);

        [Impl(256)] public void ForKillingDemon(int playerId, int demonId) => Add(playerId, _settings.killDemon[demonId]);
        [Impl(256)] public void ForKillingWarrior(int playerId, int warriorId) => Add(playerId, _settings.killWarrior[warriorId]);
        [Impl(256)] public void ForBuilding(int playerId, int edificeId) => Add(playerId, _settings.buildEdifice[edificeId]);
        [Impl(256)] public void ForWall(int playerId) => Add(playerId, _settings.perWall);
        [Impl(256)] public void ForRoad(int playerId) => Add(playerId, _settings.perRoad);
        [Impl(256)] public void ForAddingOrder(int playerId, int order) => Add(playerId, order * _settings.perOrder);

        [Impl(256)] public void Add(int playerId, int value)
        {
            _values[playerId] += value;
            _eventChanged.Invoke(this);
        }
    }
}
