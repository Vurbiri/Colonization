namespace Vurbiri.Colonization.UI
{
    public class ButtonRecruiting : AButtonBuildType<WarriorId>
    {
        private Player _playerCurrent;
        private Crossroad _currentCrossroad;

        public void Setup(Crossroad crossroadCurrent)
        {
            _playerCurrent = _players.Current;
            _currentCrossroad = crossroadCurrent;
            ACurrencies cash = _playerCurrent.Resources;

            _button.Interactable = _playerCurrent.CanRecruitingWarrior(_id) && cash >= _cost;
            _targetGraphic.color = _playerCurrent.Visual.color;

            SetTextHint(_caption, cash, _cost);
        }

        protected override void OnClick()
        {
            _parentGO.SetActive(false);
            _playerCurrent.RecruitWarriors(_currentCrossroad, _id);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            
            if(string.Empty == _key)
                _key = WarriorId.GetName(_id);
        }
#endif
    }
}
