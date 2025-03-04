//Assets\Colonization\Scripts\Players\PlayerPartial\PlayerEdifices.cs
using System;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class Player
    {
        protected class Edifices : IDisposable
        {
            private readonly IReadOnlyAbilities<PlayerAbilityId> _abilities;
            private IAbility _shrinePassiveProfit, _shrineProfit, _portsProfit, _compensationRes;

            public readonly IdArray<EdificeGroupId, ReactiveList<Crossroad>> values = new();

            public readonly ReactiveList<Crossroad> shrines;
            public readonly ReactiveList<Crossroad> ports;
            public readonly ReactiveList<Crossroad> urbans;

            public int ShrinePassiveProfit => _shrinePassiveProfit.Value * shrines.Count;
            public int ShrineProfit => _shrineProfit.Value * shrines.Count;
            
            public Edifices(IReadOnlyAbilities<PlayerAbilityId> abilities)
            {
                _abilities = abilities;
                GetAbilities();

                values[EdificeGroupId.Shrine] = shrines = new();
                values[EdificeGroupId.Port] = ports = new();
                values[EdificeGroupId.Urban] = urbans = new();
            }

            public Edifices(Id<PlayerId> playerId, IReadOnlyDictionary<int, EdificeLoadData[]> data, Crossroads crossroads, IReadOnlyAbilities<PlayerAbilityId> abilities)
            {
                _abilities = abilities;
                GetAbilities();
                var abilityWall = abilities[PlayerAbilityId.WallDefence];

                values[EdificeGroupId.Shrine] = CreateEdifices(ref shrines, data[EdificeGroupId.Shrine], playerId, crossroads, abilityWall);
                values[EdificeGroupId.Port] = CreateEdifices(ref ports, data[EdificeGroupId.Port], playerId, crossroads, abilityWall);
                values[EdificeGroupId.Urban] = CreateEdifices(ref urbans, data[EdificeGroupId.Urban], playerId, crossroads, abilityWall);
            }

            public CurrenciesLite ProfitFromEdifices(int hexId)
            {
                CurrenciesLite profit = new();

                int count = ports.Count;
                for (int i = 0; i < count; i++)
                    profit += ports[i].ProfitFromPort(hexId, _portsProfit.Value);

                count = urbans.Count;
                for (int i = 0; i < count; i++)
                    profit += urbans[i].ProfitFromUrban(hexId, _compensationRes.Value);

                return profit;
            }

            public bool CanEdificeUpgrade(Crossroad crossroad)
            {
                Id<EdificeGroupId> upGroup = crossroad.NextGroupId;
                Id<EdificeId> id = crossroad.NextId;

                if (crossroad.GroupId != EdificeGroupId.None)
                {
                    if ((id == EdificeId.LighthouseOne | id == EdificeId.LighthouseTwo) && !_abilities.IsTrue(PlayerAbilityId.IsLighthouse))
                        return false;

                    if (id == EdificeId.Capital && !_abilities.IsTrue(PlayerAbilityId.IsCapital))
                        return false;

                    return true;
                }

                return _abilities.IsGreater(upGroup.ToState(), values[upGroup].Count);
            }

            public void Dispose()
            {
                for (int i = values.Count - 1; i >= 0; i--)
                    for (int j = values[i].Count - 1; j >= 0; j--)
                        values[i][j].Dispose();
            }

            private void GetAbilities()
            {
                _shrinePassiveProfit = _abilities[PlayerAbilityId.ShrinePassiveProfit];
                _shrineProfit = _abilities[PlayerAbilityId.ShrineProfit];
                _portsProfit = _abilities[PlayerAbilityId.PortsProfit];
                _compensationRes = _abilities[PlayerAbilityId.CompensationRes];
            }

            private ReactiveList<Crossroad> CreateEdifices(ref ReactiveList<Crossroad> values, EdificeLoadData[] loadData, Id<PlayerId> playerId, Crossroads crossroads, IReactive<int> abilityWall)
            {
                int count = loadData.Length;
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
