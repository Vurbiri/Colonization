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
            _init.content.players = new Players(_init.GetPlayerSettings(), _init.content.gameLoop);
            yield return null;
        }
    }
}
