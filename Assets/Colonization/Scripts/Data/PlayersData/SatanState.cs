//Assets\Colonization\Scripts\Data\PlayersData\SatanState.cs
namespace Vurbiri.Colonization.Data
{
    public readonly struct SatanState
	{
        public readonly int level;
        public readonly int curse;
        public readonly int balance;
        public readonly int spawnPotential;
        public readonly int maxDemons;

        public SatanState(int[] status)
        {
            Satan.FromArray(status, out level, out curse, out balance, out spawnPotential, out maxDemons);
        }
    }
}
