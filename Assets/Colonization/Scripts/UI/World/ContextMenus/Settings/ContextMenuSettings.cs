using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ContextMenuSettings : ButtonSettings
    {
        public readonly Game game;
        public readonly Prices prices;
        public readonly Camera camera;
        public readonly CameraTransform cameraTransform;
        public readonly GameplayEventBus eventBus;

        public ContextMenuSettings(Game game, Players players, WorldHint hint, Prices prices, CameraTransform cameraTransform, GameplayEventBus eventBus)
            : base(players.Player, hint)
        {
            this.game = game;
            this.prices = prices;
            this.cameraTransform = cameraTransform;
            this.eventBus = eventBus;

            camera = cameraTransform.Camera;
        }
    }
}
