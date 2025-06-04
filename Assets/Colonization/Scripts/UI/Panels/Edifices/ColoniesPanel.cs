using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Controllers;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class ColoniesPanel : AEdificesPanel<CurrentMax, ColonyButton>
    {
        public override void Init(Human player, IdArray<EdificeId, Sprite> sprites, ProjectColors colors, InputController inputController, CanvasHint hint)
        {
            var edifices = player.GetEdifices(_id);
            var maxEdifices = player.GetAbility(_id.ToState());

            InitEdifice(edifices, sprites, inputController);
            _widget.Init(edifices.CountReactive, maxEdifices, colors, hint);
        }
    }
}
