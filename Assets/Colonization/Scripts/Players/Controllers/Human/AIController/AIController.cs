using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization
{
    sealed public partial class AIController : HumanController
    {
        private static readonly AIControllerSettings s_settings;

        private readonly int _specialization = AbilityTypeId.Economic;

        private readonly Diplomat _diplomat;
        private readonly Builder _builder;
        private readonly Scientist _scientist;

        static AIController() => s_settings = SettingsFile.Load<AIControllerSettings>();

        public AIController(int playerId, Settings settings) : base(playerId, settings) 
        {
            if (s_settings.militarist == playerId)
                _specialization = AbilityTypeId.Military;

            _diplomat = new(this); _builder = new(this); _scientist = new(this);
        }

        public override WaitResult<bool> OnGift(int giver, MainCurrencies gift, string msg) => _diplomat.Receive(giver, gift);

        public override void OnLanding()
        {
            StartCoroutine(OnLanding_Cn());

            IEnumerator OnLanding_Cn()
            {
                yield return null;
                yield return _builder.Init_Cn();
                yield return _scientist.Init_Cn();
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

            IEnumerator OnPlay_Cn()
            {
                yield return s_settings.waitPlay.Restart();
               
                yield return _builder.Execution_Cn();
                yield return _scientist.Execution_Cn();

                _interactable.False();

                GameContainer.GameLoop.EndTurn();
            }
        }

        public override void OnEndTurn()
        {
            _diplomat.Update();

            StartCoroutine(OnEndTurn_Cn());
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
                    int exchangeValue = delta * _exchange[exchangeIndex];
                    if (((_resources.Amount - current) - (needed.Amount - need)) >= (exchangeValue + s_settings.exchangeValueOffset))
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
            output.Write(result);
            yield break;
        }
    }
}
