using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.UI
{
    sealed public class ColoniesPanel : AEdificesPanel<CurrentMax, ColonyButton>
    {
        public void Init(IdArray<EdificeId, Sprite> sprites)
        {
            var person = GameContainer.Person;
            var edifices = person.GetEdifices(_id);
            var maxEdifices = person.GetAbility(_id.ToState());

            InitEdifice(edifices, sprites);
            _widget.Init(edifices.CountReactive, maxEdifices);
        }
    }
}
