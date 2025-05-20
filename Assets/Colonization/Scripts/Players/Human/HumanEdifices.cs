//Assets\Colonization\Scripts\Players\Player\HumanEdifices.cs
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class Human
    {
        protected class Edifices
        {
            private readonly AbilitiesSet<HumanAbilityId> _abilities;
            private readonly Ability _shrinePassiveProfit, _shrineProfit, _portsProfit, _compensationRes;

            public readonly IdArray<EdificeGroupId, ReactiveList<Crossroad>> edifices = new();

            public readonly ReactiveList<Crossroad> shrines;
            public readonly ReactiveList<Crossroad> colonies;
            public readonly ReactiveList<Crossroad> ports;

            public int ShrinePassiveProfit => _shrinePassiveProfit.Value * shrines.Count;
            public int ShrineProfit => _shrineProfit.Value * shrines.Count;

            public bool Interactable
            {
                set
                {
                    for(int j = 0; j < EdificeGroupId.Count; j++)
                    {
                        var list = edifices[j];
                        for (int i = list.Count - 1; i >= 0; i--)
                            list[i].Interactable = value;
                    }
                }
            }

            public Edifices(AbilitiesSet<HumanAbilityId> abilities)
            {
                _abilities = abilities;
                _shrinePassiveProfit = _abilities[HumanAbilityId.ShrinePassiveProfit];
                _shrineProfit = _abilities[HumanAbilityId.ShrineProfit];
                _portsProfit = _abilities[HumanAbilityId.PortsProfit];
                _compensationRes = _abilities[HumanAbilityId.CompensationRes];

                edifices[EdificeGroupId.Shrine] = shrines = new(CONST.MAX_EDIFICES);
                edifices[EdificeGroupId.Colony] = colonies = new(CONST.MAX_EDIFICES);
                edifices[EdificeGroupId.Port] = ports = new(CONST.MAX_EDIFICES);
            }

            public Edifices(Human parent, Dictionary<int, List<EdificeLoadData>> data, Crossroads crossroads, bool isPlayerTurn) : this(parent._abilities)
            {
                var abilityWall = _abilities[HumanAbilityId.WallDefence];
                for (int i = 0; i < EdificeGroupId.Count; i++)
                    CreateEdifices(edifices[i], data[i], parent._id, crossroads, abilityWall, isPlayerTurn);
            }

            public CurrenciesLite ProfitFromEdifices(int hexId)
            {
                CurrenciesLite profit = new();

                for (int i = 0; i < ports.Count; i++)
                    profit += ports[i].ProfitFromPort(hexId, _portsProfit.Value);

                for (int i = 0; i < colonies.Count; i++)
                    profit += colonies[i].ProfitFromColony(hexId, _compensationRes.Value);

                return profit;
            }

            public bool CanEdificeUpgrade(Crossroad crossroad)
            {
                Id<EdificeGroupId> nextGroup = crossroad.NextGroupId;
                Id<EdificeId> id = crossroad.NextId;

                if(nextGroup == EdificeGroupId.None) return false;

                if (crossroad.GroupId != EdificeGroupId.None)
                {
                    if ((id == EdificeId.LighthouseOne | id == EdificeId.LighthouseTwo) && !_abilities.IsTrue(HumanAbilityId.IsLighthouse))
                        return false;

                    if (id == EdificeId.City && !_abilities.IsTrue(HumanAbilityId.IsCity))
                        return false;

                    return true;
                }

                return _abilities.IsGreater(nextGroup.ToState(), edifices[nextGroup].Count);
            }

            private void CreateEdifices(ReactiveList<Crossroad> values, List<EdificeLoadData> loadData, Id<PlayerId> playerId, Crossroads crossroads, IReactive<int> abilityWall, bool isPlayerTurn)
            {
                int count = loadData.Count;
                EdificeLoadData data; Crossroad crossroad;
                for (int i = 0; i < count; i++)
                {
                    data = loadData[i];
                    crossroad = crossroads[data.key];
                    crossroad.BuildEdifice(playerId, data.id);
                    if (data.isWall)
                        crossroad.BuyWall(playerId, abilityWall);
                    values.Add(crossroad);
                    crossroad.Interactable = isPlayerTurn;
                }
            }
        }
    }
}
