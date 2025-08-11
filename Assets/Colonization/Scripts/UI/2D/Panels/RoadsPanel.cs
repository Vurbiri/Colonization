using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.UI
{
    sealed public class RoadsPanel : ASinglyPanel<CurrentMax>
    {
        public void Init()
        {
            var person = GameContainer.Players.Person;
            _widget.Init(person.Roads.CountReactive, person.GetAbility(HumanAbilityId.MaxRoad));

            Destroy(this);
        }
    }
}
