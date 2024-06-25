using UnityEngine;

public class BuildingMainMenu : MonoBehaviour
{
    [SerializeField] private CmButton _buttonClose;
    [SerializeField] private CmButton _buttonCity;
    [SerializeField] private CmButton _buttonRoads;

    private Players _players;
    private Crossroad _currentCrossroad;

    public void Initialize(BuildingRoadsMenu roadsMenu)
    {
        _players = Players.Instance;

        _buttonClose.onClick.AddListener(() => gameObject.SetActive(false));
        _buttonCity.onClick.AddListener(OnCity);
        _buttonRoads.onClick.AddListener(OnRoads);

        gameObject.SetActive(false);

        #region Local: OnRoads()
        //=================================
        void OnCity()
        {
            gameObject.SetActive(false);
            _players.Current.BuildCity(_currentCrossroad);
        }
        void OnRoads()
        {
            gameObject.SetActive(false);
            roadsMenu.Open(_currentCrossroad);
        }
        #endregion
    }

    public void Open(Crossroad crossroad)
    {
        Player player = _players.Current;
        _currentCrossroad = crossroad;

        _buttonCity.targetGraphic.color = player.Color;
        _buttonCity.interactable = player.CanCityBuilt(crossroad);

        _buttonRoads.targetGraphic.color = player.Color;
        _buttonRoads.interactable = player.CanRoadBuilt(crossroad);

        gameObject.SetActive(true);
    }

    public void Open() => gameObject.SetActive(true);

    public void Close() => gameObject.SetActive(false);

}
