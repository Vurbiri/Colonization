using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public class Balance : AReactive<int>
	{
        private readonly BalanceSettings _settings;
        private readonly VAction<Winner> _eventGameOver = new();

        public int Value => _value;
        public Event<Winner> OnGameOver => _eventGameOver;

        public Balance(GameStorage storage, GameLoop gameLoop, ActorsFactory actorsFactory)
        {
            _settings = SettingsFile.Load<BalanceSettings>();
            _settings.penaltyPerBlood *= -1;

            _value = storage.GetBalanceValue(_settings.defaultValue);
            storage.BindBalance(this);

            _eventGameOver.Add(gameLoop.End_Cn);

            actorsFactory[PlayerId.Satan].Subscribe(OnKillingDemon, false);
            for (int i = 0; i < PlayerId.HumansCount; i++)
                actorsFactory[i].Subscribe(OnKillingWarrior, false);
        }

        public void ForDemonCurse(int value) => Add(_settings.penaltyPerDemon * value);
        public void ForSatanLevelUP() => Add(_settings.penaltyPerSatanLevelUp);

        public void BindShrines(ReadOnlyReactiveList<Crossroad> shrines) => shrines.Subscribe((_, _, _) => Add(_settings.rewardPerShrine), false);
        public void BindBlood(Currency blood) => blood.SubscribeDelta(OnPayInBlood);

        public void Add(int value)
        {
            if (value != 0)
            {
                _value += value;

                _eventChanged.Invoke(value);

                if (_value <= _settings.min)
                    _eventGameOver.Invoke(Winner.Satan);
                if (_value >= _settings.max)
                    _eventGameOver.Invoke(Winner.Human);
            }
        }

        private void OnKillingWarrior(Actor actor, TypeEvent op)
        {
            if (op == TypeEvent.Remove)
                Add(_settings.penaltyPerKillWarrior[actor.Id]);
        }

        private void OnKillingDemon(Actor actor, TypeEvent op)
        {
            if (op == TypeEvent.Remove)
                Add(_settings.rewardPerKillDemon[actor.Id]);
        }

        private void OnPayInBlood(int delta)
        {
            if (delta < 0)
                Add(_settings.penaltyPerBlood * delta);
        }

        public static implicit operator int(Balance balance) => balance._value;
    }
}
