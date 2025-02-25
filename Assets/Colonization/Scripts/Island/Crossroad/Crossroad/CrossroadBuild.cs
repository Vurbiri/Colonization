//Assets\Colonization\Scripts\Island\Crossroad\Crossroad\CrossroadBuild.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class Crossroad
	{
        #region Edifice
        public void Build(Id<PlayerId> playerId, int idBuild, bool isWall)
        {
            _isWall = isWall;
            BuildEdifice(playerId, idBuild);
        }
        public bool CanUpgrade(Id<PlayerId> playerId)
        {
            return _states.isUpgrade && (_owner == playerId ||
            _states.nextGroupId.Value switch
            {
                EdificeGroupId.Shrine => IsRoadConnect(playerId),
                EdificeGroupId.Port => WaterCheck(),
                EdificeGroupId.Urban => NeighborCheck(playerId),
                _ => false
            });

            #region Local: WaterCheck(), NeighborCheck()
            //=================================
            bool WaterCheck()
            {
                if (_countFreeLink == 0 && !IsRoadConnect(playerId))
                    return false;

                foreach (var hex in _hexagons)
                    if (hex.IsOwnedByPort)
                        return false;

                return true;
            }
            //=================================
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
            if (!_states.isUpgrade | (_states.id != EdificeId.Signpost & _owner != playerId))
                return false;

            BuildEdifice(playerId, _states.nextId.Value);
            return true;
        }
        private void BuildEdifice(Id<PlayerId> playerId, int buildId)
        {
            _owner = playerId;
            _edifice = Object.Instantiate(_prefabs[buildId]).Init(_owner, _isWall, _links, _edifice);
            _edifice.Subscribe(OnSelect, OnUnselect);
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
            return true;
        }

        public bool CanRoadBuild(Id<PlayerId> playerId) => _countFreeLink > 0 && IsRoadConnect(playerId);
        public void RoadBuilt(Id<LinkId> id)
        {
            _countFreeLink--;
            _edifice.AddRoad(id, _isWall);
        }
        #endregion

        #region Road
        public bool IsFullyOwned(Id<PlayerId> playerId)
        {
            if (_links.CountAvailable <= 1)
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
        public bool CanRecruitingWarriors(Id<PlayerId> playerId)
        {
            int busyCount = 0;
            foreach (var hex in _hexagons)
                if (!hex.CanActorEnter)
                    busyCount++;

            return busyCount < _hexagons.Count && _owner == playerId && _states.groupId == EdificeGroupId.Port;
        }

        public WaitResult<Hexagon> GetHexagonForRecruiting_Wt(bool isNotDemon = true)
        {
            _waitHexagon = new();
            List<Hexagon> empty = new(2);

            foreach (var hex in _hexagons)
                if (hex.CanActorEnter)
                    empty.Add(hex);

            if (empty.Count == 0)
                return _waitHexagon.Cancel();

            Debug.Log("Сразу ли спаунить на одной ???");
            if (empty.Count == 1)
                return _waitHexagon.SetResult(empty[0]);

            foreach (var hex in empty)
                hex.TrySetSelectableFree(isNotDemon);

            _canCancel.Value = true;
            return _waitHexagon;
        }
        #endregion
    }
}
