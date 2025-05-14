//Assets\Colonization\Scripts\UI\_UIGame\Panels\PlayerPanels.cs
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization.UI
{
    public partial class PlayerPanels : MonoBehaviour
	{
        [Space]
        [SerializeField] private IdArray<EdificeGroupId, AEdificesPanel> _edifices;
        [SerializeField] private RoadsPanel _roads;
        [Space]
        [SerializeField] private WarriorsPanel _warriors;
        [Space]
        [SerializeField] private CurrenciesPanel _currencies;
        [SerializeField] private BloodPanel _blood;
        [Space]
        [SerializeField] private Direction2 _directionPopup;
        [Space]
        [SerializeField] private IdArray<EdificeId, Sprite> _sprites;

        public void Init(InputController inputController)
        {
            var player = SceneContainer.Get<Players>().Player;
            var currencies = player.Resources;
            var colors = SceneContainer.Get<ProjectColors>();

            for (int i = 0; i < EdificeGroupId.Count; i++)
                _edifices[i].Init(player, _sprites, colors, inputController);
            _roads.Init(player, colors);

            _warriors.Init(player, colors, inputController);

            _currencies.Init(_directionPopup, currencies, colors);
            _blood.Init(_directionPopup, currencies, colors);


            Destroy(gameObject);
        }
    }
}
