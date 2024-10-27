using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class PlayerObjects
    {
        private class Edifices
        {
            public readonly IdArray<EdificeGroupId, ReactiveList<Crossroad>> values = new();

            public readonly ReactiveList<Crossroad> shrines = new(Crossroad.Equals);
            public readonly ReactiveList<Crossroad> ports = new(Crossroad.Equals);
            public readonly ReactiveList<Crossroad> urbans = new(Crossroad.Equals);

            public Edifices()
            {
                values[EdificeGroupId.Shrine] = shrines;
                values[EdificeGroupId.Port] = ports;
                values[EdificeGroupId.Urban] = urbans;
            }

            public Edifices(int playerId, PlayerData data, Crossroads crossroads) : this()
            {
                for (int i = 0; i < EdificeGroupId.Count; i++)
                    SetEdifice(playerId, values[i], data.GetEdifices(i), crossroads);
            }

            private void SetEdifice(int playerId, ReactiveList<Crossroad> values, List<int[]> data, Crossroads crossroads)
            {
                Key key = new();
                Crossroad crossroad;
                foreach (var array in data)
                {
                    if (array.Length != 4)
                        throw new($"CrossroadData: неверный размер входного массива ({array.Length}, а не 4)");

                    key.SetValues(array[0], array[1]);
                    crossroad = crossroads[key];
                    if (crossroad.Build(playerId, array[2], array[3] > 0))
                        values.Add(crossroad);
                }
            }

            private bool EqualsCrossroads(Crossroad a, Crossroad b) => a.Key == b.Key;
        }
    }
}
