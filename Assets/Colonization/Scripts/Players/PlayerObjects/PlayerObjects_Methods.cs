using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class PlayerObjects
    {
        public IReadOnlyReactiveValue<int> GetAbilityReactive(Id<PlayerAbilityId> id) => _abilities[id];

        public void ShrinePassiveProfit() => _resources.AddBlood(_abilities.GetValue(PlayerAbilityId.ShrinePassiveProfit) * _edifices.shrines.Count);
        public void GateAction()
        {
            _resources.AddBlood(_abilities.GetValue(PlayerAbilityId.ShrineProfit) * _edifices.shrines.Count);
            _resources.ClampMain();
        }
        public void ProfitFromEdifices(int hexId, ACurrencies freeGroundRes)
        {
            if (_abilities.IsTrue(PlayerAbilityId.IsFreeGroundRes) && freeGroundRes != null)
                _resources.AddFrom(freeGroundRes);

            foreach (var crossroad in _edifices.ports)
                _resources.AddFrom(crossroad.ProfitFromPort(hexId, _abilities.GetValue(PlayerAbilityId.PortsAddRes)));

            foreach (var crossroad in _edifices.urbans)
                _resources.AddFrom(crossroad.ProfitFromUrban(hexId, _abilities.GetValue(PlayerAbilityId.CompensationRes), _abilities.GetValue(PlayerAbilityId.WallDefence)));
        }

        public bool CanEdificeUpgrade(Crossroad crossroad)
        {
            Id<EdificeGroupId> upGroup = crossroad.NextGroupId;
            Id<EdificeId> id = crossroad.NextId;

            if(crossroad.GroupId != EdificeGroupId.None)
            {
                if ((id == EdificeId.LighthouseOne | id == EdificeId.LighthouseTwo) && !_abilities.IsTrue(PlayerAbilityId.IsLighthouse))
                    return false;

                if(id == EdificeId.Capital && !_abilities.IsTrue(PlayerAbilityId.IsCapital))
                    return false;

                return true;
            }

            return _abilities.IsGreater(upGroup.ToState(), _edifices.values[upGroup].Count);
        }
        public bool CanWallBuild() => _abilities.IsTrue(PlayerAbilityId.IsWall);
        public bool CanRoadBuild() => _abilities.IsGreater(PlayerAbilityId.MaxRoads, _roads.Count);
        public bool CanRecruitingWarrior(Id<WarriorId> id) => _abilities.IsTrue(id.ToState());
        public bool IsNotMaxWarriors() => _abilities.IsGreater(PlayerAbilityId.MaxWarrior, WarriorsCount);

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
