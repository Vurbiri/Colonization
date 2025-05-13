//Assets\Colonization\Scripts\UI\_UIGame\Panels\ShrinesPanel.cs
using TMPro;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization.UI
{
    sealed public class ShrinesPanel : AEdificesPanel
    {
        [Space]
        [SerializeField] private TextMeshProUGUI _profit;
        [SerializeField] private TextMeshProUGUI _passiveProfit;

        public override void Init(Human player, IdArray<EdificeId, Sprite> sprites, ProjectColors colors, InputController inputController)
        {
            
            base.Init(player, sprites, colors, inputController);
        }
    }
}
