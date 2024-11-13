namespace Vurbiri.Colonization
{
    using System.Collections.Generic;

    public readonly struct PlayerLoadData
	{
        public readonly int[] resources;
        public readonly Dictionary<int, EdificeLoadData[]> edifices;
        public readonly Key[][] roads;
        public readonly ActorLoadData[] warriors;

        public PlayerLoadData(int[] resources, Dictionary<int, List<int[]>> edifices, int[][][] roads, List<int[][]> warriors)
        {
            this.resources = resources;
            this.edifices = CreateEdificesLoadData(edifices);
            this.roads = CreateRoadsData(roads);
            this.warriors = CreateActorData(warriors);

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
                int mainCount = roadsData.Length, count;
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
            ActorLoadData[] CreateActorData(List<int[][]> warriorsData)
            {
                int count = warriorsData.Count;
                ActorLoadData[] warriors = new ActorLoadData[count];

                for (int i = 0; i < count; i++)
                    warriors[i] = new(warriorsData[i]);

                return warriors;
            }
            #endregion
        }
    }
}
