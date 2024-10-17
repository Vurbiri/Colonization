using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Vurbiri.Colonization.Data
{
    [JsonArray]
    public class CrossroadData : IEnumerable<int>
    {
        public readonly Key key;
        public readonly int edificeId;
        public readonly bool isWall;

        public CrossroadData(Key key, int edificeId, bool isWall)
        {
            this.key = key;
            this.edificeId = edificeId;
            this.isWall = isWall;
        }
        [JsonConstructor]
        public CrossroadData(int[] array)
        {
            if (array.Length != 4)
                throw new Exception($"CrossroadData: неверный размер входного массива ({array.Length}, а не 4)");

            key = new(array[0], array[1]);
            edificeId = array[2];
            isWall = array[3] > 0;
        }

        public IEnumerator<int> GetEnumerator()
        {
            yield return key.X;
            yield return key.Y;
            yield return edificeId;
            yield return isWall ? 1 : 0;
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
