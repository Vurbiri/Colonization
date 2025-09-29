using System.Collections;

namespace Vurbiri.Colonization
{
    sealed public class AIController : HumanController
    {
        private readonly AISettings _settings;
        private int _giftRatio;


        public AIController(int playerId, Settings settings) : base(playerId, settings) 
        {
            _settings = SettingsFile.Load<AISettings>();

            _giftRatio = _settings.giftRatio;
        }

        public override WaitResult<bool> Gift(int giver, CurrenciesLite gift, string msg)
        {
            int amount = gift.Amount * _giftRatio;
            if (GameContainer.Diplomacy.IsGreatFriend(_id, giver))
                amount <<= 1;
            else if(GameContainer.Diplomacy.IsGreatEnemy(_id, giver))
                amount >>= 1;

            bool result = amount > _resources.Amount;
            if (result)
            {
                _resources.Add(gift);
                GameContainer.Diplomacy.Gift(_id, giver);
            }

            return _waitGift.SetResult(result);
        }

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
            _giftRatio = _settings.giftRatio;

            base.OnEndTurn();
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
