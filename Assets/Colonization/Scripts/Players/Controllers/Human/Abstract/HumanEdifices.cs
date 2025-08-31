using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public partial class Human
    {
        protected class Edifices
        {
            private readonly ReadOnlyAbilities<HumanAbilityId> _abilities;
            private readonly Ability _shrinePassiveProfit, _shrineProfit, _portsProfit, _compensationRes;

            public readonly IdArray<EdificeGroupId, ReactiveList<Crossroad>> edifices = new();

            public readonly ReactiveList<Crossroad> shrines;
            public readonly ReactiveList<Crossroad> colonies;
            public readonly ReactiveList<Crossroad> ports;

            public int ShrinePassiveProfit => _shrinePassiveProfit.Value * (shrines.Count + 1);
            public int ShrineProfit => _shrineProfit.Value * shrines.Count;

            public bool Interactable
            {
                set
                {
                    for (int j = 0; j < EdificeGroupId.Count; j++)
                    {
                        var list = edifices[j];
                        for (int i = list.Count - 1; i >= 0; i--)
                            list[i].Interactable = value;
                    }
                }
            }

            public Edifices(ReadOnlyAbilities<HumanAbilityId> abilities)
            {
                _abilities = abilities;
                _shrinePassiveProfit = _abilities[HumanAbilityId.ShrinePassiveProfit];
                _shrineProfit = _abilities[HumanAbilityId.ShrineProfit];
                _portsProfit = _abilities[HumanAbilityId.PortsProfitShift];
                _compensationRes = _abilities[HumanAbilityId.CompensationRes];

                edifices[EdificeGroupId.Shrine] = shrines = new(CONST.DEFAULT_MAX_EDIFICES);
                edifices[EdificeGroupId.Colony] = colonies = new(CONST.DEFAULT_MAX_EDIFICES);
                edifices[EdificeGroupId.Port] = ports = new(CONST.DEFAULT_MAX_EDIFICES);
            }

            public Edifices(Human parent, Dictionary<int, List<EdificeLoadData>> data) : this(parent._abilities)
            {
                for (int i = 0; i < EdificeGroupId.Count; i++)
                    CreateEdifices(edifices[i], data[i], parent._id, GameContainer.Crossroads);
            }

            public CurrenciesLite ProfitFromEdifices(int hexId)
            {
                CurrenciesLite profit = ProfitFromPorts(hexId);

                for (int i = colonies.Count - 1; i >= 0; i--)
                    profit.Add(colonies[i].ProfitFromColony(hexId, _compensationRes.Value));

                return profit;
            }

            public CurrenciesLite ProfitFromPorts(int hexId)
            {
                CurrenciesLite profit = new();

                for (int i = ports.Count - 1; i >= 0; i--)
                    profit.Add(ports[i].ProfitFromPort(hexId, _portsProfit.Value));

                return profit;
            }

            public bool CanEdificeUpgrade(Crossroad crossroad)
            {
                Id<EdificeGroupId> nextGroup = crossroad.NextGroupId;

                if (crossroad.GroupId != EdificeGroupId.None)
                    return nextGroup != EdificeGroupId.None;

                return _abilities.IsGreater(nextGroup.ToState(), edifices[nextGroup].Count);
            }
            public bool IsEdificeUnlock(Id<EdificeId> id)
            {
                if ((id == EdificeId.LighthouseOne | id == EdificeId.LighthouseTwo))
                    return _abilities.IsTrue(HumanAbilityId.IsLighthouse);

                if (id == EdificeId.City)
                    return _abilities.IsTrue(HumanAbilityId.IsCity);

                return true;
            }

            private void CreateEdifices(ReactiveList<Crossroad> values, List<EdificeLoadData> loadData, Id<PlayerId> playerId, Crossroads crossroads)
            {
                int count = loadData.Count;
                EdificeLoadData data; Crossroad crossroad;
                for (int i = 0; i < count; i++)
                {
                    data = loadData[i];
                    crossroad = crossroads[data.key];
                    crossroad.BuildEdifice(playerId, data.id, false);
                    if (data.isWall)
                        crossroad.BuildWall(playerId, false);
                    values.Add(crossroad);
                    crossroad.Interactable = false;
                }
            }
        }
    }
}
