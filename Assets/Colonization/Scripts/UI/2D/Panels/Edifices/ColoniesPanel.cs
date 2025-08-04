using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.UI
{
    sealed public class ColoniesPanel : AEdificesPanel<CurrentMax, ColonyButton>
    {
        public void Init(IdArray<EdificeId, Sprite> sprites)
        {
            var player = GameContainer.Players.Person;
            var edifices = player.GetEdifices(_id);
            var maxEdifices = player.GetAbility(_id.ToState());

            InitEdifice(edifices, sprites);
            _widget.Init(edifices.CountReactive, maxEdifices);
        }
    }
}
