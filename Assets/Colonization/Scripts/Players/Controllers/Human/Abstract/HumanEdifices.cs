using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Colonization.Storage;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Human
    {
        protected class Edifices
        {
            private readonly ReadOnlyAbilities<HumanAbilityId> _abilities;

            public readonly IdArray<EdificeGroupId, ReactiveList<Crossroad>> edifices = new();

            public readonly ReactiveList<Crossroad> shrines;
            public readonly ReactiveList<Crossroad> colonies;
            public readonly ReactiveList<Crossroad> ports;

            public int ShrinePassiveProfit { [Impl(256)] get => _abilities[HumanAbilityId.ShrinePassiveProfit].Value * (shrines.Count + 1); }
            public int ShrineProfit { [Impl(256)] get => _abilities[HumanAbilityId.ShrineProfit].Value * shrines.Count; }

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

                edifices[EdificeGroupId.Shrine] = shrines = new(CONST.DEFAULT_MAX_EDIFICES);
                edifices[EdificeGroupId.Colony] = colonies = new(CONST.DEFAULT_MAX_EDIFICES);
                edifices[EdificeGroupId.Port] = ports = new(CONST.DEFAULT_MAX_EDIFICES);
            }

            public Edifices(Human parent, Dictionary<int, List<EdificeLoadData>> data) : this(parent._abilities)
            {
                for (int i = 0; i < EdificeGroupId.Count; i++)
                    CreateEdifices(edifices[i], data[i], parent._id, GameContainer.Crossroads);

                s_shrinesCount.Add(shrines.Count);
            }

            public LiteCurrencies ProfitFromEdifices(int hexId)
            {
                LiteCurrencies profit = new();

                for (int i = ports.Count - 1; i >= 0; i--)
                    ports[i].ProfitFromPort(profit, hexId, _abilities[HumanAbilityId.PortsProfitShift]);

                for (int i = colonies.Count - 1; i >= 0; i--)
                    profit.Add(colonies[i].ProfitFromColony(hexId, _abilities[HumanAbilityId.CompensationRes]));

                return profit;
            }

            [Impl(256)] public bool CanEdificeUpgrade(Crossroad crossroad)
            {
                return crossroad.GroupId != EdificeGroupId.None || _abilities.IsGreater(crossroad.NextGroupId.ToState(), edifices[crossroad.NextGroupId].Count);
            }
            [Impl(256)] public bool IsEdificeUnlock(Id<EdificeId> id)
            {
                bool IsLighthouse = (id != EdificeId.LighthouseOne & id != EdificeId.LighthouseTwo) || _abilities.IsTrue(HumanAbilityId.IsLighthouse);
                bool IsCity = id != EdificeId.City || _abilities.IsTrue(HumanAbilityId.IsCity);

                return IsLighthouse & IsCity;
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
