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

        public void Init(ACrossroadBuildMenu roadsMenu)
        {
            _players = Players.Instance;

            _buttonClose.onClick.AddListener(() => gameObject.SetActive(false));
            _buttonRoads.onClick.AddListener(OnRoads);
            _buttonWall.onClick.AddListener(OnWall);
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

            if (ButtonSetup(_buttonUpgrade.Button, _playerCurrent.CanCrossroadUpgrade(crossroad), crossroad.CanUpgradeBuy(_playerCurrent.Resources)))
                _buttonUpgrade.SetupHint(crossroad.IdUpgrade, _playerCurrent.Resources, crossroad.CostUpgrade);

            ButtonSetup(_buttonWall, _playerCurrent.CanWallBuild(crossroad), crossroad.CanWallBuy(_playerCurrent.Resources));
            ButtonSetup(_buttonRoads, _playerCurrent.CanRoadBuild(crossroad), _playerCurrent.CanRoadBuy());

            gameObject.SetActive(true);

            #region Local: ButtonSetup(...)
            //=================================
            bool ButtonSetup(CmButton button, bool isEnable, bool isInteractable)
            {
                button.SetActive(isEnable);

                if (isEnable)
                {
                    button.targetGraphic.color = _playerCurrent.Color;
                    button.Interactable = isInteractable;
                }
                                
                return isEnable;
            }
            #endregion
        }
    }
}
