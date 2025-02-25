//Assets\Colonization\Scripts\Data\PlayersData\PlayerLoadData\PlayerLoadData.cs
using System.Collections.Generic;

namespace Vurbiri.Colonization.Data
{
    public readonly struct PlayerLoadData
	{
        public readonly IReadOnlyList<int> resources;
        public readonly IReadOnlyDictionary<int, EdificeLoadData[]> edifices;
        public readonly IReadOnlyList<IReadOnlyList<Key>> roads;
        public readonly ActorLoadData[] warriors;

        public PlayerLoadData(int[] resources, Dictionary<int, List<int[]>> edifices, int[][][] roads, List<int[][]> warriors, Id<PlayerId> currentPlayerId)
        {
            this.resources = resources;
            this.edifices = CreateEdificesLoadData(edifices);
            this.roads = CreateRoadsData(roads);
            this.warriors = CreateActorData(warriors, currentPlayerId);

            #region Local: CreateEdificesLoadData(..), CreateRoadsData(..), CreateActorData(...)
            //================================================================
            Dictionary<int, EdificeLoadData[]> CreateEdificesLoadData(Dictionary<int, List<int[]>> edificesData)
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
            Key[][] CreateRoadsData(int[][][] roadsData)
            {
                int count, mainCount = roadsData == null ? 0 : roadsData.Length;
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
            //================================================================
            ActorLoadData[] CreateActorData(List<int[][]> warriorsData, Id<PlayerId> currentPlayerId)
            {
                int count = warriorsData.Count;
                ActorLoadData[] warriors = new ActorLoadData[count];

                for (int i = 0; i < count; i++)
                    warriors[i] = new(warriorsData[i], currentPlayerId);

                return warriors;
            }
            #endregion
        }
    }
}
