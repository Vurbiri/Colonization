using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.UI
{
    sealed public class PortsPanel : AEdificesPanel<CurrentMaxAndRateProfit, PortButton>
    {
        public void Init(IdArray<EdificeId, Sprite> sprites)
        {
            var player = GameContainer.Players.Person;
            var edifices = player.GetEdifices(_id);
            var maxEdifices = player.GetAbility(_id.ToState());
            var portsProfit = player.GetAbility(HumanAbilityId.PortsProfitShift);

            InitEdifice(edifices, sprites);
            _widget.Init(edifices.CountReactive, maxEdifices, portsProfit);
        }
    }
}
