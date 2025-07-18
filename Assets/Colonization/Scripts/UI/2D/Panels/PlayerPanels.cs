using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Controllers;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public partial class PlayerPanels : MonoBehaviour
	{
        [SerializeField] private WarriorsPanel _warriors;
        [Space]
        [SerializeField] private ColoniesPanel _colonies;
        [SerializeField] private PortsPanel _ports;
        [SerializeField] private ShrinesPanel _shrines;
        [SerializeField] private RoadsPanel _roads;
        [Space]
        [SerializeField] private CurrenciesPanel _currencies;
        [SerializeField] private BloodPanel _blood;
        [Space]
        [SerializeField] private ArtefactPanel _artefactPanel;
        [Space]
        [SerializeField] private Direction2 _directionPopup;
        [Space]
        [SerializeField] private IdArray<EdificeId, Sprite> _sprites;

        public void Init(Human player, ProjectColors colors, InputController inputController, CanvasHint hint)
        {
            var currencies = player.Resources;

            _warriors.Init(player, inputController, hint);

            _colonies.Init(player, _sprites, inputController, hint);
            _ports.Init(player, _sprites, inputController, hint);
            _shrines.Init(player, _sprites, inputController, hint);
            _roads.Init(player, hint);

            _currencies.Init(_directionPopup, currencies, colors, hint);
            _blood.Init(_directionPopup, currencies, colors, hint);

            _artefactPanel.Init(player, hint);

            Destroy(this);
        }
    }
}
