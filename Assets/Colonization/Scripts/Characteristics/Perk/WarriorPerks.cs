using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public class WarriorPerks : IReactive<IPerk>
	{
		private readonly List<Perk> _perks = new();
        private readonly VAction<IPerk> _changeEvent = new();

        public WarriorPerks(PerkTree perks) => perks.Subscribe(OnPerks);

        public Subscription Subscribe(Action<IPerk> action, bool instantGetValue = true)
        {
            for(int i = _perks.Count - 1; instantGetValue & i >= 0; i--)
                action(_perks[i]);

            return _changeEvent.Add(action);
        }

        private void OnPerks(Perk perk)
		{
            if (perk.TargetObject == TargetOfPerkId.Warriors)
            {
                _perks.Add(perk);
                _changeEvent.Invoke(perk);
            }
		}
    }
}
