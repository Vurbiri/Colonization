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
        public readonly HintTextColor hintColors;

        public ButtonSettings(Players players, HintGlobal hint, HintTextColor hintColors)
        {
            this.players = players;
            this.color = SceneData.Get<PlayersVisual>()[PlayerId.Player].color;
            this.hint = hint;
            this.hintColors = hintColors;
        }
    }
}
