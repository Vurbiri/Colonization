using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization.Data
{
    [JsonArray]
    public class EdificesData : IEnumerable<int[]>
    {
        public readonly Dictionary<int, Dictionary<Key, Crossroad>> values;

        public EdificesData()
        {
            values = new(EdificeGroupId.Count - EdificeGroupId.Shrine);
            for (int i = EdificeGroupId.Shrine; i < EdificeGroupId.Count; i++)
                values[i] = new();
        }

        public EdificesData(int playerId, int[][] data, Crossroads crossroads) : this()
        {
            Key key = new();
            Crossroad crossroad;
            foreach (var array in data)
            {
                if (array.Length != 4)
                    throw new Exception($"CrossroadData: неверный размер входного массива ({array.Length}, а не 4)");

                key.SetValues(array[0], array[1]);
                crossroad = crossroads[key];
                if (crossroad.Build(playerId, array[2], array[3] > 0))
                    values[crossroad.GroupId][crossroad.Key] = crossroad;
            }
        }

        public IEnumerator<int[]> GetEnumerator()
        {
            foreach (var dict in values.Values)
                foreach (var crossroad in dict.Values)
                    yield return crossroad.ToArray();
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
