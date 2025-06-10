using System.Collections;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization
{
    sealed public class AIController : AHumanController
    {
        private readonly GameLoop _game;
        private readonly Crossroads _crossroads;
        private readonly CameraController _cameraController;

        public AIController(GameLoop game, Id<PlayerId> playerId, Storage.HumanStorage storage, Players.Settings settings)
            : base(playerId, storage, settings)
        {
            _game = game;
            _crossroads = settings.crossroads;
            _cameraController = settings.cameraController;
        }

        public override void OnLanding()
        {
            _coroutines.Run(OnInitFast_Cn());
        }

        public override void OnPlay()
        {

        }

        private IEnumerator OnInit_Cn()
        {
            yield return null;

            if (_crossroads.BreachCount > 0)
            {
                Crossroad port = _crossroads.GetRandomPort();
                yield return _cameraController.ToPosition(port.Position);
                yield return BuildPort(port).signal;
            }

            _coroutines.Run(_game.EndLanding());
        }

        private IEnumerator OnInitFast_Cn()
        {
            yield return null;

            if (_crossroads.BreachCount > 0)
            {
                Crossroad port = _crossroads.GetRandomPort();
                BuildPort(port);
                yield return null;
            }

            _coroutines.Run(_game.EndLanding());
        }

    }
}
