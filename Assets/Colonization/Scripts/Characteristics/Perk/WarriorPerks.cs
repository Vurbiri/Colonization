using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class WarriorPerks : IReactive<IPerk>
	{
		private readonly List<Perk> _perks = new();
        private readonly VAction<IPerk> _changeEvent = new();
        private readonly Subscription _subscription;

        public WarriorPerks(IReactive<Perk> perks) => _subscription = perks.Subscribe(OnPerks);

        public Subscription Subscribe(Action<IPerk> action, bool instantGetValue = true)
        {
            for(int i = _perks.Count - 1; instantGetValue & i >= 0; i--)
                action(_perks[i]);

            return _changeEvent.Add(action);
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        private void OnPerks(Perk perk)
		{
			if (perk.TargetObject == TargetOfPerkId.Player)
				return;

			_perks.Add(perk);
			_changeEvent.Invoke(perk);
		}
    }
}
