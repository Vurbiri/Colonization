using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class ColoniesPanel : AEdificesPanel<CurrentMax, ColonyButton>
    {
        public void Init(Human player, IdArray<EdificeId, Sprite> sprites, CanvasHint hint)
        {
            var edifices = player.GetEdifices(_id);
            var maxEdifices = player.GetAbility(_id.ToState());

            InitEdifice(edifices, sprites);
            _widget.Init(edifices.CountReactive, maxEdifices, hint);
        }
    }
}
