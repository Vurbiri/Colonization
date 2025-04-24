//Assets\Colonization\Scripts\UI\_UIGame\Panels\PlayerPanels.cs
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public partial class PlayerPanels : MonoBehaviour
	{
        [Space]
        [SerializeField] private CurrenciesPanel _currencies;
        [SerializeField] private BloodPanel _blood;
        [Space]
        [SerializeField] private Direction2 _directionPopup;

        public void Create(Players players)
        {
            //var color = SceneContainer.Get<PlayersVisual>()[PlayerId.Player].color.SetAlpha(_transparency);
            var currencies = players.Player.Resources;
            var settings = SceneContainer.Get<ProjectColors>();

            _currencies.Init(_directionPopup, currencies, settings);
            _blood.Init(_directionPopup, currencies, settings);

            Destroy(gameObject);
        }
    }
}
