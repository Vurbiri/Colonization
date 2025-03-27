//Assets\Colonization\Scripts\Storages\LoadData\Players\SatanLoadData.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization.Storage
{
    sealed public class SatanLoadData : APlayerLoadData
    {
        public readonly SatanLoadState state;

        public SatanLoadData(int[] artefact, SatanLoadState state, List<ActorLoadData> demons) : base(artefact, demons)
        {
            this.state = state;
        }

        public SatanLoadData(SatanLoadState state) : base()
        {
            this.state = state;
        }
    }
}
