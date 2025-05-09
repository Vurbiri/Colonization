//Assets\Colonization\Scripts\Island\Crossroad\Crossroad\CrossroadBuild.cs
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class Crossroad
	{
        #region Edifice
        public bool CanUpgrade(Id<PlayerId> playerId)
        {
            return _states.isUpgrade && (_owner == playerId ||
            _states.nextGroupId.Value switch
            {
                EdificeGroupId.Shrine => IsRoadConnect(playerId),
                EdificeGroupId.Port   => WaterCheck(),
                EdificeGroupId.Urban  => NeighborCheck(playerId),
                _ => false
            });

            #region Local: WaterCheck(), NeighborCheck()
            //=================================
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            bool WaterCheck()
            {
                if (_countFreeLink == 0 && !IsRoadConnect(playerId))
                    return false;

                for(int i = 0; i < HEX_COUNT; i++)
                    if (_hexagons[i].IsOwnedByPort)
                        return false;

                return true;
            }
            //=================================
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            bool NeighborCheck(Id<PlayerId> playerId)
            {
                Crossroad neighbor;
                foreach (var link in _links)
                {
                    neighbor = link.Other(this);
                    if (neighbor._states.groupId == EdificeGroupId.Urban)
                        return false;
                }
                return IsRoadConnect(playerId);
            }
            #endregion
        }
        public bool BuyUpgrade(Id<PlayerId> playerId)
        {
            if (!_states.isUpgrade | (_states.id != EdificeId.Empty & _owner != playerId))
                return false;

            BuildEdifice(playerId, _states.nextId.Value);
            return true;
        }
        public void BuildEdifice(Id<PlayerId> playerId, int buildId)
        {
            _owner = playerId;
            _edifice = Object.Instantiate(_prefabs[buildId]).Init(_owner, _isWall, _links, _edifice);
            _states = _edifice.Settings;
            _states.isBuildWall = _states.isBuildWall && !_isWall;
        }

        public bool CanWallBuild(Id<PlayerId> playerId) => _owner == playerId & _states.isBuildWall & !_isWall;
        public bool BuyWall(Id<PlayerId> playerId, IReactive<int> abilityWall)
        {
            if (!_states.isBuildWall | _isWall | _owner != playerId || !_edifice.WallBuild(playerId, _links))
                return false;

            _states.isBuildWall = !(_isWall = true);
            _unsubscriber = abilityWall.Subscribe(d => _defenceWall = d);

            for (int i = 0; i < _hexagons.Count; i++)
                _hexagons[i].BuildWall(playerId);

            return true;
        }

        #endregion

        #region Road
        public bool CanRoadBuild(Id<PlayerId> playerId) => _countFreeLink > 0 && IsRoadConnect(playerId);
        public void RoadBuilt(Id<LinkId> id)
        {
            _countFreeLink--;
            _edifice.AddRoad(id, _isWall);
        }

        public bool IsFullyOwned(Id<PlayerId> playerId)
        {
            if (_links.Filling <= 1)
                return false;

            if (_countFreeLink > 0)
                return _owner == playerId;

            foreach (var link in _links)
                if (link.Owner != playerId)
                    return false;

            return true;
        }
        public bool IsRoadConnect(Id<PlayerId> playerId)
        {
            if (_owner == playerId)
                return true;

            foreach (var link in _links)
                if (link.Owner == playerId)
                    return true;

            return false;
        }
        #endregion

        #region Recruiting
        public bool CanRecruiting(Id<PlayerId> playerId)
        {
            int countUnfit = 0;
            for (int i = 0; i < HEX_COUNT; i++)
                if (!_hexagons[i].CanWarriorEnter)
                    countUnfit++;

            return countUnfit < HEX_COUNT & _owner == playerId & _states.groupId == EdificeGroupId.Port;
        }

        public WaitResult<Hexagon> GetHexagonForRecruiting_Wait(bool isNotDemon = true)
        {
            _waitHexagon = new();
            List<Hexagon> empty = new(2);

            for (int i = 0; i < HEX_COUNT; i++)
                if (_hexagons[i].CanWarriorEnter)
                    empty.Add(_hexagons[i]);

            int emptyCount = empty.Count;
            if (emptyCount == 0)
                return _waitHexagon.Cancel();

            Debug.Log("����� �� �������� �� ����� ???");
            if (emptyCount == 1)
                return _waitHexagon.SetResult(empty[0]);

            for (int i = 0;i < emptyCount; i++)
                empty[i].TrySetSelectableFree();

            _canCancel.True();
            return _waitHexagon;
        }
        #endregion
    }
}
