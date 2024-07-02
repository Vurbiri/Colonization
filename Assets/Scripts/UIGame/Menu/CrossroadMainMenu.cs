using UnityEngine;
using UnityEngine.UI;

public class CrossroadMainMenu : ACrossroadMenu
{
    [SerializeField] private CmButton _buttonClose;
    [SerializeField] private CmButton _buttonCity;
    [SerializeField] private CmButton _buttonRoads;
    [Space]
    [SerializeField] private Image _buttonCityIcon;
    [SerializeField] private UnityDictionary<CityBuildType, Sprite> _citySprites;

    private ACrossroadBuildMenu _buildMenu;
    private Hinting _buttonHinting;

    public void Initialize(ACrossroadBuildMenu roadsMenu, ACrossroadBuildMenu buildMenu)
    {
        _players = Players.Instance;

        _buildMenu = buildMenu;

        _buttonHinting = _buttonCity.GetComponent<Hinting>();

        _buttonClose.onClick.AddListener(() => gameObject.SetActive(false));
        _buttonRoads.onClick.AddListener(OnRoads);

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
        CityBuildType cityBuildType = crossroad.CityBuildType;
        _currentCrossroad = crossroad;
        
        _buttonRoads.targetGraphic.color = color;
        _buttonRoads.interactable = player.CanRoadBuilt(crossroad);

        _buttonCity.targetGraphic.color = color;
        _buttonCityIcon.sprite = _citySprites[cityBuildType];
        _buttonHinting.Key = cityBuildType.ToString();
        _buttonCity.onClick.RemoveAllListeners();

        switch (cityBuildType)
        {
            case CityBuildType.Build     : ToMenuBuild(); break;
            case CityBuildType.Upgrade  : CityUpgrade(); break;
            case CityBuildType.Berth:
            case CityBuildType.Port:
            case CityBuildType.Shrine     : CityBuild(cityBuildType.ToCityType()); break;

            default : break;
        }

        gameObject.SetActive(true);

        #region Local: CityUpgrade()
        //=================================
        void ToMenuBuild()
        {
            _buttonCity.interactable = crossroad.CanCityBuild(player.Type, player.Resources);
            _buttonCity.onClick.AddListener(ToMenu);

            #region Local: ToMenu()
            //=================================
            void ToMenu()
            {
                gameObject.SetActive(false);
                _buildMenu.Open(_currentCrossroad);
            }
            #endregion
        }
        //=================================
        void CityUpgrade()
        {
            _buttonCity.interactable = crossroad.CanCityUpgrade(player);
            _buttonCity.onClick.AddListener(OnUpgrade);

            #region Local: OnUpgrade()
            //=================================
            void OnUpgrade()
            {
                gameObject.SetActive(false);
                player.CityUpgrade(_currentCrossroad);
            }
            #endregion
        }
        //=================================
        void CityBuild(CityType cityType)
        {
            _buttonCity.interactable = crossroad.CanBuild(cityType, player.Resources);
            _buttonCity.onClick.AddListener(OnBuild);

            #region Local: OnUpgrade()
            //=================================
            void OnBuild()
            {
                gameObject.SetActive(false);
                player.CityBuild(_currentCrossroad, cityType);
            }
            #endregion
        }
        #endregion
    }
}
