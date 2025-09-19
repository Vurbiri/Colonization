using System;
using System.Runtime.CompilerServices;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public partial class Score : IReactive<Score>
    {
        private readonly int[] _values;
        private readonly ScoreSettings _settings;
        private readonly Subscription<Score> _eventChanged = new();

        public int this[int index] { [Impl(256)] get => _values[index]; }

        private Score(int[] values)
        {
            _settings = SettingsFile.Load<ScoreSettings>();
            _values = values;
        }
        private Score() : this(new int[PlayerId.HumansCount]) { }

        public static Score Create(GameStorage storage)
        {
            if (!storage.TryGetScore(out Score instance))
                instance = new();

            storage.BindScore(instance);
            return instance;
        }

        public Unsubscription Subscribe(Action<Score> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, this);

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
            _eventChanged.Invoke(this);
        }
    }
}
