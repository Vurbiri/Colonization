using System.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    sealed public class PersonController : HumanController
    {
        public PersonController(Id<PlayerId> playerId, Settings settings) : base(playerId, settings, true)
        {
            _abilities.ReplaceToDependent(HumanAbilityId.MaxShrine, s_shrinesCount, _edifices.shrines.CountReactive);
        }

        public override WaitResult<bool> OnGift(int giver, MainCurrencies gift, string msg)
        {
            StartCoroutine(Gift_Cn(giver, gift, msg));
            return _waitGift.Restart();

            // Local
            IEnumerator Gift_Cn(int giver, MainCurrencies gift, string msg)
            {
                yield return MessageBox.Open(msg, out WaitButton wait, MBButton.OkNo);

                bool result = wait.Id == MBButtonId.Ok;
                if (result)
                {
                    _resources.Add(gift);
                    GameContainer.Diplomacy.Gift(_id, giver);
                }

                _waitGift.Set(result);
            }
        }

        public override void OnEndLanding()
        {
            _edifices.Interactable = false;
        }

        public override void OnPlay()
        {
            _edifices.Interactable = true;
        }

        public override void OnEndTurn()
        {
            _edifices.Interactable = false;

            StartCoroutine(OnEndTurn_Cn());
        }
    }
}
