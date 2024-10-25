using UnityEngine;
using Vurbiri.Localization;

namespace Vurbiri.Colonization.UI
{
    public class ButtonRecruiting : AButtonBuildType<WarriorId>
    {
        private Players _players;
        private Player _playerCurrent;
        private Crossroad _currentCrossroad;

        public ButtonRecruiting Init(Vector3 localPosition, Players players, ACurrencies cost)
        {
            base.Init(localPosition);
            _players = players;
            _cost = cost;
            _buttonClicked.AddListener(OnClick);
            _unsubscriber = SceneServices.Get<Language>().Subscribe(SetText);
            return this;
        }

        public void Setup(Crossroad crossroadCurrent)
        {
            _playerCurrent = _players.Current;
            _currentCrossroad = crossroadCurrent;
            ACurrencies cash = _playerCurrent.Resources;

            _button.Interactable = _playerCurrent.CanRecruitingWarrior(_id) && cash >= _cost;
            _targetGraphic.color = _playerCurrent.Visual.color;

            SetTextHint(_caption, cash, _cost);
        }

        private void OnClick()
        {
            _playerCurrent.RecruitWarriors(_currentCrossroad, _id);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if(string.Empty == _key)
                _key = WarriorId.Names[_id.Value];
        }
#endif
    }
}
