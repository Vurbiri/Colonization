//Assets\Colonization\Scripts\Characteristics\Perk\WarriorPerks.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class WarriorPerks : IReactive<IPerk>
	{
		private readonly List<Perk> _perks = new();
        private readonly Subscriber<IPerk> _subscriber = new();
        private readonly Unsubscriber _unsubscriber;

        public WarriorPerks(IReactive<Perk> perks) => _unsubscriber = perks.Subscribe(OnPerks);

        public Unsubscriber Subscribe(Action<IPerk> action, bool sendCallback = true)
        {
            for(int i = _perks.Count - 1; sendCallback & i >= 0; i--)
                action(_perks[i]);

            return _subscriber.Add(action);
        }

        public void Dispose()
        {
            _unsubscriber.Unsubscribe();
        }

        private void OnPerks(Perk perk)
		{
			if (perk.TargetObject == TargetOfPerkId.Player)
				return;

			_perks.Add(perk);
			_subscriber.Invoke(perk);
		}
    }
}
