using UnityEngine;
using Vurbiri.Collections;
using static Vurbiri.Colonization.Characteristics.HumanAbilityId;

namespace Vurbiri.Colonization.UI
{
    sealed public class ShrinesPanel : AEdificesPanel<CurrentMaxAndProfit, ShrineButton>
    {
        public void Init(IdArray<EdificeId, Sprite> sprites)
        {
            var person = GameContainer.Players.Person;

            InitEdifice(person.Shrines, sprites);
            _widget.Init(person.Shrines.CountReactive, person.GetAbility(MaxShrine), person.GetAbility(ShrineProfit), person.GetAbility(ShrinePassiveProfit));
        }
    }
}
