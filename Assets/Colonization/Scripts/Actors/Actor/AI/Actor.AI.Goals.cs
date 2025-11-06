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
                public HashSet<Key> Defensed { [Impl(256)] get; } = new(CONST.DEFAULT_MAX_EDIFICES);
                public HashSet<Key> Raided { [Impl(256)] get; } = new(CONST.DEFAULT_MAX_EDIFICES);
                public TargetEnemies Enemies { [Impl(256)] get; } = new();

                #region Nested: TargetEnemies
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

                    public bool Add(ActorCode target, ActorData force)
                    {
                        if (!_targets.TryGetValue(target, out List<ActorData> supports))
                            _targets.Add(target, supports = new());
                        supports.Add(force);

                        return true;
                    }

                    public void Remove(ActorCode target, ActorData force)
                    {
                        var supports = _targets[target];
                        supports.Remove(force);
                        if (supports.Count == 0)
                            _targets.Remove(target);
                    }
                }
                //********************************************
                #endregion
            }
        }
    }
}