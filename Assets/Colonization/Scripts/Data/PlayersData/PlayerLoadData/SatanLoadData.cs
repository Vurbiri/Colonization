//Assets\Colonization\Scripts\Data\PlayersData\PlayerLoadData\SatanLoadData.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization.Data
{
    public class SatanLoadData : APlayerLoadData
    {
        public readonly int level;
        public readonly int curse;
        public readonly int spawnPotential;

        public SatanLoadData(int[] artefact, int[] status, List<int[][]> demons) : base(artefact, demons)
        {
            Satan.FromArray(status, out level, out curse, out spawnPotential);
        }
    }
}
