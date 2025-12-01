using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public class Chaos : ReactiveValue<int>
	{
        private readonly ChaosSettings _settings;
        private readonly VAction<Winner> _eventGameOver = new();

        public Event<Winner> OnGameOver => _eventGameOver;
        public int Max => _settings.max;

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

        public void ForDemonCurse(int value) => Add(_settings.penaltyPerDemon * value);
        public void ForSatanLevelUP(int level) => Add(-level);

        public void BindShrines(ReadOnlyReactiveList<Crossroad> shrines) => shrines.Subscribe((_, _, _) => Add(_settings.rewardPerShrine), false);
        public void BindBlood(Currency blood) => blood.SubscribeDelta(OnPayInBlood);

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

                _changeEvent.Invoke(_value);
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

        public static implicit operator int(Chaos balance) => balance._value;
    }
}
