using UnityEngine;

public class CrossroadMainMenu : ACrossroadMenu
{
    [SerializeField] private CmButton _buttonClose;
    [SerializeField] private ButtonBuildUniversal _buttonUniversal;
    [SerializeField] private CmButton _buttonRoads;

    private ACrossroadBuildMenu _buildMenu;

    public void Initialize(ACrossroadBuildMenu roadsMenu, ACrossroadBuildMenu buildMenu)
    {
        _players = Players.Instance;

        _buildMenu = buildMenu;

        _buttonClose.onClick.AddListener(() => gameObject.SetActive(false));
        _buttonRoads.onClick.AddListener(OnRoads);

        _buttonUniversal.Initialize();

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
        CityBuildType cityBuildType = crossroad.BuildType;
        _currentCrossroad = crossroad;
        
        _buttonRoads.targetGraphic.color = color;
        _buttonRoads.interactable = player.CanRoadBuilt(crossroad);

        _buttonUniversal.Setup(crossroad, color);
        switch (cityBuildType)
        {
            case CityBuildType.Build : ToMenuBuild(); break;
            case CityBuildType.Upgrade : CityUpgrade(); break;
            case CityBuildType.Berth or CityBuildType.Port or CityBuildType.Shrine: CityBuild(cityBuildType.ToCityType()); break;
            default : break;
        }

        gameObject.SetActive(true);

        #region Local: CityUpgrade()
        //=================================
        void ToMenuBuild()
        {
            _buttonUniversal.Interactable = crossroad.CanCityBuild(player.Type, player.Resources);
            _buttonUniversal.AddListener(ToMenu);

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
            _buttonUniversal.Interactable = crossroad.CanCityUpgrade(player);
            _buttonUniversal.AddListener(OnUpgrade);

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
            _buttonUniversal.Interactable = crossroad.CanBuild(player.Type, cityType, player.Resources);
            _buttonUniversal.AddListener(OnBuild);

            #region Local: OnBuild()
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
