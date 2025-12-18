using System.Collections;
using System.Collections.Generic;
using static Vurbiri.Colonization.Actor;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public partial class WarriorAI
	{
		sealed private class Support : Heal
		{
			private readonly WeightsList<Actor> _friends = new(3);

			[Impl(256)] public Support(AI<WarriorsAISettings, WarriorId, WarriorAIStateId> parent) : base(parent) { }

			sealed public override bool TryEnter()
			{
				_friends.Clear(); _wounded.Clear();

				if (Settings.support && Status.nearFriends.NotEmpty && Actor.CurrentAP > 0)
				{
					var buffs = Settings.buffs; var heal = Settings.heal;
					var isValidHeal = heal.IsValid;
					List<Actor> friends = Status.nearFriends; Actor friend;
					for (int i = 0, shiftForce; i < friends.Count; i++)
					{
						friend = friends[i];

						shiftForce = friend.Owner == OwnerId ? 1 : 0;
						if (buffs.CanUsed(Actor, friend))
							_friends.Add(friend, friend.CurrentForce << shiftForce);

						if (isValidHeal && heal.CanUsed(Actor, friend))
							_wounded.Add(friend, (BASE_HP - friend.PercentHP) << shiftForce);
					}
				}

				return _friends.Count > 0 || _wounded.Count > 0;
			}

			sealed public override IEnumerator Execution_Cn(Out<bool> isContinue)
			{
				if (_wounded.Count > 0)
					 yield return Settings.heal.TryUse_Cn(Actor, _wounded.Roll);

				if (_friends.Count > 0)
					yield return Settings.buffs.TryUse_Cn(Actor, _friends.Roll);

				isContinue.Set(Actor.CurrentAP > 0);
				Exit();
			}

			sealed public override void Dispose()
			{
				_friends.Clear();
				_wounded.Clear();
			}
		}

	}
}
