using System.Collections;
using Vurbiri.Colonization.Characteristics;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            private abstract class Plan
            {
                private readonly Builder _parent;
                protected bool _done;

                protected AIController Human { [Impl(256)] get => _parent._parent; }
                public Currencies Resources { [Impl(256)] get => _parent._parent._resources; }
                protected int Id { [Impl(256)] get => _parent._parent._id; }
                protected Roads Roads { [Impl(256)] get => _parent._parent._roads; }
                protected ReadOnlyAbilities<HumanAbilityId> Abilities { [Impl(256)] get => _parent._parent._abilities; }
                protected int FreeRoadCount { [Impl(256)] get => _parent._parent._abilities[HumanAbilityId.MaxRoad] -  _parent._parent._roads.Count; }
                protected bool CanPlay { [Impl(256)] get => _parent._parent._waitExchange.Value; }

                public static Plan Empty { get; } = new Dummy();

                public bool Done { [Impl(256)] get => _done; }
                public abstract bool IsValid { get; }
                
                [Impl(256)] protected Plan(Builder parent)
                {
                    _parent = parent;
                }
    
                public abstract IEnumerator Execution_Cn();

                public override string ToString() => GetType().Name;

                #region Nested
                // **********************************************************
                public class Dummy : Plan
                {
                    public Dummy() : base(null) { }

                    public override bool IsValid => false;

                    public override IEnumerator Execution_Cn() { yield break; }
                }
                #endregion
            }
        }
    }
}
