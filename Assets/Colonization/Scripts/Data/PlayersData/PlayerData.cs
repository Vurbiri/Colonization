//Assets\Colonization\Scripts\Data\PlayersData\PlayerData.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Data
{
    using static JSON_KEYS;

    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerData : IReactive<PlayerData>, IDisposable
    {
        [JsonProperty(P_ID)]
        private readonly int _id;
        [JsonProperty(P_RESURSES)]
        private readonly int[] _resources;
        [JsonProperty(P_EDIFICES)]
        private readonly Dictionary<int, List<int[]>> _edifices = new(EdificeGroupId.Count);
        [JsonProperty(P_ROADS)]
        private int[][][] _roads;
        [JsonProperty(P_WARRIORS)]
        private readonly List<int[][]> _warriors = new();
        [JsonProperty(P_PERKS)]
        private int[] _perks;

        private Unsubscribers _unsubscribers = new(CurrencyId.CountAll + EdificeGroupId.Count + 2);

        private Action<PlayerData> actionThisChange;

        public int Id => _id;

        public PlayerData(int id)
        {
            _id = id;
            _resources = new int[CurrencyId.CountAll];
            _edifices = new(EdificeGroupId.Count);
            _perks = new int[0];

            for (int i = 0; i < EdificeGroupId.Count; i++)
                _edifices[i] = new();
        }

        [JsonConstructor]
        public PlayerData() {}


        public PlayerLoadData ToLoadData() => new(_resources, _edifices, _roads, _warriors);

        public void CurrenciesBind(IReactive<int, int> currencies, bool calling)
        {
            _unsubscribers += currencies.Subscribe((i, v) => _resources[i] = v, calling);
        }

        public void EdificesBind(IReadOnlyList<IReactiveList<Crossroad>> edificesReactive)
        {
            for(int i = 0; i < EdificeGroupId.Count; i++)
                EdificesBind(edificesReactive[i], _edifices[i]);
        }
        public void RoadsBind(IReactive<int[][][]> roadsReactive, bool calling)
        {
            _unsubscribers += roadsReactive.Subscribe(OnRoads, calling);

            #region Local OnRoads(..)
            //==============================
            void OnRoads(int[][][] values)
            {
                _roads = values;
                actionThisChange?.Invoke(this);
            }
            #endregion
        }
        public void WarriorsBind(IReactiveCollection<Actor> warriorsReactive)
        {
            _unsubscribers += warriorsReactive.Subscribe(OnWarriors);

            #region Local OnWarriors(..)
            //==============================
            void OnWarriors(Actor actor, TypeEvent operation)
            {
                switch (operation)
                {
                    case TypeEvent.Add:
                        _warriors.Add(actor.ToArray());
                        actionThisChange?.Invoke(this);
                        return;
                    case TypeEvent.Remove:
                        _warriors.RemoveAt(actor.Index);
                        actionThisChange?.Invoke(this);
                        return;
                    case TypeEvent.Change:
                        _warriors[actor.Index] = actor.ToArray();
                        return;
                    default:
                        return;
                }
            }
            #endregion
        }

        public IUnsubscriber Subscribe(Action<PlayerData> action, bool calling = true)
        {
            actionThisChange += action;
            if (calling)
                action(this);

            return new Unsubscriber<Action<PlayerData>>(this, action);
        }

        public void Unsubscribe(Action<PlayerData> action) => actionThisChange -= action;

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }

        private void EdificesBind(IReactiveList<Crossroad> edificesReactive, List<int[]> edifices)
        {
            _unsubscribers += edificesReactive.Subscribe(OnEdifice);

            #region Local OnEdifice(..)
            //==============================
            void OnEdifice(int index, Crossroad crossroad, TypeEvent operation)
            {
                switch (operation)
                {
                    case TypeEvent.Add:
                        edifices.Add(crossroad.ToArray());
                        break;
                    case TypeEvent.Remove:
                        edifices.RemoveAt(index);
                        break;
                    case TypeEvent.Change:
                        edifices[index] = crossroad.ToArray();
                        break;
                    default:
                        return;
                }
                actionThisChange?.Invoke(this);
            }
            #endregion
        }
    }
}
