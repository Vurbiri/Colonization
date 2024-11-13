using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class PlayerObjects
    {
        private class Edifices
        {
            public readonly IdArray<EdificeGroupId, ReactiveList<Crossroad>> values = new();

            public readonly ReactiveList<Crossroad> shrines;
            public readonly ReactiveList<Crossroad> ports;
            public readonly ReactiveList<Crossroad> urbans;

            public Edifices()
            {
                values[EdificeGroupId.Shrine] = shrines = new();
                values[EdificeGroupId.Port] = ports = new();
                values[EdificeGroupId.Urban] = urbans = new();
            }

            public Edifices(int playerId, IReadOnlyDictionary<int, EdificeLoadData[]> data, Crossroads crossroads)
            {
                values[EdificeGroupId.Shrine] = CreateEdifices(ref shrines, data[EdificeGroupId.Shrine], playerId, crossroads);
                values[EdificeGroupId.Port] = CreateEdifices(ref ports, data[EdificeGroupId.Port], playerId, crossroads);
                values[EdificeGroupId.Urban] = CreateEdifices(ref urbans, data[EdificeGroupId.Urban], playerId, crossroads);

            }

            private ReactiveList<Crossroad> CreateEdifices(ref ReactiveList<Crossroad> values, EdificeLoadData[] edificesData, int playerId, Crossroads crossroads)
            {
                int count = edificesData.Length;
                values = new(count);

                EdificeLoadData data;
                Crossroad crossroad;
                for (int i = 0; i < count; i++)
                {
                    data = edificesData[i];
                    crossroad = crossroads[data.key];
                    crossroad.Build(playerId, data.id, data.isWall);
                    values.Add(crossroad);
                }

                return values;
            }
        }
    }
}
