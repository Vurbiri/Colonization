using System.Collections;
using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            sealed private class LandBuild : Plan
            {
                private readonly ReadOnlyMainCurrencies _allCost, _edificeCost, _roadCost;
                private readonly Step[] _steps;
                private readonly int _roadsCount;
                private int _cursor;

                private LandBuild(Builder parent, Crossroad crossroad, ReadOnlyMainCurrencies cost) : base(parent)
                {
                    _steps = new Step[] { new(crossroad) };
                    _edificeCost = cost;
                }
                private LandBuild(Builder parent, List<Step> steps, ReadOnlyMainCurrencies edificeCost, ReadOnlyMainCurrencies roadCost, ReadOnlyMainCurrencies allCost) : base(parent)
                {
                    _roadsCount = steps.Count - 1;
                    _steps = new Step[_roadsCount + 1];
                    for(int i = _roadsCount, j = 0; i >= 0; i--, j++)
                        _steps[i] = steps[j];

                    _edificeCost = edificeCost; _roadCost = roadCost; _allCost = allCost;
                }

                public override bool IsValid
                {
                    get
                    {
                        bool isValid = FreeRoadCount >= (_roadsCount - _cursor) && _steps[_cursor].crossroad.IsRoadConnect(HumanId); 
                        for (int i = _cursor; isValid & i < _roadsCount; i++ )
                            isValid = _steps[i].link.IsEmpty;

                        return isValid && _steps[_roadsCount].crossroad.CanBuild();
                    }
                }

                public override IEnumerator Execution_Cn()
                {
                    if (!_done)
                    {
                        if (_cursor < _roadsCount)
                            yield return BuyRoads_Cn();

                        if (_cursor == _roadsCount)
                            yield return BuyCamp_Cn();
                    }
                    yield return s_waitRealtime.Restart();
                }

                private IEnumerator BuyRoads_Cn()
                {
                    Step step;
                    bool canRoadBuild = _cursor > 0 || Resources >= _allCost || s_settings.chanceIncomplete.Roll;
                    while (canRoadBuild)
                    {
                        yield return Human.Exchange_Cn(_roadCost, Out<bool>.Get(out int key));
                        if (canRoadBuild = Out<bool>.Result(key))
                        {
                            step = _steps[_cursor]; _steps[_cursor++] = null;
                            yield return GameContainer.CameraController.ToPositionControlled(step.link.Position);
                            yield return Human.BuyRoad(step.crossroad.Type, step.link, _roadCost);
                            yield return s_waitRealtime.Restart();
                            canRoadBuild = _cursor < _roadsCount;
                            Log.Info($"[Builder::LandBuild] Player {HumanId} built Road");
                        }
                    }
                }

                private IEnumerator BuyCamp_Cn()
                {
                    yield return Human.Exchange_Cn(_edificeCost, Out<bool>.Get(out int key));
                    if (Out<bool>.Result(key))
                    {
                        var crossroad = _steps[_cursor].crossroad;
                        yield return GameContainer.CameraController.ToPositionControlled(crossroad.Position);
                        yield return Human.BuyEdificeUpgrade(crossroad, _edificeCost); ;
                        Log.Info($"[Builder::LandBuild] Player {HumanId} built Camp");
                        _done = true;
                    }
                }

                public static IEnumerator Create_Cn(Builder parent, Plans plans)
                {
                    bool canColony = parent.Abilities.IsGreater(HumanAbilityId.MaxColony, parent.Colonies.Count);
                    bool canShrine = s_shrinesCount < HEX.VERTICES && parent.Colonies.Count > 0;

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
                    static IEnumerator BuildOnRoad(Builder parent, Plans plans, bool canColony, bool canShrine)
                    {
                        var prices = GameContainer.Prices.Edifices;
                        HashSet<Crossroad> crossroads = new(parent.Roads.CrossroadsCount);
                        if (canColony)
                            parent.Roads.GetCrossroads(crossroads);
                        else
                            parent.Roads.GetDeadEnds(crossroads);

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
                                    plans.Add(new LandBuild(parent, crossroad, cost), weight);
                            }
                        }

                        yield break;
                    }
                    //===============================================
                    static IEnumerator BuildOnLand(Builder parent, Finder finder, Plans plans)
                    {
                        HashSet<Crossroad> starting = new(parent.Roads.CrossroadsCount + parent.Ports.Count);

                        parent.Roads.GetDeadEnds(starting);

                        if (starting.Count == 0)
                        {
                            starting.UnionWith(parent.Ports);
                            parent.Roads.GetCrossroads(starting);
                        }

                        yield return null;

                        finder.Run(starting);

                        yield return null;

                        var prices = GameContainer.Prices.Edifices;
                        List<Step> steps = new(); int weight, roadCount; bool repeat = false;
                        ReadOnlyMainCurrencies allCost, edificeCost, roadCost = GameContainer.Prices.Road;
                        System.Func<Crossroad, int, int> GetWeight = parent.Colonies.Count == 0 ? parent.GetFirstColonyWeight : parent.GetColonyWeight;

                        do
                        {
                            foreach (var crossroad in finder.Ending)
                            {
                                finder.CreateSteps(crossroad, steps);
                                roadCount = steps.Count - 1;

                                edificeCost = prices[crossroad.NextId];
                                allCost = edificeCost + roadCost * roadCount;

                                weight = crossroad.Weight + GetEdificeWeight(crossroad.NextId) + parent.GetCostWeight(allCost);
                                if (crossroad.NextGroupId == EdificeGroupId.Colony)
                                    weight += GetWeight(crossroad, roadCount);

                                if (weight > 0)
                                    plans.Add(new LandBuild(parent, steps, edificeCost, roadCost, allCost), weight);

                                yield return null;
                            }

                            if(repeat)
                            {
                                repeat = false;
                                if(plans.Count == 0)
                                {
                                    parent.Resources.Add(GameContainer.Prices.Edifices[EdificeId.PortTwo]);
                                    PortBuild.Create(parent, plans);
                                }
                            }
                            else
                            {
                                repeat = plans.Count == 0 & parent.Colonies.Count == 0;
                                GetWeight = (_, _) => 0;
                            }
                        }
                        while (repeat);
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

                #region Nested Finder, Step
                // *********************************************************************************
                private class Finder
                {
                    private readonly Dictionary<Crossroad, CrossroadLink> _links = new();
                    private readonly HashSet<Crossroad> _ending = new();
                    private readonly int _maxDepth;
                    private readonly bool _canColony, _canShrine;

                    public HashSet<Crossroad> Ending { [Impl(256)] get => _ending;}

                    public Finder(int maxDepth, bool canColony, bool canShrine)
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
                            int depth = 0;
                            while (_links.TryGetValue(crossroad, out CrossroadLink link))
                            {
                                crossroad = link.GetOtherCrossroad(crossroad.Type);
                                depth++;
                            }
                            return depth < _maxDepth;
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
                // *********************************************************************************
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
                // *********************************************************************************
                #endregion
            }
        }
    }
}
