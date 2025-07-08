using System.Collections;
using Vurbiri.Colonization.Controllers;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class GameplayStart : ALoadingStep
    {
        private readonly GameLoop _game;
        private readonly GameSettings _gameState;
        private readonly InputController _inputController;
        
        public GameplayStart(GameplayInitObjects init) : base(string.Empty)
        {
            _game = init.game;
            _gameState = init.gameSettings;
            _inputController = init.inputController;
        }

        public override IEnumerator GetEnumerator()
        {
            yield return null;
            _gameState.Start();
            _inputController.Enable();
            
            yield return _game.Start();
        }
    }
}
