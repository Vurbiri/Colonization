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

            private readonly List<Crossroad> _starts = new();
            private readonly List<Plan> _plans = new();
            private readonly CurrenciesLite _profitWeight = new();
            private Plan _currentPlan = Plan.Empty;

            static Builder() => s_settings = SettingsFile.Load<BuilderSettings>();

            public Builder(AIController parent) : base(parent)
            {
                
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
                yield return _parent.BuildPort(port).signal;
            }

            public IEnumerator BuildPort_Cn()
            {
                if (GameContainer.Crossroads.BreachCount > s_settings.minBreach)
                {
                    Crossroad port = GameContainer.Crossroads.GetRandomPort();
                    yield return GameContainer.CameraController.ToPositionControlled(port);
                    yield return _parent.BuildPort(port).signal;
                }
            }

            private IEnumerator CreatePlan_Cn()
            {
                List<Plan> plans = new() { Plan.Empty };

                SetWallBuild(plans);
                SetUpgrades(plans);
                yield return null;

                _starts.Clear();
                Roads.SetDeadEnds(_starts);
                if (_starts.Count == 0)
                {
                    _starts.AddRange(Ports);
                    _starts.AddRange(Colonies);
                }

                yield break;
            }

            [Impl(256)] private void SetWallBuild(List<Plan> plans)
            {
                if (_parent.IsWallUnlock())
                {
                    var colonies = Colonies;
                    for (int i = 0; i < colonies.Count; i++)
                        CreatePlan(plans, colonies[i]);
                }

                #region Local 
                //=========================================================================
                [Impl(256)] void CreatePlan(List<Plan> plans, Crossroad crossroad)
                {
                    if (crossroad.CanWallBuild())
                        plans.Add(new WallBuild(this, crossroad, plans[^1].Weight));
                }
                #endregion
            }
            [Impl(256)] private void SetUpgrades(List<Plan> plans)
            {
                Create(plans, Ports);
                Create(plans, Colonies);

                #region Local 
                //=========================================================================
                [Impl(256)] void Create(List<Plan> plans, ReadOnlyReactiveList<Crossroad> edifice)
                {
                    for (int i = 0; i < edifice.Count; i++)
                        CreatePlan(plans, edifice[i]);
                }
                //=========================================================================
                [Impl(256)] void CreatePlan(List<Plan> plans, Crossroad crossroad)
                {
                    var next = crossroad.NextId;
                    if (next != EdificeId.Empty && _parent.IsEdificeUnlock(next))
                        plans.Add(new Upgrade(this, crossroad, plans[^1].Weight));
                }
                #endregion
            }


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
