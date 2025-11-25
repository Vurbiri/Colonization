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
                public TargetEnemies Enemies { [Impl(256)] get; } = new();
                public TargetColonies Colonies { [Impl(256)] get; } = new();

                #region Nested: TargetEnemies, TargetColonies
                //********************************************
                public class TargetEnemies
                {
                    private readonly Dictionary<ActorCode, List<ActorData>> _targets = new();

                    [Impl(256)] public bool CanAdd(Actor target) => CanAdd(target, target.CurrentForce);
                    public bool CanAdd(Actor target, int enemiesForce)
                    {
                        int alliesForce = target.GetCurrentForceNearEnemies();

                        if (_targets.TryGetValue(target, out List<ActorData> supports))
                        {
                            for (int i = supports.Count - 1; i >= 0; --i)
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
                public class TargetColonies
                {
                    private readonly Dictionary<Key, List<ActorCode>> _targets = new();

                    public int Count(Crossroad target)
                    {
                        if(_targets.TryGetValue(target.Key, out List<ActorCode> actors))
                            return actors.Count;
                        return 0;
                    }

                    public bool Add(Key target, ActorCode actor)
                    {
                        if (!_targets.TryGetValue(target, out List<ActorCode> actors))
                            _targets.Add(target, actors = new(3));
                        actors.Add(actor);

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