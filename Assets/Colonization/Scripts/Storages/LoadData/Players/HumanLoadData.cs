//Assets\Colonization\Scripts\Storages\LoadData\Players\HumanLoadData.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization.Storage
{
    sealed public class HumanLoadData : APlayerLoadData
    {
        public readonly IReadOnlyList<int> resources;
        public readonly Dictionary<int, List<EdificeLoadData>> edifices;
        public readonly int[][] perks;

        public HumanLoadData(int[] resources, int[] artefact, int[][] perks, Dictionary<int, List<EdificeLoadData>> edifices, List<ActorLoadData> warriors) 
            : base(artefact, warriors)
        {
            this.resources = resources;
            this.perks = perks;
            this.edifices = edifices;
        }

        public HumanLoadData() : base() { }
    }
}
