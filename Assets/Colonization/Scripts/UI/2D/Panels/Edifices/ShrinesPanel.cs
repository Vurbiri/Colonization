using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive;
using static Vurbiri.Colonization.HumanAbilityId;

namespace Vurbiri.Colonization.UI
{
    sealed public class ShrinesPanel : AEdificesPanel<CurrentMaxAndProfit, ShrineButton>
    {
        private readonly ReactiveInt _maxShrines = new(0);

        public void Init(IdArray<EdificeId, Sprite> sprites)
        {
            Player.ShrinesCount.Subscribe(SetMaxShrines);

            var person = GameContainer.Person;
            InitEdifice(person.Shrines, sprites);
            _widget.Init(person.Shrines.CountReactive, _maxShrines, person.GetAbility(ShrineProfit), person.GetAbility(ShrinePassiveProfit));
        }

        private void SetMaxShrines(int current)
        {
            var person = GameContainer.Person;
            _maxShrines.Value = person.GetAbility(MaxShrine) + person.Shrines.Count - current;
        }
    }
}
