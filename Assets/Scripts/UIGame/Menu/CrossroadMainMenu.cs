using UnityEngine;

public class CrossroadMainMenu : ACrossroadMenu
{
    [SerializeField] private CmButton _buttonClose;
    [SerializeField] private CmButton _buttonCity;
    [SerializeField] private CmButton _buttonRoads;

    public override void Initialize(ACrossroadMenu roadsMenu)
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
            _players.Current.CityUpgrade(_currentCrossroad);
        }
        void OnRoads()
        {
            gameObject.SetActive(false);
            roadsMenu.Open(_currentCrossroad);
        }
        #endregion
    }

    public override void Open(Crossroad crossroad)
    {
        Player player = _players.Current;
        Color color = player.Color;
        _currentCrossroad = crossroad;

        _buttonCity.targetGraphic.color = color;
        _buttonCity.interactable = player.CanCityUpgrade(crossroad);

        _buttonRoads.targetGraphic.color = color;
        _buttonRoads.interactable = player.CanRoadBuilt(crossroad);

        gameObject.SetActive(true);
    }
}
