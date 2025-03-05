//Assets\Colonization\Scripts\UI\_UIGame\Button\WarriorButtons\ButtonRecruiting.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.UI
{

    public class ButtonRecruiting : AButtonBuildType<WarriorId>
    {
        public override void Setup(Crossroad crossroad)
        {
            base.Setup(crossroad);
            _button.Interactable = _player.CanRecruitingWarrior(_id) && _cash >= _cost;
        }

        protected override void OnClick()
        {
            _parentGO.SetActive(false);
            _player.RecruitWarriors(_id, _currentCrossroad);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            if (_key == string.Empty)
                _key = WarriorId.GetName(_id);
        }
#endif
    }
}
