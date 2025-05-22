//Assets\Colonization\Scripts\EntryPoint\Gameplay\Steps\GameplayStart.cs
using System.Collections;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class GameplayStart : ALoadingStep
    {
        private readonly GameLoop _game;

        public GameplayStart(GameLoop game) : base(string.Empty)
        {
            _game = game;
        }

        public override IEnumerator GetEnumerator()
        {
            yield return null;
            _game.Start();
        }
    }
}
