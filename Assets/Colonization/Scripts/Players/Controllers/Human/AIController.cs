using System.Collections;
using Vurbiri.Colonization.Controllers;

namespace Vurbiri.Colonization
{
    sealed public class AIController : AHumanController
    {
        private readonly Game _game;
        private readonly Crossroads _crossroads;
        private readonly Hexagons _hexagons;
        private readonly InputController _inputController;

        public AIController(Game game, Id<PlayerId> playerId, Storage.HumanStorage storage, Players.Settings settings)
            : base(playerId, storage, settings)
        {
            _game = game;
            _crossroads = settings.crossroads;
            _hexagons = settings.hexagons;
            _inputController = settings.inputController;
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
            WaitRealtime waitRealtime = new(1.5f);
            yield return waitRealtime;

            if (_crossroads.BreachCount > 0)
            {
                Crossroad port = _crossroads.GetRandomPort();
                _inputController.Select(port);

                yield return waitRealtime;

                BuildPort(port);
                yield return waitRealtime;
            }
            
            _game.Init();
        }
    }
}
