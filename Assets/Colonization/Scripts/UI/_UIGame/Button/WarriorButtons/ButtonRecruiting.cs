using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonRecruiting : AButtonBuildType<WarriorId>
    {
        public override void Setup(Crossroad crossroad)
        {
            base.Setup(crossroad);
            interactable = _player.CanRecruiting(_id) && _cash >= _cost;
        }

        protected override void OnClick()
        {
            _parent.Close();
            _player.Recruiting(_id, _currentCrossroad);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            _getText.id = Files.Actors;
            _getText.key = WarriorId.GetName(_id);
        }
#endif
    }
}
