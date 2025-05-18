//Assets\Colonization\Scripts\EntryPoint\Gameplay\Steps\CreatePlayers.cs
using System.Collections;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class CreatePlayers : ALocalizationLoadingStep
    {
        private readonly GameplayInitObjects _objects;

        public CreatePlayers(GameplayInitObjects objects) : base("PlayersCreationStep")
        {
            _objects = objects;
        }

        public override IEnumerator GetEnumerator()
        {
            _objects.diContainer.AddInstance(_objects.players = new Players(_objects.playersSettings, _objects.turnQueue, _objects.storage));
            yield return null;
            _objects.playersSettings.Dispose();
            _objects.playersSettings = null;

            yield break;
        }
    }
}
