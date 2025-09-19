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
        [SerializeField] private ScorePanel _score;
        [Space]
        [SerializeField] private ArtefactPanel _artefactPanel;
        [Space]
        [SerializeField] private Direction2 _directionPopup;
        [Space]
        [SerializeField] private IdArray<EdificeId, Sprite> _sprites;

        public void Init()
        {
            var currencies = GameContainer.Players.Person.Resources;

            _warriors.Init();

            _colonies.Init(_sprites);
            _ports.Init(_sprites);
            _shrines.Init(_sprites);
            _roads.Init();

            Vector3 directionPopup = _directionPopup;
            _currencies.Init(directionPopup, currencies);
            _blood.Init(directionPopup, currencies);

            _score.Init(directionPopup);

            _artefactPanel.Init();

            Destroy(this);
        }
    }
}
