using System.Collections;

namespace Vurbiri.Colonization
{
	public abstract class HumanController : Human,  IPlayerController
	{
		protected readonly WaitResultSource<bool> _waitGift = new();

		protected HumanController(Id<PlayerId> playerId, Settings settings, WaitAllWaits waitSpawn, bool isPerson) : base(playerId, settings, waitSpawn, isPerson) { }

		public void ActorKill(Id<ActorTypeId> type, int id)
		{
			if (type == ActorTypeId.Demon)
			{
				GameContainer.Score.ForKillingDemon(_id.Value, id);
				_resources.Blood.Add(id + 1);
			}
			else
			{
				GameContainer.Score.ForKillingWarrior(_id.Value, id);
			}
		}

		public abstract WaitResult<bool> OnGift(int giver, LiteCurrencies gift, string msg);

		public abstract void OnLanding();
		public virtual void OnEndLanding() { }

		public abstract void OnEndTurn();

		public int OnProfit(Id<PlayerId> id, int hexId)
		{
			if (id == PlayerId.Satan)
				_resources.Blood.Add(_edifices.ShrinePassiveProfit);

			if (hexId == HEX.GATE)
			{
				_resources.Blood.Add(_edifices.ShrineProfit);
#if TEST_AI
                return Clamp();
#else
				return _resources.Clamp();
#endif
            }

            if (_abilities.IsTrue(HumanAbilityId.IsFreeGroundRes))
				_resources.Add(GameContainer.Hexagons.FreeResources);

			_resources.Add(_edifices.ProfitFromEdifices(hexId));
#if TEST_AI
			DrawRes();
#endif
            return 0;

#if TEST_AI
            #region Local
            void DrawRes() => UnityEngine.Debug.Log($"[{_id}]  {_resources}");
            int Clamp()
			{
                int delta = _resources.Clamp();
                DrawRes();
                UnityEngine.Debug.Log($"[{_id}] Delta {delta}");
                return delta;
            }
            #endregion
#endif
        }

        public virtual void OnStartTurn()
		{
			foreach (var warrior in Actors)
				warrior.EffectsUpdate();

			_exchange.Update();
		}

		public abstract void OnPlay();

		protected IEnumerator OnEndTurn_Cn()
		{
			int countBuffs = 0;
			int mainProfit = _abilities[HumanAbilityId.WarriorProfit];
			bool isArtefact = _abilities.IsTrue(HumanAbilityId.IsArtefact);
			LiteCurrencies profit = new();
			ReturnSignal returnSignal;

			GameContainer.InputController.Unselect();

			foreach (var warrior in Actors)
			{
				if (!warrior.IsInCombat())
				{
					if (returnSignal = warrior.IsMainProfit)
					{
						profit.Add(warrior.Hexagon.GetProfit(), mainProfit);
						yield return returnSignal.signal;
					}
					if (isArtefact && (returnSignal = warrior.IsAdvProfit))
					{
						countBuffs++;
						yield return returnSignal.signal;
					}
				}

				yield return s_delayHalfSecond.Restart();

				warrior.StatesUpdate();
			}

			_resources.Add(profit);
			_artefact.Next(countBuffs);

			GameContainer.GameLoop.StartTurn();
			_coroutine = null;
		}
	}
}
