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

#if YSDK
            var money = ProjectContainer.Money;
            if (money.IsInitialize)
            {
                money.ShowBannerAdv();
                Transition.OnExit.Add(money.HideBannerAdv);
                yield return null;
            }
#endif
            GameContainer.GameLoop.Start();
        }
    }
}
