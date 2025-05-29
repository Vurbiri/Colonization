using System.Collections;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization
{
    sealed public class AIController : AHumanController
    {
        private readonly Game _game;
        private readonly Crossroads _crossroads;
        private readonly Hexagons _hexagons;
        private readonly CameraController _cameraController;

        public AIController(Game game, Id<PlayerId> playerId, Storage.HumanStorage storage, Players.Settings settings)
            : base(playerId, storage, settings)
        {
            _game = game;
            _crossroads = settings.crossroads;
            _hexagons = settings.hexagons;
            _cameraController = settings.cameraController;
        }

        public override void OnInit()
        {
            _coroutines.Run(OnInit_Cn());
        }

        public override void OnPlay()
        {

        }

        private IEnumerator OnInit_Cn()
        {
            //WaitRealtime waitRealtime = new(0.5f);
            //yield return waitRealtime;
            yield return null;

            if (_crossroads.BreachCount > 0)
            {
                Crossroad port = _crossroads.GetRandomPort();
                //yield return _cameraController.ToPosition(port.Position);
                BuildPort(port);
                //yield return waitRealtime.Restart(1.5f);
            }
            
            _game.Init();
        }
    }
}
