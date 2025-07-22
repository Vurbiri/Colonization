using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class ShrinesPanel : AEdificesPanel<CurrentMaxAndProfit, ShrineButton>
    {
        public void Init(Human player, IdArray<EdificeId, Sprite> sprites, CanvasHint hint)
        {
            var edifices = player.GetEdifices(_id);
            var maxEdifices = player.GetAbility(_id.ToState());
            var activeProfit = player.GetAbility(HumanAbilityId.ShrineProfit);
            var passiveProfit = player.GetAbility(HumanAbilityId.ShrinePassiveProfit);

            InitEdifice(edifices, sprites);
            _widget.Init(edifices.CountReactive, maxEdifices, activeProfit, passiveProfit, hint);
        }
    }
}
