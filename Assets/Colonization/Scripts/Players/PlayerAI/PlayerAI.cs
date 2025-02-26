//Assets\Colonization\Scripts\Players\PlayerAI\PlayerAI.cs
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization
{
    public class PlayerAI : Player
    {
        public PlayerAI(Id<PlayerId> playerId, PlayerSaveData data, Players.Settings settings) : base(playerId, data, settings)
        {
        }
    }
}
