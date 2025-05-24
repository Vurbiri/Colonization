using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ContextMenuSettings : ButtonSettings
    {
        public readonly GameEvents game;
        public readonly Prices prices;
        public readonly Camera camera;
        public readonly GameplayEventBus eventBus;

        public ContextMenuSettings(GameEvents game, Players players, WorldHint hint, Prices prices, Camera camera, GameplayEventBus eventBus)
            : base(players.Player, hint)
        {
            this.game = game;
            this.prices = prices;
            this.camera = camera;
            this.eventBus = eventBus;
        }
    }
}
