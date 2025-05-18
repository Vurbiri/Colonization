//Assets\Colonization\Scripts\UI\_UIGame\Panels\ShrinesPanel.cs
using TMPro;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class ShrinesPanel : AEdificesPanel
    {
        private const string PROFIT = "{0} {1}";

        [Space]
        [SerializeField] private TextMeshProUGUI _profitTMP;

        private ReactiveCombination<int, int> _reactiveProfit;

        public override void Init(Human player, IdArray<EdificeId, Sprite> sprites, ProjectColors colors, InputController inputController, CanvasHint hint)
        {
            _reactiveProfit = new(player.GetAbility(HumanAbilityId.ShrineProfit), player.GetAbility(HumanAbilityId.ShrinePassiveProfit), SetProfit);

            base.Init(player, sprites, colors, inputController, hint);
        }

        private void SetProfit(int profit, int passiveProfit)
        {
            _profitTMP.text = string.Format(PROFIT, profit, passiveProfit);
        }

        private void OnDestroy()
        {
            _reactiveProfit.Dispose();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (_profitTMP == null)
                _profitTMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(this, "ProfitTMP");
        }
#endif
    }
}
