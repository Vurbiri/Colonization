//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\Settings\ContextMenuSettings.cs
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ContextMenuSettings : ButtonSettings
    {
        public readonly Prices prices;
        public readonly Camera camera;
        public readonly TurnQueue turn;
        public readonly GameplayEventBus eventBus;

        public ContextMenuSettings(TurnQueue turn, Players players, WorldHint hint, Prices prices, Camera camera, GameplayEventBus eventBus)
            : base(players.Player, hint)
        {
            this.turn = turn;
            this.prices = prices;
            this.camera = camera;
            this.eventBus = eventBus;
        }
    }
}
