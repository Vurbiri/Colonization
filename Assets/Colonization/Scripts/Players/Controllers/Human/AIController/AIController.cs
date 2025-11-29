using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    sealed public partial class AIController : HumanController
    {
        private static readonly AIControllerSettings s_settings;
        
        private readonly Counselors _counselors;
        private readonly WaitAll _waitAll;
        private readonly int _specialization = AbilityTypeId.Economic;

        static AIController() => s_settings = SettingsFile.Load<AIControllerSettings>();

        public AIController(int playerId, Settings settings) : base(playerId, settings, false) 
        {
            if (s_settings.militarist == playerId)
                _specialization = AbilityTypeId.Military;

            _counselors = new(this);
            _waitAll = new(GameContainer.Shared);
        }

        public override WaitResult<bool> OnGift(int giver, MainCurrencies gift, string msg) => _counselors.GiftReceive(giver, gift);

        public override void OnLanding()
        {
            StartCoroutine(OnLanding_Cn());

            // ======= Local ==========
            IEnumerator OnLanding_Cn()
            {
                _resources.AddBlood(_id.Value);
                
                yield return s_settings.waitPlayStart.Restart();
                yield return _counselors.Landing_Cn();
                //BuildPort(GameContainer.Crossroads.GetRandomPort());
                
                GameContainer.GameLoop.EndLanding();
            }
        }

        public override void OnStartTurn()
        {
            base.OnStartTurn();

            _interactable.True();
        }

        public override void OnPlay()
        {
            StartCoroutine(OnPlay_Cn());

            // ======= Local ==========
            IEnumerator OnPlay_Cn()
            {
                if (_resources.PercentAmount < s_settings.minPercentRes)
                    _resources.AddToMin(s_settings.addRes);

                yield return s_settings.waitPlayStart.Restart();
                yield return _waitAll.Add(s_settings.waitPlay.Restart(), _counselors.Execution_Cn());

                _interactable.False();
#if TEST_AI
                Log.Info("===================================================");
#endif

                GameContainer.GameLoop.EndTurn();
            }
        }

        public override void OnEndTurn()
        {
            _counselors.Update();

            StartCoroutine(OnEndTurn_Cn());
        }

        public override void Dispose()
        {
            base.Dispose();
            _waitAll.Dispose();
            _counselors.Dispose();
        }

        private IEnumerator Exchange_Cn(ReadOnlyMainCurrencies needed, Out<bool> output)
        {
            bool result = _resources >= needed;

            if (!result)
            {
                int exchangeBlood = _resources[CurrencyId.Blood] >> (_perks.IsAllLearned() ? 0 : 1);
                if (exchangeBlood > s_settings.minExchangeBlood && Chance.Rolling(_resources.PercentBlood - s_settings.percentBloodOffset))
                {
                    _spellBook.Cast(MilitarySpellId.Type, MilitarySpellId.BloodTrade, new(_id, Random.Range(s_settings.minExchangeBlood, exchangeBlood + 1)));
                    result = _resources >= needed;
                }

                yield return null;

                if (!result && _resources.OverCount(needed, out int exchangeIndex) == 1)
                {
                    int need = needed[exchangeIndex], current = _resources[exchangeIndex], delta = need - current;
                    int exchange = _exchange[exchangeIndex], exchangeValue = delta * exchange;
                    if (((_resources.Amount - current) - (needed.Amount - need)) > exchangeValue && Chance.Rolling((int)(6.251f * (20 - exchange * exchange))))
                    {
                        MainCurrencies pay = new(), diff = _resources - needed;
                        pay.Remove(exchangeIndex, delta);
                        diff.Set(exchangeIndex, 0);

                        int index;
                        while (exchangeValue > 0)
                        {
                            index = diff.MaxIndex;
                            diff.Decrement(index);
                            pay.Increment(index);
                            exchangeValue--;

                            yield return null;
                        }
                        _resources.Remove(pay);
                        result = true;
                    }
                }
            }
            output?.Set(result);
            yield break;
        }
    }
}
