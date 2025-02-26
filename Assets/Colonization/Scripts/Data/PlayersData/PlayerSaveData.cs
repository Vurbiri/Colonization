//Assets\Colonization\Scripts\Data\PlayersData\PlayerSaveData.cs
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Data
{
    using static SAVE_KEYS;

    [JsonObject(MemberSerialization.OptIn)]
    public class PlayerSaveData : IReactive<PlayerSaveData>, IDisposable
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

        private readonly bool _isLoaded;
        private readonly Subscriber<PlayerSaveData> _subscriber = new();
        private Unsubscribers _unsubscribers = new(CurrencyId.CountAll + EdificeGroupId.Count + 3);

        public int Id => _id;
        public bool IsLoaded => _isLoaded;

        public PlayerSaveData(int id)
        {
            _id = id;
            _resources = new int[CurrencyId.CountAll];
            _edifices = new(EdificeGroupId.Count);
            _perks = new int[0];

            for (int i = 0; i < EdificeGroupId.Count; i++)
                _edifices[i] = new();

            _isLoaded = false;
        }

        [JsonConstructor]
        public PlayerSaveData() { _isLoaded = true; }

        public PlayerLoadData ToLoadData => new(_resources, _edifices, _roads, _warriors);

        #region Bind
        public void CurrenciesBind(IReactive<int, int> currencies)
        {
            _unsubscribers += currencies.Subscribe((i, v) => _resources[i] = v, !_isLoaded);
        }

        public void EdificesBind(IReadOnlyList<IReactiveList<IArrayable>> edificesReactive)
        {
            for(int i = 0; i < EdificeGroupId.Count; i++)
                Bind(edificesReactive[i], _edifices[i]);

            #region Local Bind(..)
            //==============================
            void Bind(IReactiveList<IArrayable> edificesReactive, List<int[]> edifices)
            {
                _unsubscribers += edificesReactive.Subscribe(OnEdifice, false);

                #region Local OnEdifice(..)
                //==============================
                void OnEdifice(int index, IArrayable crossroad, TypeEvent operation)
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
                            crossroad.ToArray(edifices[index]);
                            break;
                        default:
                            return;
                    }
                    _subscriber.Invoke(this);
                }
                #endregion
            }
            #endregion
        }
        public void RoadsBind(IReactive<int[][][]> roadsReactive)
        {
            _unsubscribers += roadsReactive.Subscribe(OnRoads, false);

            #region Local OnRoads(..)
            //==============================
            void OnRoads(int[][][] values)
            {
                _roads = values;
                _subscriber.Invoke(this);
            }
            #endregion
        }
        public void WarriorsBind(IListReactiveItems<Actor> warriorsReactive)
        {
            _unsubscribers += warriorsReactive.Subscribe(OnWarriors, false);

            #region Local OnWarriors(..)
            //==============================
            void OnWarriors(Actor actor, TypeEvent operation)
            {
                switch (operation)
                {
                    case TypeEvent.Add:
                        _warriors.Add(actor.ToArray());
                        break;
                    case TypeEvent.Remove:
                        _warriors.RemoveAt(actor.Index);
                        break;
                    case TypeEvent.Change:
                        _warriors[actor.Index] = actor.ToArray(_warriors[actor.Index]);
                        break;
                    default:
                        return;
                }
                _subscriber.Invoke(this);
            }
            #endregion
        }
        #endregion

        #region IReactive
        public Unsubscriber Subscribe(Action<PlayerSaveData> action, bool calling = true)
        {
            if (calling)
                action(this);

            return _subscriber.Add(action);
        }
        #endregion

        public void Dispose()
        {
            _unsubscribers.Unsubscribe();
        }

    }
}
