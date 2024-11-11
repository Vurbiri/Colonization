namespace Vurbiri.Colonization.Data
{
    using Newtonsoft.Json;
    using Reactive;
    using Reactive.Collections;
    using System;
    using System.Collections.Generic;
    using Vurbiri.Colonization.Actors;
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
        private List<int[][]> _warriors;
        [JsonProperty(P_PERKS)]
        private int[] _perks;

        private readonly List<Unsubscriber<int>> _unsubscribersResources = new(CurrencyId.CountAll);
        private readonly List<UnsubscriberList<Crossroad>> _unsubscriberEdifices = new(EdificeGroupId.Count);
        private Unsubscriber<int[][][]> _unsubscriberRoads;
        private UnsubscriberCollection<Actor> _unsubscriberWarriors;

        private Action<PlayerData> actionThisChange;

        public int Id => _id;
        public int[] Resources => _resources;
        public int[][][] Roads => _roads;
        public List<int[][]> Warriors => _warriors;
        public int[] Perks => _perks;

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


        public List<int[]> GetEdifices(int id) => _edifices[id];

        public void CurrenciesBind(AReadOnlyCurrenciesReactive currencies, bool calling)
        {
            for (int i = 0; i < CurrencyId.Count; i++)
            {
                int index = i;
                _unsubscribersResources.Add(currencies.Subscribe(i, v => _resources[index] = v, calling));
            }
        }

        public void EdificesBind(IReadOnlyList<IReactiveList<Crossroad>> edificesReactive, bool calling)
        {
            for(int i = 0; i < EdificeGroupId.Count; i++)
                EdificesBind(edificesReactive[i], _edifices[i], calling);
        }

        public void RoadsBind(IReactive<int[][][]> roadsReactive, bool calling)
        { 
            _unsubscriberRoads = roadsReactive.Subscribe(OnRoads, calling);

            #region Local OnRoads(...)
            //==============================
            void OnRoads(int[][][] values)
            {
                _roads = values;
                actionThisChange?.Invoke(this);
            }
            #endregion
        }

        public void WarriorsBind(IReactiveCollection<Actor> warriorsReactive, bool calling)
        {
            _unsubscriberWarriors = warriorsReactive.Subscribe(OnWarriors, calling);

            #region Local OnWarriors(...)
            //==============================
            void OnWarriors(Actor actor, Operation operation)
            {
                switch (operation)
                {
                    case Operation.Add:
                        _warriors.Add(actor.ToArray());
                        actionThisChange?.Invoke(this);
                        break;
                    case Operation.Remove:
                        _warriors.RemoveAt(actor.Index);
                        actionThisChange?.Invoke(this);
                        break;
                    case Operation.Change:
                        _warriors[actor.Index] = actor.ToArray();
                        break;
                    default:
                        return;
                }
            }
            #endregion
        }

        public Unsubscriber<PlayerData> Subscribe(Action<PlayerData> action, bool calling = true)
        {
            actionThisChange -= action ?? throw new ArgumentNullException("action");

            actionThisChange += action;
            if (calling)
                action(this);

            return new(this, action);
        }

        public void Unsubscribe(Action<PlayerData> action) => actionThisChange -= action ?? throw new ArgumentNullException("action");

        public void Dispose()
        {
            foreach (var unsubscriber in _unsubscribersResources)
                unsubscriber?.Unsubscribe();

            foreach (var unsubscriber in _unsubscriberEdifices)
                unsubscriber?.Unsubscribe();

            _unsubscriberRoads?.Unsubscribe();
            _unsubscriberWarriors?.Unsubscribe();
        }

        private void EdificesBind(IReactiveList<Crossroad> edificesReactive, List<int[]> edifices, bool calling)
        {
            _unsubscriberEdifices.Add(edificesReactive.Subscribe(OnEdifice, calling));

            #region Local OnEdifice(...)
            //==============================
            void OnEdifice(int index, Crossroad crossroad, Operation operation)
            {
                switch (operation)
                {
                    case Operation.Add:
                        edifices.Add(crossroad.ToArray());
                        break;
                    case Operation.Remove:
                        edifices.RemoveAt(index);
                        break;
                    case Operation.Insert:
                        edifices.Insert(index, crossroad.ToArray());
                        break;
                    case Operation.Change:
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
