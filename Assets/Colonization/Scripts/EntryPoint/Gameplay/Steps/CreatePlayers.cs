using System.Collections;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class CreatePlayers : ALocalizationLoadingStep
    {
        private readonly GameplayInitObjects _init;

        public CreatePlayers(GameplayInitObjects objects) : base("PlayersCreationStep")
        {
            _init = objects;
        }

        public override IEnumerator GetEnumerator()
        {
            _init.diContainer.AddInstance(_init.players = new Players(_init.GetPlayersSettings(), _init.game, _init.triggerBus, _init.storage));
            yield return null;
            _init.PlayersSettingsDispose();
        }
    }
}
