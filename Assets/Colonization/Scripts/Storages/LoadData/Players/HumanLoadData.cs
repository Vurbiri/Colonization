using System.Collections.Generic;

namespace Vurbiri.Colonization.Storage
{
    sealed public class HumanLoadData : APlayerLoadData
    {
        public readonly CurrenciesLite resources;
        public readonly int[] exchange;
        public readonly Dictionary<int, List<EdificeLoadData>> edifices;
        public readonly int[][] perks;

        public HumanLoadData(int[] resources,
                             int[] exchange,
                             int[] artefact,
                             int[][] perks,
                             Dictionary<int, List<EdificeLoadData>> edifices,
                             List<ActorLoadData> warriors) 
            : base(artefact, warriors)
        {
            this.resources = new(resources);
            this.exchange = exchange;
            this.perks = perks;
            this.edifices = edifices;
        }

        public HumanLoadData() : base() { }
    }
}
