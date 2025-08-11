using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.UI
{
    sealed public class ShrinesPanel : AEdificesPanel<CurrentMaxAndProfit, ShrineButton>
    {
        public void Init(IdArray<EdificeId, Sprite> sprites)
        {
            var person = GameContainer.Players.Person;
            var edifices = person.GetEdifices(_id);
            var maxEdifices = person.GetAbility(_id.ToState());
            var activeProfit = person.GetAbility(HumanAbilityId.ShrineProfit);
            var passiveProfit = person.GetAbility(HumanAbilityId.ShrinePassiveProfit);

            InitEdifice(edifices, sprites);
            _widget.Init(edifices.CountReactive, maxEdifices, activeProfit, passiveProfit);
        }
    }
}
