namespace Vurbiri.Colonization
{
    using Reactive;

    public partial class PlayerObjects
    {
        public IReadOnlyReactiveValue<int> GetStateReactive(Id<PlayerStateId> id) => _states[id];

        public void ShrinePassiveProfit()
         => _resources.AddAndClampBlood(_states.GetValue(PlayerStateId.ShrinePassiveProfit) * _edifices.shrines.Count, _states.GetValue(PlayerStateId.ShrineMaxRes));
        public void ShrineProfit()
         => _resources.AddAndClampBlood(_states.GetValue(PlayerStateId.ShrineProfit) * _edifices.shrines.Count, _states.GetValue(PlayerStateId.ShrineMaxRes));
        public void ClampMainResources() => _resources.ClampMain(_states.GetValue(PlayerStateId.MaxResources));
        public void ProfitFromEdifices(int hexId, ACurrencies freeGroundRes)
        {
            if (_states.IsTrue(PlayerStateId.IsFreeGroundRes) && freeGroundRes != null)
                _resources.AddFrom(freeGroundRes);

            foreach (var crossroad in _edifices.ports)
                _resources.AddFrom(crossroad.ProfitFromPort(hexId, _states.GetValue(PlayerStateId.PortsRatioRes)));

            foreach (var crossroad in _edifices.urbans)
                _resources.AddFrom(crossroad.ProfitFromUrban(hexId, _states.GetValue(PlayerStateId.CompensationRes), _states.GetValue(PlayerStateId.WallDefence)));
        }

        public bool CanEdificeUpgrade(Crossroad crossroad)
        {
            Id<EdificeGroupId> upGroup = crossroad.NextGroupId;
            return crossroad.GroupId != EdificeGroupId.None || _states.IsGreater(upGroup.ToState(), _edifices.values[upGroup].Count);
        }
        public bool CanWallBuild() => _states.IsTrue(PlayerStateId.IsWall);
        public bool CanRoadBuild() => _states.IsGreater(PlayerStateId.MaxRoads, _roads.Count);
        public bool CanRecruitingWarrior(Id<WarriorId> id) => _states.IsTrue(id.ToState());
        public bool IsNotMaxWarriors() => _states.IsGreater(PlayerStateId.MaxWarrior, WarriorsCount);

        public void BuyEdificeUpgrade(Crossroad crossroad)
        {
            _resources.Pay(_prices.Edifices[crossroad.Id.Value]);
            _edifices.values[crossroad.GroupId].TryAdd(crossroad);
        }
        public void BuyWall(Crossroad crossroad)
        {
            _resources.Pay(_prices.Wall);
            _edifices.values[crossroad.GroupId].ChangeSignal(crossroad);
        }
        public void BuyRoad(CrossroadLink link)
        {
            _resources.Pay(_prices.Road);
            _roads.BuildAndUnion(link);
        }

        public void RecruitingWarrior(int id, Hexagon startHex)
        {
            _resources.Pay(_prices.Warriors[id]);
            _warriors.Add(_spawner.Create(id, startHex));
        }

    }
}
