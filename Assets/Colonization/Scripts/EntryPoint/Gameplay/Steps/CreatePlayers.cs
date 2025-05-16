//Assets\Colonization\Scripts\EntryPoint\Gameplay\Steps\CreatePlayers.cs
using System.Collections;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class CreatePlayers : ALoadingStep
    {
        private readonly GameplayInitObjects _objects;
        private readonly Players.Settings _settings;

        public CreatePlayers(GameplayInitObjects objects, Players.Settings settings) : base("CreatePlayers")
        {
            _objects = objects;
            _settings = settings;
        }

        public override IEnumerator GetEnumerator()
        {
            _objects.diContainer.AddInstance(_objects.players = new Players(_settings, _objects.turnQueue, _objects.storage));

            yield return null;

            _settings.Dispose();

            yield break;
        }
    }
}
