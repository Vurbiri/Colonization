using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class RoadsPanel : ASinglyPanel<CurrentMax>
    {
        public void Init(Human player, ProjectColors colors, CanvasHint hint)
        {
            _widget.Init(player.Roads.CountReactive, player.GetAbility(HumanAbilityId.MaxRoads), colors, hint);

            Destroy(this);
        }
    }
}
