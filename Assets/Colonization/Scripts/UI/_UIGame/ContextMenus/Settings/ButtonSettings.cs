//Assets\Colonization\Scripts\UI\_UIGame\ContextMenus\Settings\ButtonSettings.cs
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class ButtonSettings
    {
        public readonly Players players;
        public readonly Color color;
        public readonly HintGlobal hint;
        public readonly SettingsTextColor colors;

        public ButtonSettings(Players players, HintGlobal hint)
        {
            this.players = players;
            this.color = SceneData.Get<PlayersVisual>()[PlayerId.Player].color;
            this.hint = hint;
            this.colors = SceneData.Get<SettingsTextColor>();
        }
    }
}
