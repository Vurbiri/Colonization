using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class ShrinesPanel : AEdificesPanel<CurrentMaxAndProfit, ShrineButton>
    {
        public void Init(Human player, IdArray<EdificeId, Sprite> sprites, InputController inputController, CanvasHint hint)
        {
            var edifices = player.GetEdifices(_id);
            var maxEdifices = player.GetAbility(_id.ToState());
            var activeProfit = player.GetAbility(HumanAbilityId.ShrineProfit);
            var passiveProfit = player.GetAbility(HumanAbilityId.ShrinePassiveProfit);

            InitEdifice(edifices, sprites, inputController);
            _widget.Init(edifices.CountReactive, maxEdifices, activeProfit, passiveProfit, hint);
        }
    }
}
