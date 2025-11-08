using System.Collections.Generic;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public partial class AI
        {
            public class Goals
            {
                private readonly HashSet<Key> _home = new(CONST.DEFAULT_MAX_EDIFICES);

                public HashSet<Key> Home { [Impl(256)] get => _home; } 
                public TargetEnemies Enemies { [Impl(256)] get; } = new();
                public RaidTargets Raid { [Impl(256)] get; } = new();

                public bool CanGoHome(Crossroad target) => !_home.Contains(target.Key);

                #region Nested: TargetEnemies, RaidTargets
                //********************************************
                public class TargetEnemies
                {
                    private readonly Dictionary<ActorCode, List<ActorData>> _targets = new();

                    [Impl(256)] public bool CanAdd(Actor target) => CanAdd(target, target.CurrentForce);
                    public bool CanAdd(Actor target, int enemiesForce)
                    {
                        int alliesForce = target.GetCurrentForceEnemiesNear();

                        if (_targets.TryGetValue(target, out List<ActorData> supports))
                        {
                            for (int i = supports.Count - 1; i >= 0; i--)
                                alliesForce += supports[i].force;
                        }

                        return (enemiesForce << 1) > alliesForce;
                    }

                    public bool Add(ActorCode target, ActorData actor)
                    {
                        if (!_targets.TryGetValue(target, out List<ActorData> supports))
                            _targets.Add(target, supports = new(3));
                        supports.Add(actor);

                        return true;
                    }

                    [Impl(256)] public void Remove(ActorCode target, ActorData actor) => _targets[target].Remove(actor);

                }
                //********************************************
                public class RaidTargets
                {
                    private readonly Dictionary<Key, List<ActorCode>> _targets = new();

                    public bool CanAdd(Crossroad target) => !(_targets.TryGetValue(target.Key, out List<ActorCode> raiders) && raiders.Count > 1);

                    public bool Add(Key target, ActorCode actor)
                    {
                        if (!_targets.TryGetValue(target, out List<ActorCode> raiders))
                            _targets.Add(target, raiders = new(2));
                        raiders.Add(actor);

                        return true;
                    }

                    [Impl(256)] public void Remove(Key target, ActorCode actor) => _targets[target].Remove(actor);
                }
                //********************************************
                #endregion
            }
        }
    }
}