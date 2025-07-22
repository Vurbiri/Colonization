using Vurbiri.Colonization.Controllers;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ContextMenuSettings : ButtonSettings
    {
        public readonly Prices prices;
        public readonly CameraTransform cameraTransform;

        public ContextMenuSettings(Players players, WorldHint hint, CameraTransform camera)
            : base(players.Person, hint)
        {
            prices = GameContainer.Prices;
            cameraTransform = camera;
        }
    }
}
