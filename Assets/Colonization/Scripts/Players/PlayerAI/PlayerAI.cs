//Assets\Colonization\Scripts\Players\PlayerAI\PlayerAI.cs
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization
{
    public class PlayerAI : Player
    {
        public PlayerAI(int playerId, int currentPlayerId, bool isLoad, PlayerData data, Players.Settings settings) 
            : base(playerId, currentPlayerId, isLoad, data, settings)
        {
        }
    }
}
