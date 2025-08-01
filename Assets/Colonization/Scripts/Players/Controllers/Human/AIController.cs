using System.Collections;

namespace Vurbiri.Colonization
{
    sealed public class AIController : AHumanController
    {
        public AIController(int playerId, Settings settings) : base(playerId, settings) 
        { 
        }

        public override void OnLanding()
        {
            OnInitFast_Cn().Start();
        }

        public override void OnPlay()
        {

        }

        private IEnumerator OnInit_Cn()
        {
            yield return null;

            if (GameContainer.Crossroads.BreachCount > 0)
            {
                Crossroad port = GameContainer.Crossroads.GetRandomPort();
                yield return GameContainer.CameraController.ToPosition(port.Position, false);
                yield return BuildPort(port).signal;
            }

            GameContainer.GameLoop.EndLanding().Start();
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

            GameContainer.GameLoop.EndLanding().Start();
        }

    }
}
