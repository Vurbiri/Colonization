using UnityEngine;

public class CrossroadMainMenu : ACrossroadMenu
{
    [SerializeField] private CmButton _buttonClose;
    [SerializeField] private ButtonUpgrade _buttonUpgrade;
    [SerializeField] private CmButton _buttonRoads;
    [Space]
    //[SerializeField] private UnityDictionary<EdificeType, Sprite> _edificeSprites;
    [SerializeField] private EnumArray<EdificeType, Sprite> _edificeSprites;

    public void Initialize(ACrossroadBuildMenu roadsMenu)
    {
        _players = Players.Instance;

        _buttonClose.onClick.AddListener(() => gameObject.SetActive(false));
        _buttonRoads.onClick.AddListener(OnRoads);

        _buttonUpgrade.Initialize();

        gameObject.SetActive(false);

        #region Local: OnRoads()
        //=================================
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
        EdificeType upgradeType = crossroad.UpgradeType;
        bool isUpgrade = upgradeType != EdificeType.None;
        _currentCrossroad = crossroad;
        
        _buttonRoads.targetGraphic.color = color;
        _buttonRoads.interactable = player.CanRoadBuilt(crossroad);

        if(_buttonUpgrade.Setup(upgradeType, _edificeSprites[upgradeType], color, OnUpgrade))
            _buttonUpgrade.Interactable = crossroad.CanCityUpgrade(player);

        gameObject.SetActive(true);

        #region Local: OnUpgrade()
        //=================================
        void OnUpgrade()
        {
            gameObject.SetActive(false);
            player.CityUpgrade(_currentCrossroad);
        }
        #endregion
    }
}
