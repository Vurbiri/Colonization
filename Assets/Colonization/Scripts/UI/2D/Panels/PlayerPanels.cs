using UnityEngine;
using Vurbiri.Collections;

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

        public void Init()
        {
            var player = GameContainer.Players.Person;
            var currencies = player.Resources;
            var colors = GameContainer.UI.Colors;
            var hint = GameContainer.UI.CanvasHint;

            _warriors.Init(player, hint);

            _colonies.Init(player, _sprites, hint);
            _ports.Init(player, _sprites, hint);
            _shrines.Init(player, _sprites, hint);
            _roads.Init(player, hint);

            _currencies.Init(_directionPopup, currencies, colors, hint);
            _blood.Init(_directionPopup, currencies, colors, hint);

            _artefactPanel.Init(player, hint);

            Destroy(this);
        }
    }
}
