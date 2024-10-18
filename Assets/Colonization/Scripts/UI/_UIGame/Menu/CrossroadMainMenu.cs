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

        private PricesScriptable _prices;

        private Player _playerCurrent;

        public void Init(ACrossroadMenu roadsMenu, PricesScriptable prices)
        {
            _players = SceneObjects.Get<Players>();

            _prices = prices;

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
                _playerCurrent.EdificeUpgradeBuy(_currentCrossroad);
            }
            //=================================
            void OnWall()
            {
                gameObject.SetActive(false);
                _playerCurrent.EdificeWallBuy(_currentCrossroad);
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

            if (ButtonSetup(_buttonUpgrade, _playerCurrent.CanEdificeUpgrade(crossroad)))
                _buttonUpgrade.SetupHint(crossroad.NextId, _playerCurrent.Resources, _prices.Edifices);

            if (ButtonSetup(_buttonWall, _playerCurrent.CanWallBuild(crossroad)))
                _buttonWall.SetupHint(_playerCurrent.Resources, _prices.Wall);

            if(ButtonSetup(_buttonRoads, _playerCurrent.CanRoadBuild(crossroad)))
                _buttonRoads.SetupHint(_playerCurrent.Resources, _prices.Road);

            gameObject.SetActive(true);

            #region Local: ButtonSetup(...)
            //=================================
            bool ButtonSetup(AButtonBuild button, bool isEnable)
            {
                button.SetActive(isEnable);

                if (isEnable)
                    button.TargetGraphic.color = _playerCurrent.Color;

                return isEnable;
            }
            #endregion
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (_prices == null)
                _prices = VurbiriEditor.Utility.FindAnyScriptable<PricesScriptable>();
        }
#endif
    }
}
