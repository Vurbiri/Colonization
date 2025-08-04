using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.UI
{
    sealed public class RoadsPanel : ASinglyPanel<CurrentMax>
    {
        public void Init()
        {
            var player = GameContainer.Players.Person;
            _widget.Init(player.Roads.CountReactive, player.GetAbility(HumanAbilityId.MaxRoad));

            Destroy(this);
        }
    }
}
