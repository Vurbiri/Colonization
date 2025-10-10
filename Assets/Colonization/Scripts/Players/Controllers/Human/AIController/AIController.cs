using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    sealed public partial class AIController : HumanController
    {
        private static readonly AIControllerSettings s_settings;

        private readonly Diplomat _diplomat;
        private readonly Builder _builder;

        static AIController() => s_settings = SettingsFile.Load<AIControllerSettings>();

        public AIController(int playerId, Settings settings) : base(playerId, settings) 
        {
            _diplomat = new(this); _builder = new(this);
        }

        public override WaitResult<bool> OnGift(int giver, MainCurrencies gift, string msg) => _diplomat.Receive(giver, gift);

        public override void OnLanding()
        {
            OnLanding_Cn().Start();

            IEnumerator OnLanding_Cn()
            {
                yield return null;
                //yield return _builder.BuildFirstPort_Cn();
                BuildPort(GameContainer.Crossroads.GetRandomPort());

                GameContainer.GameLoop.EndLanding();
            }
        }

        public override void OnPlay()
        {
            OnPlay_Cn().Start();

            IEnumerator OnPlay_Cn()
            {
                yield return _builder.Appeal_Cn();

                GameContainer.GameLoop.EndTurn();
            }
        }

        public override void OnEndTurn()
        {
            _diplomat.Update();

            OnEndTurn_Cn().Start();
        }

        private bool Exchange(ReadOnlyMainCurrencies needed)
        {
            bool result = _resources >= needed;
            if (!result)
            {
                int blood = _resources[CurrencyId.Blood] >> (_perks.IsAllLearned ? 0 : 1);
                if (blood > s_settings.minExchangeBlood && Chance.Rolling(_resources.PercentBlood))
                {
                    _spellBook.Cast(MilitarySpellId.Type, MilitarySpellId.BloodTrade, new(_id, Random.Range(s_settings.minExchangeBlood, blood)));
                    result = _resources >= needed;
                }

                if(!result && _resources.OverCount(needed, out int exchangeIndex) == 1)
                {
                    int need = needed[exchangeIndex], current = _resources[exchangeIndex], delta = need - current;
                    int exchangeValue = delta * _exchange[exchangeIndex];
                    if (((_resources.Amount - current) - (needed.Amount - need)) >= (exchangeValue + CurrencyId.MainCount))
                    {
                        MainCurrencies pay = new(), diff = _resources - needed;
                        pay.Remove(exchangeIndex, delta);
                        diff.Set(exchangeIndex, 0);

                        int index = Random.Range(0, CurrencyId.MainCount);
                        while (exchangeValue > 0)
                        {
                            if (diff[index] > 0)
                            {
                                diff.Decrement(index);
                                pay.Increment(index);
                                exchangeValue--;
                            }
                            index = (index + 1) % CurrencyId.MainCount;
                        }
                        Debug.Log($"Exchange {pay}");
                        _resources.Remove(pay);
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
