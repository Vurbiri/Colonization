//Assets\Colonization\Scripts\UI\_UIGame\Panels\RoadsPanel.cs
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.UI
{
    sealed public class RoadsPanel : ASinglyPanel<CurrentMax>
    {
        public void Init(Human player, ProjectColors colors)
        {
            _widget.Init(player.Roads.CountReactive, player.GetAbility(HumanAbilityId.MaxRoads), colors);

            Destroy(this);
        }
    }
}
