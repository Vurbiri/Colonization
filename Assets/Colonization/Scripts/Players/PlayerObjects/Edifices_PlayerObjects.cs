using System;
using System.Collections.Generic;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class PlayerObjects
    {
        private class Edifices : IReactive<List<int[]>>
        {
            public readonly Dictionary<int, Dictionary<Key, Crossroad>> values;

            private Action<List<int[]>> ActionValueChange;

            public Edifices()
            {
                values = new(EdificeGroupId.Count - EdificeGroupId.Shrine);
                for (int i = EdificeGroupId.Shrine; i < EdificeGroupId.Count; i++)
                    values[i] = new();
            }

            public Edifices(int playerId, List<int[]> data, Crossroads crossroads) : this()
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
                        values[crossroad.GroupId][crossroad.Key] = crossroad;
                }
            }

            public void Add(Crossroad crossroad)
            {
                values[crossroad.GroupId][crossroad.Key] = crossroad;
                ActionValueChange?.Invoke(ToList());
            }

            #region Reactive
            public Unsubscriber<List<int[]>> Subscribe(Action<List<int[]>> action, bool calling = false)
            {
                ActionValueChange -= action;
                ActionValueChange += action;
                if (calling && action != null)
                    action(ToList());

                return new(this, action);
            }

            public void Unsubscribe(Action<List<int[]>> action) => ActionValueChange -= action;

            public void Signal() => ActionValueChange?.Invoke(ToList());

            private List<int[]> ToList()
            {
                List<int[]> result = new();

                foreach (var dict in values.Values)
                    foreach (var crossroad in dict.Values)
                        result.Add(crossroad.ToArray());

                return result;
            }
            #endregion

        }
    }
}
