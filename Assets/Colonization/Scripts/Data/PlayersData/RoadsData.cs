using Newtonsoft.Json;
using System.Collections;

namespace Vurbiri.Colonization.Data
{
    [JsonArray]
    public class RoadsData : IEnumerable
    {
        public int[][][] values;

        public RoadsData(Roads roads)
        {
            roads.EventChangeValue += (v) => values = v;
            values = new int[0][][];
        }

        public RoadsData(int[][][] array, Roads roads, Crossroads crossroads)
        {
            values = array;
            roads.EventChangeValue += (v) => values = v;

            if (values == null)
            {
                values = new int[0][][];
                return;
            }

            foreach (var keys in values)
                CreateRoad(keys, roads, crossroads);

            roads.SetRoadsEndings();

            #region Local: CreateRoad(...)
            //=================================
            static void CreateRoad(int[][] keys, Roads roads, Crossroads crossroads)
            {
                int count = keys.Length;
                if (count < 2) return;

                Key key = new(keys[0]);
                Crossroad start = crossroads[key];
                for (int i = 1; i < count; i++)
                {
                    foreach (var link in start.Links)
                    {
                        if (link.Contains(key.SetValues(keys[i])))
                        {
                            link.SetStart(start);
                            start = link.End;
                            roads.Build(link);
                            break;
                        }
                    }
                }
            }
            #endregion
        }

        IEnumerator IEnumerable.GetEnumerator() => values.GetEnumerator();
    }
}
