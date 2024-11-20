using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization
{
    public partial class Player
    {
        private partial class Objects
        {
            public IReadOnlyReactive<int> GetAbilityReactive(Id<PlayerAbilityId> id) => abilities[id];

            public void ShrinePassiveProfit() => resources.AddBlood(abilities.GetValue(PlayerAbilityId.ShrinePassiveProfit) * edifices.shrines.Count);
            public void GateAction()
            {
                resources.AddBlood(abilities.GetValue(PlayerAbilityId.ShrineProfit) * edifices.shrines.Count);
                resources.ClampMain();
            }
            public void ProfitFromEdifices(int hexId, ACurrencies freeGroundRes)
            {
                if (abilities.IsTrue(PlayerAbilityId.IsFreeGroundRes) && freeGroundRes != null)
                    resources.AddFrom(freeGroundRes);

                foreach (var crossroad in edifices.ports)
                    resources.AddFrom(crossroad.ProfitFromPort(hexId, abilities.GetValue(PlayerAbilityId.PortsAddRes)));

                foreach (var crossroad in edifices.urbans)
                    resources.AddFrom(crossroad.ProfitFromUrban(hexId, abilities.GetValue(PlayerAbilityId.CompensationRes)));
            }

            public bool CanEdificeUpgrade(Crossroad crossroad)
            {
                Id<EdificeGroupId> upGroup = crossroad.NextGroupId;
                Id<EdificeId> id = crossroad.NextId;

                if (crossroad.GroupId != EdificeGroupId.None)
                {
                    if ((id == EdificeId.LighthouseOne | id == EdificeId.LighthouseTwo) && !abilities.IsTrue(PlayerAbilityId.IsLighthouse))
                        return false;

                    if (id == EdificeId.Capital && !abilities.IsTrue(PlayerAbilityId.IsCapital))
                        return false;

                    return true;
                }

                return abilities.IsGreater(upGroup.ToState(), edifices.values[upGroup].Count);
            }
            public bool CanRoadBuild() => abilities.IsGreater(PlayerAbilityId.MaxRoads, roads.Count);
            public bool CanRecruitingWarrior(Id<WarriorId> id) => abilities.IsTrue(id.ToState());
            public bool IsNotMaxWarriors() => abilities.IsGreater(PlayerAbilityId.MaxWarrior, warriors.Count);

            public void BuyEdificeUpgrade(Crossroad crossroad)
            {
                if (crossroad.BuyUpgrade(id))
                {
                    resources.Pay(_prices.Edifices[crossroad.Id.Value]);
                    edifices.values[crossroad.GroupId].TryAdd(crossroad);
                }
            }
            public void BuyWall(Crossroad crossroad)
            {
                if (crossroad.BuyWall(id, abilities.GetAbility(PlayerAbilityId.WallDefence)))
                {
                    resources.Pay(_prices.Wall);
                    edifices.values[crossroad.GroupId].ChangeSignal(crossroad);
                }
            }
            public void BuyRoad(CrossroadLink link)
            {
                resources.Pay(_prices.Road);
                roads.BuildAndUnion(link);
            }

            public void RecruitingWarrior(int id, Hexagon startHex)
            {
                resources.Pay(_prices.Warriors[id]);
                warriors.Add(_spawner.Create(id, startHex));
            }

        }
    }
}
