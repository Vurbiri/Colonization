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

            public readonly ReactiveList<Crossroad> shrines = new();
            public readonly ReactiveList<Crossroad> ports = new();
            public readonly ReactiveList<Crossroad> urbans = new();

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
                        throw new($"CrossroadData: �������� ������ �������� ������� ({array.Length}, � �� 4)");

                    key.SetValues(array[0], array[1]);
                    crossroad = crossroads[key];
                    crossroad.Build(playerId, array[2], array[3] > 0);
                    values.Add(crossroad);
                }
            }

            
        }
    }
}
