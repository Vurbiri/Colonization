using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public class Chaos : Reactive<int>
	{
        private readonly ChaosSettings _settings;
        private readonly VAction<Winner> _eventGameOver = new();

        [Impl(256)] public static implicit operator int(Chaos chaos) => chaos._value;

        public Event<Winner> OnGameOver { [Impl(256)] get => _eventGameOver; }
        public int Max { [Impl(256)] get => _settings.max; }

        public Chaos(GameStorage storage, GameLoop gameLoop, ActorsFactory actorsFactory)
        {
            _settings = SettingsFile.Load<ChaosSettings>();
            _settings.penaltyPerBlood *= -1;

            _value = storage.GetChaosValue(_settings.max >> 1);
            storage.BindChaos(this);

            _eventGameOver.Add(gameLoop.GameOver);

            actorsFactory[PlayerId.Satan].Subscribe(OnDemonDeath, false);
            for (int i = 0; i < PlayerId.HumansCount; i++)
                actorsFactory[i].Subscribe(OnWarriorDeath, false);
        }

        [Impl(256)] public void ForDemonCurse(int value) => Add(_settings.penaltyPerDemon * value);
        [Impl(256)] public void ForSatanLevelUP(int level) => Add(level);
        [Impl(256)] public void ForSatanTurn() => Add(_settings.penaltyPerTurn);

        [Impl(256)] public void BindShrines(ReadOnlyReactiveList<Crossroad> shrines) => shrines.Subscribe((_, _, _) => Add(_settings.rewardPerShrine), false);
        [Impl(256)] public void BindBlood(Blood blood) => blood.SubscribeDelta(OnPayInBlood);

        public void Add(int value)
        {
            if (value != 0)
            {
                _value += value;

                if (_value <= 0)
                {
                    _value = 0;
                    _eventGameOver.InvokeOneShot(Winner.Human);
                }
                else if (_value >= _settings.max)
                {
                    _value = _settings.max;
                    _eventGameOver.InvokeOneShot(Winner.Satan);
                }

                _onChange.Invoke(_value);
            }
        }

        private void OnWarriorDeath(Actor actor, TypeEvent op)
        {
            if (op == TypeEvent.Remove)
                Add(_settings.penaltyPerKillWarrior[actor.Id]);
        }

        private void OnDemonDeath(Actor actor, TypeEvent op)
        {
            if (op == TypeEvent.Remove)
                Add(_settings.rewardPerKillDemon[actor.Id]);
        }

        private void OnPayInBlood(int delta)
        {
            if (delta < 0)
                Add(_settings.penaltyPerBlood * delta);
        }
    }
}
