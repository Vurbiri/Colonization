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
            OnInitFast_Cn().Run();
        }

        public override void OnPlay()
        {

        }

        private IEnumerator OnInit_Cn()
        {
            yield return null;

            if (s_crossroads.BreachCount > 0)
            {
                Crossroad port = s_crossroads.GetRandomPort();
                yield return s_cameraController.ToPosition(port.Position);
                yield return BuildPort(port).signal;
            }

            s_game.EndLanding().Run();
        }

        private IEnumerator OnInitFast_Cn()
        {
            yield return null;

            if (s_crossroads.BreachCount > 0)
            {
                Crossroad port = s_crossroads.GetRandomPort();
                BuildPort(port);
                yield return null;
            }

            s_game.EndLanding().Run();
        }

    }
}
