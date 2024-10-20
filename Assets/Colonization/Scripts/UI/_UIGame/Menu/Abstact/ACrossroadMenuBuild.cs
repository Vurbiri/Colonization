using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public class ACrossroadMenuBuild : ACrossroadMenu
    {
        protected Player _playerCurrent;
        protected Color _currentColor;

        public override void Open(Crossroad crossroad)
        {
            _playerCurrent = _players.Current;
            _currentCrossroad = crossroad;
            _currentColor = _playerCurrent.Color;
        }

        protected bool ButtonSetup(AButtonBuild button, bool isEnable)
        {
            button.SetActive(isEnable);

            if (isEnable)
                button.Color = _currentColor;

            return isEnable;
        }
    }
}
