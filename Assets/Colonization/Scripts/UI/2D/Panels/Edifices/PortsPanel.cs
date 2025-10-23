using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.UI
{
    sealed public class PortsPanel : AEdificesPanel<CurrentMaxAndRateProfit, PortButton>
    {
        public void Init(IdArray<EdificeId, Sprite> sprites)
        {
            var person = GameContainer.Players.Person;
            var edifices = person.GetEdifices(_id);
            var maxEdifices = person.GetAbility(_id.ToState());
            var portsProfit = person.GetAbility(HumanAbilityId.PortsProfitShift);

            InitEdifice(edifices, sprites);
            _widget.Init(edifices.CountReactive, maxEdifices, portsProfit);
        }
    }
}
