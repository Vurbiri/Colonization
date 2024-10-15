namespace Vurbiri.Colonization
{
    using static CONST;

    public class PlayersData
    {
        private readonly PlayerData[] _playersData;

        public PlayersData(PricesScriptable prices)
        {
            _playersData = new PlayerData[MAX_PLAYERS];

            for (int i = 0; i < MAX_PLAYERS; i++)
                _playersData[i] = new(prices.PlayersDefault);
        }
    }
}
