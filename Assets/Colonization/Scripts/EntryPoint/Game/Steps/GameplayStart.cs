using System.Collections;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class GameplayStart : ALoadingStep
    {
       
        public GameplayStart() : base(string.Empty)
        {
        }

        public override IEnumerator GetEnumerator()
        {
            yield return null;
            GameContainer.GameSettings.Start();
            GameContainer.InputController.Enable();

            GameContainer.GameLoop.Start();
        }
    }
}
