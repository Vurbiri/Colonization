using UnityEngine;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    public class CrossroadMainMenu : ACrossroadMenu
    {
        [SerializeField] private CmButton _buttonClose;
        [SerializeField] private ButtonUpgrade _buttonUpgrade;
        [SerializeField] private CmButton _buttonWall;
        [SerializeField] private CmButton _buttonRoads;
        
        private Player _playerCurrent;

        public void Initialize(ACrossroadBuildMenu roadsMenu)
        {
            _players = Players.Instance;

            _buttonClose.onClick.AddListener(() => gameObject.SetActive(false));
            _buttonRoads.onClick.AddListener(OnRoads);
            _buttonWall.onClick.AddListener(OnWall);
            _buttonUpgrade.Initialize();
            _buttonUpgrade.AddListener(OnUpgrade);

            gameObject.SetActive(false);

            #region Local: OnUpgrade(), OnWall(), OnRoads()
            //=================================
            void OnUpgrade()
            {
                gameObject.SetActive(false);
                _playerCurrent.CrossroadUpgrade(_currentCrossroad);
            }
            //=================================
            void OnWall()
            {
                gameObject.SetActive(false);
                _playerCurrent.CrossroadWall(_currentCrossroad);
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

            Color color = _playerCurrent.Color;
            PlayerType type = _playerCurrent.Type;
            bool isActive;

            if (isActive = ButtonSetup(_buttonUpgrade.Button, crossroad.CanUpgrade(type), crossroad.CanUpgradeBuy(_playerCurrent.Resources)))
                _buttonUpgrade.SetupHint(crossroad.UpgradeType);

            isActive = ButtonSetup(_buttonWall, crossroad.CanWallBuild(type), crossroad.CanWallBuy(_playerCurrent.Resources)) || isActive;
            isActive = ButtonSetup(_buttonRoads, crossroad.CanRoadBuild(type), _playerCurrent.CanRoadBuy()) || isActive;

            gameObject.SetActive(isActive);

            #region Local: ButtonSetup(...)
            //=================================
            bool ButtonSetup(CmButton button, bool isEnable, bool isInteractable)
            {
                button.SetActive(isEnable);

                if (isEnable)
                {
                    button.targetGraphic.color = color;
                    button.Interactable = isInteractable;
                }
                                
                return isEnable;
            }
            #endregion
        }
    }
}
