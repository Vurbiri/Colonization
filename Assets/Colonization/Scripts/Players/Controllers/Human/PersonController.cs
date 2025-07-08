namespace Vurbiri.Colonization
{
    sealed public class PersonController : AHumanController
    {
        public PersonController(Settings settings) : base(PlayerId.Person, settings)
        {
        }

        public override void OnEndLanding() => _edifices.Interactable = false;

        public override void OnPlay()
        {
            _edifices.Interactable = true;
            foreach (var warrior in _actors)
                warrior.IsPersonTurn = true;
        }

        public override void OnEndTurn()
        {
            base.OnEndTurn();

            _edifices.Interactable = false;
        }
    }
}
