namespace Vurbiri.Colonization
{
    sealed public class PlayerController : AHumanController
    {
        public PlayerController(Storage.HumanStorage[] storages, Players.Settings settings) : base(PlayerId.Player, storages[PlayerId.Player], settings)
        {
        }

        public override void OnEndLanding() => _edifices.Interactable = false;

        public override void OnPlay()
        {
            _edifices.Interactable = true;
            foreach (var warrior in _warriors)
                warrior.IsPlayerTurn = true;
        }

        public override void OnEndTurn()
        {
            base.OnEndTurn();

            _edifices.Interactable = false;
        }
    }
}
