//Assets\Colonization\Scripts\Island\Crossroad\Crossroad\Crossroad.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization
{
    public partial class Crossroad : IDisposable, IPositionable, ICancel, IArrayable
    {
        #region Fields
        private const int HEX_COUNT = 3;

        private readonly Key _key;
        private AEdifice _edifice;
        private EdificeSettings _states;
        private Id<PlayerId> _owner = PlayerId.None;
        private bool _isWall = false;
        private int _defenceWall = 0;

        private readonly GameplayEventBus _eventBus;
        private readonly IReadOnlyList<AEdifice> _prefabs;
        private readonly List<Hexagon> _hexagons = new(HEX_COUNT);
        private readonly IdHashSet<LinkId, CrossroadLink> _links = new();

        private int _countFreeLink = 0, _countWater = 0;
        private bool _isGate = false;
        private WaitResult<Hexagon> _waitHexagon;
        private readonly ReactiveValue<bool> _canCancel = new(false);

        private Unsubscriber _unsubscriber;
        #endregion

        #region Property
        public Key Key => _key;
        public Id<EdificeId> Id => _states.id;
        public Id<EdificeGroupId> GroupId => _states.groupId;
        public Id<EdificeId> NextId => _states.nextId;
        public Id<EdificeGroupId> NextGroupId => _states.nextGroupId;
        public bool IsPort => _states.groupId == EdificeGroupId.Port;
        public bool IsUrban => _states.groupId == EdificeGroupId.Urban;
        public bool IsShrine => _states.groupId == EdificeGroupId.Shrine;
        public bool IsWall => _isWall;
        public IReadOnlyList<CrossroadLink> Links => _links;
        public IReactiveValue<bool> CanCancel => _canCancel;
        public Vector3 Position { get;}
        #endregion

        public Crossroad(Key key, Transform container, Vector3 position, Quaternion rotation, IReadOnlyList<AEdifice> prefabs, GameplayEventBus eventBus)
        {
            _key = key;
            Position = position;
            _prefabs = prefabs;

            _eventBus = eventBus;

            _edifice = Object.Instantiate(_prefabs[EdificeId.Signpost], position, rotation, container);
            _edifice.Subscribe(OnSelect, OnUnselect);
            _states = _edifice.Settings;
        }

        public bool AddHexagon(Hexagon hexagon)
        {
            _isGate |= hexagon.IsGate;

            if (hexagon.IsWater) _countWater++;

            if (_hexagons.Count < (HEX_COUNT - 1) | _countWater < HEX_COUNT)
            {
                _hexagons.Add(hexagon);

                if (_hexagons.Count == HEX_COUNT)
                    _countFreeLink = _countWater <= 1 ? _isGate ? 1 : 3 : 2;

                return true;
            }

            for(int i = 0; i < _hexagons.Count; i++)
                _hexagons[i].CrossroadRemove(this);
            Object.Destroy(_edifice.gameObject);
            return false;
        }

        public int GetDefense(Id<PlayerId> playerId) => playerId == _owner ? _defenceWall : -1;

        #region Link
        public bool ContainsLink(int id) => _links.ContainsKey(id);
        public void AddLink(CrossroadLink link)
        {
            _links.Add(link);

            if (_countFreeLink == _links.CountAvailable)
            {
                _states.SetNextId(EdificeId.GetId(_countWater, _isGate));
                _edifice.Init(_owner, _isWall, _links, _edifice);
            }
        }

        public CrossroadLink GetLink(Id<LinkId> linkId) => _links[linkId];
        public CrossroadLink GetLinkAndSetStart(Id<LinkId> linkId) => _links[linkId].SetStart(this);
        #endregion

        #region Profit
        public CurrenciesLite ProfitFromPort(int idHex, int add)
        {
            CurrenciesLite profit = new();
            for (int i = 0; i < HEX_COUNT; i++)
                if (_hexagons[i].TryGetProfit(idHex, true, out int currencyId))
                    profit.Add(currencyId, _states.profit + add);

            return profit;
        }
        public CurrenciesLite ProfitFromUrban(int idHex, int compensationRes)
        {
            CurrenciesLite profit = new();
            Hexagon hex;
            int countEnemy = 0;

            for (int i = 0; i < HEX_COUNT; i++)
            {
                hex = _hexagons[i];

                if (hex.IsEnemy(_owner))
                    countEnemy++;

                if (hex.TryGetProfit(idHex, false, out int currencyId))
                    profit.Increment(currencyId);
            }

            if (profit.Amount == 0)
            {
                if (countEnemy == 0)
                    profit.RandomMainAdd(compensationRes);

                return profit;
            }

            profit.Multiply(Mathf.Max(_states.profit - Mathf.Max(countEnemy - _defenceWall, 0), 0));
            return profit;
        }
        #endregion

        #region ISelectable, ICancel
        public void OnSelect()
        {
            Debug.Log("Отправлять только если игрок");
            _eventBus.TriggerCrossroadSelect(this);
        }
        public void OnUnselect(ISelectable newSelectable)
        {
            _eventBus.TriggerUnselect();

            if (_waitHexagon == null)
                return;

            _canCancel.Value = false;

            _waitHexagon.SetResult(newSelectable as Hexagon);
            foreach (var hex in _hexagons)
                hex.SetUnselectable();

            _waitHexagon = null;
        }
        public void Cancel() => OnUnselect(null);
        #endregion

        #region IArrayable
        private const int SIZE_ARRAY = 4;
        public int[] ToArray() => new int[] { _key.X, _key.Y, _states.id.Value, _isWall ? 1 : 0 };
        public int[] ToArray(int[] array)
        {
            if (array == null || array.Length != SIZE_ARRAY)
                return ToArray();

            int i = 0;
            array[i++] = _key.X; array[i++] = _key.Y; array[i++] = _states.id.Value; array[i] = _isWall ? 1 : 0;

            return array;
        }
        #endregion

        public void Dispose()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}

