using System;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class PlayerScore : IReactive<int>
    {
        private readonly int _id;
        private readonly int[] _values;
        private readonly ScoreSettings _settings;
        private readonly Subscription<int> _eventChanged = new();

        public PlayerScore(int id, int[] values, ScoreSettings settings)
		{
            _id = id;
            _values = values;
			_settings = settings;
        }

        public Unsubscription Subscribe(Action<int> action, bool instantGetValue = true) => _eventChanged.Add(action, instantGetValue, _values[_id]);

        public void DemonKill(int id)
        {
            _eventChanged.Invoke(_values[_id] += _settings.killDemon[id]);
        }

        public void WarriorKill(int id)
        {
            _eventChanged.Invoke(_values[_id] += _settings.killWarrior[id]);
        }

        public void Build(int id)
        {
            _eventChanged.Invoke(_values[_id] += _settings.buildEdifice[id]);
        }
    }
}
