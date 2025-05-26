using System.Collections;

namespace Vurbiri.Colonization
{
    sealed public class AIController : AHumanController
    {
        private readonly Game _game;
        private readonly Crossroads _crossroads;
        private readonly Hexagons _hexagons;

        public AIController(Game game, Id<PlayerId> playerId, Storage.HumanStorage storage, Players.Settings settings) 
            : base(playerId, storage, settings)
        {
            _game = game;
            _crossroads = settings.crossroads;
            _hexagons = settings.hexagons;
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
            WaitRealtime waitRealtime = new(1f);
            yield return waitRealtime;
            while (_crossroads.BreachCount > 0)
            {
                if (BuildPort(_crossroads.GetRandomPort()))
                    break;
            }
            yield return waitRealtime;
            
            _game.Init();
        }
    }
}
