using Vurbiri.Colonization.Characteristics;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class RoadsPanel : ASinglyPanel<CurrentMax>
    {
        public void Init(Human player, CanvasHint hint)
        {
            _widget.Init(player.Roads.CountReactive, player.GetAbility(HumanAbilityId.MaxRoad), hint);

            Destroy(this);
        }
    }
}
