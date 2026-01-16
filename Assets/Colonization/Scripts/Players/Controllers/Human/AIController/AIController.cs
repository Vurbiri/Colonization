using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
	sealed public partial class AIController : HumanController
	{
		private static readonly AIControllerSettings s_aiSettings;
		
		private readonly Counselors _counselors;
		private readonly int _specialization = AbilityTypeId.Economic;

		static AIController() => s_aiSettings = SettingsFile.Load<AIControllerSettings>();

		public AIController(Id<PlayerId> playerId, Settings settings, WaitAllWaits waitSpawn) : base(playerId, settings, waitSpawn, false) 
		{
			if (s_aiSettings.militarist == playerId)
				_specialization = AbilityTypeId.Military;

			_counselors = new(this);
		}

		public override WaitResult<bool> OnGift(int giver, LiteCurrencies gift, string msg) => _counselors.GiftReceive(giver, gift);

		public override void OnLanding()
		{
			if (_edifices.ports.Count > 0)
				GameContainer.GameLoop.EndLanding();
			else
				_coroutine = StartCoroutine(OnLanding_Cn());

			// ======= Local ==========
			IEnumerator OnLanding_Cn()
			{
				_resources.Blood.Add(_id.Value);
				
				yield return s_aiSettings.waitPlayStart.Restart();
				yield return _counselors.Landing_Cn();

				GameContainer.GameLoop.EndLanding();
				_coroutine = null;
			}
		}

		public override void OnPlay()
		{
			_coroutine = StartCoroutine(OnPlay_Cn());

			// ======= Local ==========
			IEnumerator OnPlay_Cn()
			{
#if TEST_AI
                Debug.Log($"====================== {_id} ======================");
#endif
				s_aiSettings.cheat.TryAddRes(_resources);

				yield return s_aiSettings.waitPlayStart.Restart();
				yield return _waitAll.Add(s_aiSettings.waitPlay.Restart(), _counselors.Execution_Cn());
#if TEST_AI
                Debug.Log("===================================================");
#endif
				GameContainer.GameLoop.EndTurn();
				_coroutine = null;
			}
		}

		public override void OnEndTurn()
		{
			_counselors.Update();

			_coroutine = StartCoroutine(OnEndTurn_Cn());
		}

		public override void Dispose()
		{
			base.Dispose();
			_waitAll.Dispose();
			_counselors.Dispose();
		}

		private IEnumerator Exchange_Cn(ReadOnlyLiteCurrencies needed, Out<bool> output)
		{
			var settings = s_aiSettings.exchange;
			var resources = _resources;
            var result = resources >= needed;

            if (!result)
			{
				var blood = resources.Blood;
				int exchangeBlood = blood >> (_perks.IsAllLearned() ? 0 : 1);
				if (exchangeBlood > settings.minExchangeBlood && Chance.Rolling(blood.Percent - settings.percentBloodOffset))
				{
					_spellBook.Cast(MilitarySpellId.Type, MilitarySpellId.BloodTrade, new(_id, Random.Range(settings.minExchangeBlood, exchangeBlood + 1)));
					result = resources >= needed;
				}

				yield return null;

				if (!result && resources.OverCount(needed, out int exchangeIndex) == 1)
				{
					int need = needed[exchangeIndex], current = resources[exchangeIndex], delta = need - current;
					int exchange = _exchange[exchangeIndex], exchangeValue = delta * exchange;
					if (((resources.Amount - current) - (needed.Amount - need)) > exchangeValue && (resources.PercentAmount > settings.minPercentAmount || Chance.Rolling((int)(6.251f * (20 - exchange * exchange)))))
					{
						LiteCurrencies pay = new(), diff = resources - needed;
						pay.Remove(exchangeIndex, delta);
						diff.Set(exchangeIndex, 0);

						int index;
						while (exchangeValue --> 0)
						{
							index = diff.MaxIndex;
							diff.Decrement(index);
							pay.Increment(index);

							yield return null;
						}
                        resources.Remove(pay);
						result = true;
					}
				}
			}
			output?.Set(result);
			yield break;
		}
	}
}
