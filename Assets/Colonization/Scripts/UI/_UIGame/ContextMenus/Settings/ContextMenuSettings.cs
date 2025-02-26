//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\Settings\ContextMenuSettings.cs
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ContextMenuSettings : ButtonSettings
    {
        public readonly PricesScriptable prices;
        public readonly Camera camera;
        public readonly ITurn turn;
        public readonly GameplayEventBus eventBus;

        public ContextMenuSettings(ITurn turn, Players players, HintGlobal hint, PricesScriptable prices, Camera camera, GameplayEventBus eventBus)
            : base(players.Player, hint)
        {
            this.turn = turn;
            this.prices = prices;
            this.camera = camera;
            this.eventBus = eventBus;
        }
    }
}
