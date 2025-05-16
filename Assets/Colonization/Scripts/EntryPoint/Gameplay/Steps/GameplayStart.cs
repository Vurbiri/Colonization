//Assets\Colonization\Scripts\EntryPoint\Gameplay\Steps\GameplayStart.cs
using System.Collections;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.Storage;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class GameplayStart : ALoadingStep
    {
        private readonly GameLoop _game;
        private readonly InputController _inputController;
        private readonly TurnQueue _turnQueue;
        private readonly GameplayStorage _storage;

        public GameplayStart(GameplayInitObjects objects) : base("GameplayStart")
        {
            _game = objects.game;
            _inputController = objects.inputController;
            _turnQueue = objects.turnQueue;
            _storage = objects.storage;
        }

        public override IEnumerator GetEnumerator()
        {
            _game.Init(_turnQueue, _inputController);

            yield return null;

            _storage.Save();
        }
    }
}
