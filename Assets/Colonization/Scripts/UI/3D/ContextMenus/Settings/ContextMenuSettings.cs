using UnityEngine;
using Vurbiri.Colonization.Controllers;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ContextMenuSettings : ButtonSettings
    {
        public readonly GameLoop game;
        public readonly Prices prices;
        public readonly Camera camera;
        public readonly CameraTransform cameraTransform;
        public readonly GameplayEventBus eventBus;

        public ContextMenuSettings(GameLoop game, Players players, WorldHint hint, CameraTransform cameraTransform, GameplayEventBus eventBus)
            : base(players.Person, hint)
        {
            this.game = game;
            this.cameraTransform = cameraTransform;
            this.eventBus = eventBus;

            prices = Player.States.prices;
            camera = cameraTransform.Camera;
        }
    }
}
