using System.Collections;
using System.Collections.Generic;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        sealed private partial class Builder : Counselor
        {
            private static readonly BuilderSettings s_settings;

            private readonly System.Random _random;
            private readonly List<Crossroad> _starts = new();
            private readonly List<Plan> _plans = new();
            private readonly MainCurrencies _profitWeights = new();
            private Plan _currentPlan = Plan.Empty;

            static Builder()
            {
                s_settings = SettingsFile.Load<BuilderSettings>();
                s_settings.profitWeight = -s_settings.profitWeight;
            }

            public Builder(AIController parent) : base(parent)
            {
                _random = new((int)System.DateTime.Now.Ticks >> Id);
            }

            public override IEnumerator Appeal_Cn()
            {
                if (!_currentPlan.IsValid || _currentPlan.Done)
                    yield return CreatePlan_Cn();

                yield return _currentPlan.Appeal_Cn();
            }

            public IEnumerator BuildFirstPort_Cn()
            {
                Crossroad port = GameContainer.Crossroads.GetRandomPort();
                yield return GameContainer.CameraController.ToPositionControlled(port);
                yield return Human.BuildPort(port).signal;
            }

            private IEnumerator CreatePlan_Cn()
            {
                List<Plan> plans = new() { Plan.Empty };

                SetProfitWeight();
                yield return null;

                SetWallBuild(plans);
                SetUpgrades(plans);
                yield return null;

                PortBuild.Create(this, plans);
                yield return null;

                _currentPlan = GetPlan(plans);

                _starts.Clear();
                Roads.SetDeadEnds(_starts);
                if (_starts.Count == 0)
                {
                    _starts.AddRange(Ports);
                    _starts.AddRange(Colonies);
                }

                yield break;

                #region Local
                // ======================================
                [Impl(256)] void SetProfitWeight()
                {
                    _profitWeights.Clear();
                    var colonies = Colonies;
                    for (int i = 0; i < colonies.Count; i++)
                        colonies[i].AddNetProfit(_profitWeights);

                    _profitWeights.Normalize(s_settings.profitWeight);
                }
                // ======================================
                [Impl(256)] void SetWallBuild(List<Plan> plans)
                {
                    if (Human.IsWallUnlock())
                    {
                        var colonies = Colonies;
                        for (int i = 0; i < colonies.Count; i++)
                            WallBuild.Create(this, plans, colonies[i]);
                    }
                }
                // ======================================
                [Impl(256)] void SetUpgrades(List<Plan> plans)
                {
                    Create(plans, Ports);
                    Create(plans, Colonies);

                    // ====== Local ========
                    [Impl(256)] void Create(List<Plan> plans, ReadOnlyReactiveList<Crossroad> edifice)
                    {
                        for (int i = 0; i < edifice.Count; i++)
                            Upgrade.Create(this, plans, edifice[i]);
                    }
                }
                // ======================================
                Plan GetPlan(List<Plan> plans)
                {
                    if(plans.Count == 1) return plans[0];

                    int weight = _random.Next(plans[^1].Weight);
                    int min = 0, max = plans.Count, current;
                    while (true)
                    {
                        current = min + max >> 1;
                        if (plans[current] <= weight)
                            min = current;
                        else if (plans[current - 1] > weight)
                            max = current;
                        else
                            return plans[current];
                    }
                }
                // ======================================
                #endregion
            }

            [Impl(256)] private int GetProfitWeight(List<Hexagon> hexagons)
            {
                int weight = 0;
                for (int i = 0; i < Crossroad.HEX_COUNT; i++)
                    weight += _profitWeights[hexagons[i].GetProfit()];
                return weight;
            }
            [Impl(256)] private int GetCostWeight(ReadOnlyMainCurrencies cost) => Resources.Delta(cost) * s_settings.costWeight;
            [Impl(256)] private static int GetEdificeWeight(int id) => s_settings.edificeWeight[id];

            //private class Plan
            //{
            //    private readonly Queue<Crossroad> _queue = new();
            //    private readonly Id<PlayerId> _id;

            //    public bool ready = false;

            //    public bool Done
            //    {
            //        get
            //        {
            //            bool done = true;
            //            if(_queue.Count > 0)
            //            {

            //            }
            //            return done;
            //        }
            //    }

            //    public Plan() { }

            //    private Plan(AIController parent, Crossroad crossroad, bool isReady)
            //    {
            //        _id = parent.Id;
            //        ready = isReady;
            //        _queue.Enqueue(crossroad);
            //    }

            //    public static bool TryCreate(AIController parent, Crossroad crossroad, out Plan plan)
            //    {
            //        plan = null;
            //        bool ready = parent.IsEdificeUnlock(crossroad.NextId) && parent.CanEdificeUpgrade(crossroad);
            //        bool valid = ready || parent.CanRoadBuild(crossroad);
            //        if (valid)
            //            plan = new(parent, crossroad, ready);
            //        return valid;
            //    }
            //}
        }
    }
}
