//Assets\Colonization\Scripts\Players\Player\HumanEdifices.cs
using System;
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
        protected class Edifices : IDisposable
        {
            private readonly AbilitiesSet<HumanAbilityId> _abilities;
            private AAbility<HumanAbilityId> _shrinePassiveProfit, _shrineProfit, _portsProfit, _compensationRes;

            public readonly IdArray<EdificeGroupId, ReactiveList<Crossroad>> values = new();

            public readonly ReactiveList<Crossroad> shrines;
            public readonly ReactiveList<Crossroad> ports;
            public readonly ReactiveList<Crossroad> urbans;

            public int ShrinePassiveProfit => _shrinePassiveProfit.Value * shrines.Count;
            public int ShrineProfit => _shrineProfit.Value * shrines.Count;
            
            public Edifices(AbilitiesSet<HumanAbilityId> abilities)
            {
                _abilities = abilities;
                GetAbilities();

                values[EdificeGroupId.Shrine] = shrines = new();
                values[EdificeGroupId.Port] = ports = new();
                values[EdificeGroupId.Urban] = urbans = new();
            }

            public Edifices(Id<PlayerId> playerId, Dictionary<int, List<EdificeLoadData>> data, Crossroads crossroads, AbilitiesSet<HumanAbilityId> abilities)
            {
                _abilities = abilities;
                GetAbilities();
                var abilityWall = abilities[HumanAbilityId.WallDefence];

                values[EdificeGroupId.Shrine] = CreateEdifices(ref shrines, data[EdificeGroupId.Shrine], playerId, crossroads, abilityWall);
                values[EdificeGroupId.Port] = CreateEdifices(ref ports, data[EdificeGroupId.Port], playerId, crossroads, abilityWall);
                values[EdificeGroupId.Urban] = CreateEdifices(ref urbans, data[EdificeGroupId.Urban], playerId, crossroads, abilityWall);
            }

            public CurrenciesLite ProfitFromEdifices(int hexId)
            {
                CurrenciesLite profit = new();

                for (int i = 0; i < ports.Count; i++)
                    profit += ports[i].ProfitFromPort(hexId, _portsProfit.Value);

                for (int i = 0; i < urbans.Count; i++)
                    profit += urbans[i].ProfitFromUrban(hexId, _compensationRes.Value);

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

                    if (id == EdificeId.Capital && !_abilities.IsTrue(HumanAbilityId.IsCapital))
                        return false;

                    return true;
                }

                return _abilities.IsGreater(nextGroup.ToState(), values[nextGroup].Count);
            }

            public void Dispose()
            {
                for (int i = values.Count - 1; i >= 0; i--)
                    values[i].Dispose();
            }

            private void GetAbilities()
            {
                _shrinePassiveProfit = _abilities[HumanAbilityId.ShrinePassiveProfit];
                _shrineProfit = _abilities[HumanAbilityId.ShrineProfit];
                _portsProfit = _abilities[HumanAbilityId.PortsProfit];
                _compensationRes = _abilities[HumanAbilityId.CompensationRes];
            }

            private ReactiveList<Crossroad> CreateEdifices(ref ReactiveList<Crossroad> values, List<EdificeLoadData> loadData, Id<PlayerId> playerId, Crossroads crossroads, IReactive<int> abilityWall)
            {
                int count = loadData.Count;
                values = new(count);

                EdificeLoadData data;
                Crossroad crossroad;
                for (int i = 0; i < count; i++)
                {
                    data = loadData[i];
                    crossroad = crossroads[data.key];
                    crossroad.BuildEdifice(playerId, data.id);
                    if (data.isWall)
                        crossroad.BuyWall(playerId, abilityWall);
                    values.Add(crossroad);
                }

                return values;
            }
        }
    }
}
