using System.Collections;
using Vurbiri.Colonization.Controllers;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class GameplayStart : ALoadingStep
    {
        private readonly Game _game;
        private readonly GameState _gameState;
        private readonly InputController _inputController;
        
        public GameplayStart(GameplayInitObjects init) : base(string.Empty)
        {
            _game = init.game;
            _gameState = init.gameState;
            _inputController = init.inputController;
        }

        public override IEnumerator GetEnumerator()
        {
            yield return null;
            _gameState.Start();
            _inputController.Enable();
            _game.Start();
        }
    }
}
