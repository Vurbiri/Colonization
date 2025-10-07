using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.UI
{
    sealed public class ButtonRecruiting : AButtonBuildType<WarriorId>
    {
        public override void Setup(Crossroad crossroad)
        {
            base.Setup(crossroad);

            InteractableAndUnlock(_cash >= _cost, GameContainer.Players.Person.CanRecruiting(_id));
        }

        protected override void OnClick()
        {
            _parent.Close();
            GameContainer.Players.Person.Recruiting(_id, _currentCrossroad);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            _getText.id = LangFiles.Actors;
            _getText.key = WarriorId.GetName_Ed(_id);
        }
#endif
    }
}
