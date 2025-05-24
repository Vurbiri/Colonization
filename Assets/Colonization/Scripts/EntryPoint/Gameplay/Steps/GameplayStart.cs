using System.Collections;
using Vurbiri.Colonization.Controllers;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class GameplayStart : ALoadingStep
    {
        private readonly GameLoop _game;
        private readonly InputController _inputController;

        public GameplayStart(GameLoop game, InputController inputController) : base(string.Empty)
        {
            _game = game;
            _inputController = inputController;
        }

        public override IEnumerator GetEnumerator()
        {
            yield return null;
            _inputController.Enable();
            _game.Start();
        }
    }
}
