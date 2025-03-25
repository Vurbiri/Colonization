//Assets\Colonization\Scripts\Data\PlayersData\PlayerLoadData\HumanLoadData.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization.Data
{
    public class HumanLoadData : APlayerLoadData
    {
        public readonly IReadOnlyList<int> resources;
        public readonly Dictionary<int, EdificeLoadData[]> edifices;
        public readonly Key[][] roads;
        public readonly int[][] perks;

        public HumanLoadData(int[] resources, Key[][] roads, int[] artefact, int[][] perks, Dictionary<int, List<int[]>> edifices, List<int[][]> warriors) 
            : base(artefact, warriors)
        {
            this.resources = resources;
            this.perks = perks;
            this.edifices = CreateEdificesLoadData(edifices);
            this.roads = roads;

            #region Local: CreateEdificesLoadData(..), CreateRoadsData(..)
            //================================================================
            static Dictionary<int, EdificeLoadData[]> CreateEdificesLoadData(Dictionary<int, List<int[]>> edificesData)
            {
                Dictionary<int, EdificeLoadData[]> edifices = new(EdificeGroupId.Count);

                List<int[]> source;
                EdificeLoadData[] data;
                int count;
                for (int i = 0; i < EdificeGroupId.Count; i++)
                {
                    source = edificesData[i];
                    count = source.Count;
                    data = new EdificeLoadData[count];
                    for (int j = 0; j < count; j++)
                        data[j] = new(source[j]);
                    edifices.Add(i, data);
                }

                return edifices;
            }
            #endregion
        }

        public HumanLoadData() : base()
        {

        }
    }
}
