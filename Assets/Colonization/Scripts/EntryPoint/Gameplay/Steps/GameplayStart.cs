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
            GameplayContainer.GameSettings.Start();
            GameplayContainer.InputController.Enable();
            
            yield return GameplayContainer.Game.Start();
        }
    }
}
