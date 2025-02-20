//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\Settings\ButtonSettings.cs
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ButtonSettings
    {
        public readonly Player player;
        public readonly Color playerColor;
        public readonly HintGlobal hint;
        public readonly TextColorSettings colorSettings;

        public ButtonSettings(Player player, HintGlobal hint)
        {
            this.player = player;
            this.hint = hint;
            playerColor = SceneData.Get<PlayersVisual>()[PlayerId.Player].color;
            colorSettings = SceneData.Get<TextColorSettings>();
        }
    }
}
