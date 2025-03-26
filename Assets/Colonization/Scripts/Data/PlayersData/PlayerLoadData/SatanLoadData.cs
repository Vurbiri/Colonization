//Assets\Colonization\Scripts\Data\PlayersData\PlayerLoadData\SatanLoadData.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization.Data
{
    public class SatanLoadData : APlayerLoadData
    {
        public readonly SatanState state;

        public SatanLoadData(int[] artefact, SatanState state, List<ActorLoadData> demons) : base(artefact, demons)
        {
            this.state = state;
        }

        public SatanLoadData() : base()
        {
        }
    }
}
