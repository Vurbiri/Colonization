//Assets\Colonization\Scripts\UI\_UIGame\Panels\PlayerPanels.cs
using UnityEngine;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization.UI
{
    public partial class PlayerPanels : MonoBehaviour
	{
        [Space]
        [SerializeField] private WarriorsPanel _warriors;
        [Space]
        [SerializeField] private CurrenciesPanel _currencies;
        [SerializeField] private BloodPanel _blood;
        [Space]
        [SerializeField] private Direction2 _directionPopup;

        public void Init(InputController inputController)
        {
            var player = SceneContainer.Get<Players>().Player;
            var currencies = player.Resources;
            var colors = SceneContainer.Get<ProjectColors>();

            _warriors.Init(player, colors, inputController);
            _currencies.Init(_directionPopup, currencies, colors);
            _blood.Init(_directionPopup, currencies, colors);

            Destroy(gameObject);
        }
    }
}
