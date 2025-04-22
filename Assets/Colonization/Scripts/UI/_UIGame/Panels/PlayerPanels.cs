//Assets\Colonization\Scripts\UI\_UIGame\Panels\PlayerPanels.cs
using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public partial class PlayerPanels : MonoBehaviour
	{
        [Space]
        [SerializeField] private CurrenciesPanel _currencies;
        [SerializeField] private BloodPanel _blood;
        [Space]
        [SerializeField, Range(0f, 1f)] private float _transparency = 0.84f;
        [Space]
        [SerializeField] private Direction2 _directionPopup;

        public void Create(Players players)
        {
            var color = SceneContainer.Get<PlayersVisual>()[PlayerId.Player].color.SetAlpha(_transparency);
            var currencies = players.Player.Resources;
            var settings = SceneContainer.Get<TextColorSettings>();

            _currencies.Init(color, _directionPopup, currencies, settings);
            _blood.Init(color, _directionPopup, currencies, settings);

            Destroy(gameObject);
        }
    }
}
