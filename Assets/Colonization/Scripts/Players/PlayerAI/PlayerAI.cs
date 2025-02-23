//Assets\Colonization\Scripts\Players\PlayerAI\PlayerAI.cs
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization
{
    public class PlayerAI : Player
    {
        public PlayerAI(Id<PlayerId> playerId, Id<PlayerId> currentPlayerId, PlayerSaveData data, Players.Settings settings) 
            : base(playerId, currentPlayerId, data, settings)
        {
        }
    }
}
