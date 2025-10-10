using System.Collections;
using System.Collections.Generic;
using Vurbiri.Colonization.Characteristics;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            sealed private class LandBuild : Plan
            {
                private readonly ReadOnlyMainCurrencies _edificeCost, _roadCost;
                private readonly Step[] _steps;
                private readonly int _count;
                private int _cursor;

                private LandBuild(Builder parent, Crossroad crossroad, ReadOnlyMainCurrencies cost, int weight) : base(parent, weight)
                {
                    _steps = new Step[] { new(crossroad) };
                    _edificeCost = cost;
                }
                private LandBuild(Builder parent, List<Step> steps, ReadOnlyMainCurrencies edificeCost, ReadOnlyMainCurrencies roadCost, int weight) : base(parent, weight)
                {
                    _count = steps.Count - 1;
                    _steps = new Step[_count + 1];
                    for(int i = _count, j = 0; i >= 0; i--, j++)
                        _steps[i] = steps[j];

                    _edificeCost = edificeCost; _roadCost = roadCost;
                }

                public override bool IsValid
                {
                    get
                    {
                        bool isValid = FreeRoadCount >= _count && _steps[_cursor].crossroad.IsRoadConnect(Id); 
                        for (int i = _cursor; isValid & i < _count; i++ )
                            isValid = _steps[i].link.IsEmpty;

                        return isValid && _steps[_count].crossroad.CanBuild(Id);
                    }
                }


                public override IEnumerator Appeal_Cn()
                {
                    if (!_done)
                    {
                        Step step;
                        while (_cursor < _count && Human.Exchange(_roadCost))
                        {
                            step = _steps[_cursor];
                            yield return GameContainer.CameraController.ToPositionControlled(step.link.Position);
                            yield return Human.BuyRoad(step.crossroad.Type, step.link, _roadCost);
                            _steps[_cursor++] = null;
                        }
                        yield return null;
                        if(_cursor == _count && Human.Exchange(_edificeCost))
                        {
                            var crossroad = _steps[_cursor].crossroad;
                            yield return GameContainer.CameraController.ToPositionControlled(crossroad.Position);
                            yield return Human.BuyEdificeUpgrade(crossroad, _edificeCost);
                            _done = true;
                        }
                    }
                    yield break;
                }

                public static IEnumerator Create(Builder parent, List<Plan> plans)
                {
                    bool canColony = parent.Abilities.IsGreater(HumanAbilityId.MaxColony, parent.Colonies.Count);
                    bool canShrine = s_shrinesCount < HEX.VERTICES;

                    if (canColony | canShrine)
                    {
                        int maxDepth = System.Math.Min(s_settings.searchDepth, parent.FreeRoadCount);

                        if (maxDepth == 0)
                            yield return BuildOnRoad(parent, plans, canColony, canShrine);
                        else
                            yield return BuildOnLand(parent, new(maxDepth, canColony, canShrine), plans);
                    }

                    #region Loacl
                    //===============================================
                    static IEnumerator BuildOnRoad(Builder parent, List<Plan> plans, bool canColony, bool canShrine)
                    {
                        var prices = GameContainer.Prices.Edifices;
                        HashSet<Crossroad> crossroads = new(parent.Roads.CrossroadsCount);
                        if (canColony)
                            parent.Roads.SetCrossroads(crossroads);
                        else
                            parent.Roads.SetDeadEnds(crossroads);

                        yield return null;

                        int weight; ReadOnlyMainCurrencies cost; 
                        foreach (var crossroad in crossroads)
                        {
                            if (CanBuild(crossroad, canColony, canShrine))
                            {
                                cost = prices[crossroad.NextId];
                                weight = crossroad.Weight + GetEdificeWeight(crossroad.NextId) + parent.GetCostWeight(cost);
                                if (crossroad.NextGroupId == EdificeGroupId.Colony)
                                    weight += parent.GetProfitWeight(crossroad.Hexagons);
                                if (weight > 0)
                                    plans.Add(new LandBuild(parent, crossroad, cost, weight + plans[^1].Weight));
                            }
                        }

                        yield break;
                    }
                    //===============================================
                    static IEnumerator BuildOnLand(Builder parent, Search search, List<Plan> plans)
                    {
                        HashSet<Crossroad> starting = new(parent.Roads.CrossroadsCount + parent.Ports.Count);

                        parent.Roads.SetDeadEnds(starting);

                        if (starting.Count == 0)
                        {
                            starting.UnionWith(parent.Ports);
                            parent.Roads.SetCrossroads(starting);
                        }

                        yield return null;

                        search.Run(starting);

                        yield return null;

                        var prices = GameContainer.Prices.Edifices;
                        List<Step> steps = new();
                        int weight, roadCount; 
                        ReadOnlyMainCurrencies edificeCost, roadCost = GameContainer.Prices.Road, cost;

                        foreach (var crossroad in search.Ending)
                        {
                            search.CreateSteps(crossroad, steps);
                            roadCount = steps.Count - 1;

                            edificeCost = prices[crossroad.NextId];
                            cost = edificeCost + roadCost * roadCount;

                            weight = crossroad.Weight + GetEdificeWeight(crossroad.NextId) + parent.GetCostWeight(cost) - (s_settings.penaltyPerRoad ^ roadCount);
                            if (crossroad.NextGroupId == EdificeGroupId.Colony)
                                weight += parent.GetProfitWeight(crossroad.Hexagons);
                            //Log.Info($"RoadCount {roadCount}, Weight {weight}");
                            if (weight > 0)
                                plans.Add(new LandBuild(parent, steps, edificeCost, roadCost, weight + plans[^1].Weight));

                            yield return null;
                        }

                        yield break;
                    }
                    //===============================================
                    #endregion
                }

                [Impl(256)]
                private static bool CanBuild(Crossroad crossroad, bool canColony, bool canShrine)
                {
                    var group = crossroad.NextGroupId;
                    return crossroad.CanBuild() && ((group == EdificeGroupId.Colony & canColony) | (group == EdificeGroupId.Shrine & canShrine));
                }

                private class Search
                {
                    private readonly Dictionary<Crossroad, CrossroadLink> _links = new();
                    private readonly HashSet<Crossroad> _ending = new();
                    private readonly int _maxDepth;
                    private readonly bool _canColony, _canShrine;

                    public HashSet<Crossroad> Ending { [Impl(256)] get => _ending;}

                    public Search(int maxDepth, bool canColony, bool canShrine)
                    {
                        _maxDepth = maxDepth;
                        _canColony = canColony; _canShrine = canShrine;
                    }

                    public void Run(HashSet<Crossroad> starting)
                    {
                        var crossroads = GameContainer.Crossroads;
                        Queue<Crossroad> queue = new(starting);

                        Crossroad current, linked;
                        while (queue.Count > 0)
                        {
                            current = queue.Dequeue();
                            if (CanBuild(current, _canColony, _canShrine))
                                _ending.Add(current);

                            if (IsDepth(current))
                            {
                                foreach (var link in current.Links)
                                {
                                    if (link.IsEmpty)
                                    {
                                        linked = crossroads[link.GetOther(current.Type)];
                                        if (!starting.Contains(linked) && _links.TryAdd(linked, link))
                                            queue.Enqueue(linked);
                                    }
                                }
                            }
                        }

                        // === Local ===
                        bool IsDepth(Crossroad crossroad)
                        {
                            int searchDepth = 0;
                            while (_links.TryGetValue(crossroad, out CrossroadLink link))
                            {
                                crossroad = link.GetOtherCrossroad(crossroad.Type);
                                searchDepth++;
                            }
                            return searchDepth < _maxDepth;
                        }
                    }

                    public void CreateSteps(Crossroad crossroad, List<Step> steps)
                    {
                        steps.Clear();
                        steps.Add(new(crossroad));

                        while (_links.TryGetValue(crossroad, out CrossroadLink link))
                        {
                            crossroad = link.GetOtherCrossroad(crossroad.Type);
                            steps.Add(new(crossroad, link));
                        }
                    }
                }

                private class Step
                {
                    public readonly Crossroad crossroad;
                    public readonly CrossroadLink link;

                    public Step(Crossroad crossroad, CrossroadLink link)
                    {
                        this.crossroad = crossroad;
                        this.link = link;
                    }
                    public Step(Crossroad crossroad) => this.crossroad = crossroad;
                }
            }
        }
    }
}
