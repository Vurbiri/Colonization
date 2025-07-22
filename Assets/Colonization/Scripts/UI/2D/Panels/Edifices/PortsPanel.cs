using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class PortsPanel : AEdificesPanel<CurrentMaxAndRateProfit, PortButton>
    {
        public void Init(Human player, IdArray<EdificeId, Sprite> sprites, CanvasHint hint)
        {
            var edifices = player.GetEdifices(_id);
            var maxEdifices = player.GetAbility(_id.ToState());
            var portsProfit = player.GetAbility(HumanAbilityId.PortsProfitShift);

            InitEdifice(edifices, sprites);
            _widget.Init(edifices.CountReactive, maxEdifices, portsProfit, hint);
        }
    }
}
