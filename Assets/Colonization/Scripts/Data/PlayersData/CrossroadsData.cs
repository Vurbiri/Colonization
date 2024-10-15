using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization
{
    [JsonArray]
    public class CrossroadsData : IEnumerable<CrossroadData>
    {
        private readonly Dictionary<int, Dictionary<Key, CrossroadData>> _crossroads;

        public CrossroadsData()
        {
            _crossroads = new(EdificeGroupId.Count - EdificeGroupId.Shrine);
            for (int i = EdificeGroupId.Shrine; i < EdificeGroupId.Count; i++)
                _crossroads[i] = new();
        }

        public CrossroadsData(int[][] array) : this() 
        {
            CrossroadData cross;
            foreach (var data in array)
            {
                cross = new(data);
                _crossroads[EdificeId.ToGroup(cross.edificeId)].Add(cross.key, cross);
            }
        }

        public void Add(Crossroad crossroad)
        {
            Key key = crossroad.Key;
            _crossroads[crossroad.GroupId][key] = new(key, crossroad.Id, crossroad.IsWall);
        }

        public int Count(int edificeGroupId) => _crossroads[edificeGroupId].Count;

        public IEnumerator<CrossroadData> GetEnumerator()
        {
            foreach (var dict in _crossroads.Values)
                foreach (var cross in dict.Values)
                    yield return cross;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
