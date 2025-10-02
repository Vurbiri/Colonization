using System.Collections;

namespace Vurbiri.Colonization
{
    sealed public partial class AIController : HumanController
    {
        private readonly Gift _gift;

        public AIController(int playerId, Settings settings) : base(playerId, settings) 
        {
            _gift = new(this);
        }

        public override WaitResult<bool> OnGift(int giver, CurrenciesLite gift, string msg) => _gift.Receive(giver, gift);

        public override void OnLanding()
        {
            OnInitFast_Cn().Start();
        }

        public override void OnPlay()
        {
            GameContainer.GameLoop.EndTurn();
        }

        public override void OnEndTurn()
        {
            _gift.Update();

            OnEndTurn_Cn().Start();
        }

        private IEnumerator OnInit_Cn()
        {
            yield return null;

            if (GameContainer.Crossroads.BreachCount > 0)
            {
                Crossroad port = GameContainer.Crossroads.GetRandomPort();
                yield return GameContainer.CameraController.ToPositionControlled(port);
                yield return BuildPort(port).signal;
            }

            GameContainer.GameLoop.EndLanding();
        }

        private IEnumerator OnInitFast_Cn()
        {
            yield return null;

            if (GameContainer.Crossroads.BreachCount > 0)
            {
                Crossroad port = GameContainer.Crossroads.GetRandomPort();
                BuildPort(port);
                yield return null;
            }

            GameContainer.GameLoop.EndLanding();
        }

    }
}
