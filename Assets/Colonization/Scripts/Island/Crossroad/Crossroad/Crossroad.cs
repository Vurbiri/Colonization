//Assets\Colonization\Scripts\Island\Crossroad\Crossroad\Crossroad.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Reactive;
using Object = UnityEngine.Object;

namespace Vurbiri.Colonization
{
    public partial class Crossroad : IDisposable, IPositionable, ICancel
    {
        #region Fields
        private const int HEX_COUNT = 3;

        private readonly Key _key;
        private AEdifice _edifice;
        private EdificeSettings _states;
        private Id<PlayerId> _owner = PlayerId.None;
        private bool _isWall = false;
        private int _defenceWall = 0;

        private readonly GameplayTriggerBus _triggerBus;
        private readonly IReadOnlyList<AEdifice> _prefabs;
        private readonly List<Hexagon> _hexagons = new(HEX_COUNT);
        private readonly IdSet<LinkId, CrossroadLink> _links = new();

        private int _countFreeLink = 0, _countWater = 0;
        private bool _isGate = false;
        private WaitResultSource<Hexagon> _waitHexagon;
        private readonly RBool _canCancel = new();

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
        public IEnumerable<CrossroadLink> Links => _links;
        public IReactiveValue<bool> CanCancel => _canCancel;
        public Vector3 Position { get; }
        #endregion

        public Crossroad(Key key, Transform container, Vector3 position, Quaternion rotation, IReadOnlyList<AEdifice> prefabs, GameplayTriggerBus triggerBus)
        {
            _key = key;
            Position = position;
            _prefabs = prefabs;

            _triggerBus = triggerBus;

            _edifice = Object.Instantiate(_prefabs[EdificeId.Empty], position, rotation, container);
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

            if (_countFreeLink == _links.Filling)
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
            _triggerBus.TriggerCrossroadSelect(this);
        }
        public void OnUnselect(ISelectable newSelectable)
        {
            _triggerBus.TriggerUnselect();

            if (_waitHexagon == null)
                return;

            _canCancel.False();

            _waitHexagon.SetResult(newSelectable as Hexagon);
            foreach (var hex in _hexagons)
                hex.SetUnselectable();

            _waitHexagon = null;
        }
        public void Cancel() => OnUnselect(null);
        #endregion

        public void Dispose()
        {
            _unsubscriber?.Unsubscribe();
        }
    }
}

