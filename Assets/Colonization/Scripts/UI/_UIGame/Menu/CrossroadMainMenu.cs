using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CrossroadMainMenu : ACrossroadMenu
    {
        [SerializeField] private CmButton _buttonClose;
        [SerializeField] private ButtonBuildEdifice _buttonUpgrade;
        [SerializeField] private ButtonBuild _buttonWall;
        [SerializeField] private ButtonBuild _buttonRoads;
        
        private Player _playerCurrent;

        public void Init(ACrossroadBuildMenu roadsMenu)
        {
            _players = Players.Instance;

            _buttonClose.onClick.AddListener(() => gameObject.SetActive(false));
            _buttonRoads.Init();
            _buttonRoads.AddListener(OnRoads);
            _buttonWall.Init();
            _buttonWall.AddListener(OnWall);
            _buttonUpgrade.Init();
            _buttonUpgrade.AddListener(OnUpgrade);

            gameObject.SetActive(false);

            #region Local: OnUpgrade(), OnWall(), OnRoads()
            //=================================
            void OnUpgrade()
            {
                gameObject.SetActive(false);
                _playerCurrent.CrossroadUpgradeBuy(_currentCrossroad);
            }
            //=================================
            void OnWall()
            {
                gameObject.SetActive(false);
                _playerCurrent.CrossroadWallBuy(_currentCrossroad);
            }
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
            _playerCurrent = _players.Current;
            _currentCrossroad = crossroad;

            if (ButtonSetup(_buttonUpgrade, _playerCurrent.CanCrossroadUpgrade(crossroad), crossroad.CanUpgradeBuy(_playerCurrent.Resources)))
                _buttonUpgrade.SetupHint(crossroad.IdUpgrade, _playerCurrent.Resources, crossroad.CostUpgrade);

            if (ButtonSetup(_buttonWall, _playerCurrent.CanWallBuild(crossroad), crossroad.CanWallBuy(_playerCurrent.Resources)))
                _buttonWall.SetupHint(_playerCurrent.Resources, crossroad.CostUpgrade);

            if(ButtonSetup(_buttonRoads, _playerCurrent.CanRoadBuild(crossroad), _playerCurrent.CanRoadBuy()))
                _buttonRoads.SetupHint(_playerCurrent.Resources, _playerCurrent.RoadCost);

            gameObject.SetActive(true);

            #region Local: ButtonSetup(...)
            //=================================
            bool ButtonSetup(AButtonBuild button, bool isEnable, bool isInteractable)
            {
                button.SetActive(isEnable);

                if (isEnable)
                {
                    button.TargetGraphic.color = _playerCurrent.Color;
                    button.Interactable = isInteractable;
                }
                                
                return isEnable;
            }
            #endregion
        }
    }
}
