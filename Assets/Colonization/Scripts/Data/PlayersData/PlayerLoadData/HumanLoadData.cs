//Assets\Colonization\Scripts\Data\PlayersData\PlayerLoadData\HumanLoadData.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization.Data
{
    public class HumanLoadData : APlayerLoadData
    {
        public readonly IReadOnlyList<int> resources;
        public readonly IReadOnlyDictionary<int, EdificeLoadData[]> edifices;
        public readonly IReadOnlyList<IReadOnlyList<Key>> roads;
        public readonly IReadOnlyList<IReadOnlyList<int>> perks;

        public HumanLoadData(int[] resources, int[][][] roads, int[] artefact, List<int>[] perks, Dictionary<int, List<int[]>> edifices, List<int[][]> warriors) 
            : base(artefact, warriors)
        {
            this.resources = resources;
            this.perks = perks;
            this.edifices = CreateEdificesLoadData(edifices);
            this.roads = CreateRoadsData(roads);

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
            //================================================================
            static Key[][] CreateRoadsData(int[][][] roadsData)
            {
                int count, mainCount = roadsData.Length;
                Key[][] roads = new Key[mainCount][];

                for (int i = 0; i < mainCount; i++)
                {
                    count = roadsData[i].Length;
                    roads[i] = new Key[count];
                    for (int j = 0; j < count; j++)
                        roads[i][j] = new(roadsData[i][j]);
                }
                return roads;
            }
            #endregion
        }
    }
}
