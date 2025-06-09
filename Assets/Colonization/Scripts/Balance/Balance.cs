using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
	public class Balance : AReactive<int>
	{
        private readonly BalanceSettings _settings;
        private readonly GameLoop _game;
        private readonly Subscription<Winner> _eventGameOver = new();

        public int Value => _value;
        public ISubscription<Winner> OnGameOver => _eventGameOver;

        public Balance(GameplayStorage storage, GameLoop game)
        {
            _settings = SettingsFile.Load<BalanceSettings>();
            _value = storage.GetBalanceValue(_settings.defaultValue);
            storage.BindBalance(this);

            _eventGameOver.Add(game.End);
        }

        public void DemonCurse(int value) => AddBalance(_settings.penaltyPerDemon * -value);

        public void BindShrines(ReactiveList<Crossroad> shrines)
        {
            shrines.Subscribe((_, _, _) => AddBalance(_settings.rewardPerShrine), false);
        }
        public void BindBlood(IReactive<int, int> blood) => blood.Subscribe(OnPayInBlood, false);

        private void AddBalance(int value)
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

        private void OnPayInBlood(int current, int delta)
        {
            if (delta < 0)
                AddBalance(_settings.penaltyPerBlood * delta);
        }

        public static implicit operator int(Balance balance) => balance._value;
    }
}
