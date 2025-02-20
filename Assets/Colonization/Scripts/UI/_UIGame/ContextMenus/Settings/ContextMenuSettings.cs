//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\Settings\ContextMenuSettings.cs
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ContextMenuSettings : ButtonSettings
    {
        public readonly PricesScriptable prices;
        public readonly Camera camera;
        public readonly Players players;
        public readonly GameplayEventBus eventBus;

        public ContextMenuSettings(Players players, HintGlobal hint, PricesScriptable prices, Camera camera, GameplayEventBus eventBus)
            : base(players.Player, hint)
        {
            this.players = players;
            this.prices = prices;
            this.camera = camera;
            this.eventBus = eventBus;
        }
    }
}
