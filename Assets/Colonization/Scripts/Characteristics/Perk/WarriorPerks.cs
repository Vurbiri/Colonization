//Assets\Colonization\Scripts\Characteristics\Perk\WarriorPerks.cs
using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Characteristics
{
    public class WarriorPerks : IReactive<IPerk>
	{
		private readonly List<Perk> _perks = new();
        private readonly Signer<IPerk> _signer = new();
        private readonly Unsubscriber _unsubscriber;

        public WarriorPerks(IReactive<Perk> perks) => _unsubscriber = perks.Subscribe(OnPerks);

        public Unsubscriber Subscribe(Action<IPerk> action, bool instantGetValue = true)
        {
            for(int i = _perks.Count - 1; instantGetValue & i >= 0; i--)
                action(_perks[i]);

            return _signer.Add(action);
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
			_signer.Invoke(perk);
		}
    }
}
