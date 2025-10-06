using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class AIController
    {
        private partial class Builder
        {
            private abstract class Plan
            {
                protected readonly Builder _parent;
                protected bool _done;
                protected int _weight;

                protected AIController Human { [Impl(256)] get => _parent._parent; }
                protected int Id { [Impl(256)] get => _parent._parent._id; }

                public static Plan Empty { get; } = new Dummy();

                public int Weight { [Impl(256)] get => _weight; }
                public bool Done { [Impl(256)] get => _done; }
                public bool IsWeightValid { [Impl(256)] get => _weight > 0; }
                public abstract bool IsValid { get; }
                
                [Impl(256)] protected Plan(Builder parent)
                {
                    _parent = parent;
                }
                                
                [Impl(256)] public Plan WeightAdd(int weight)
                {
                    _weight += weight;
                    return this;
                }

                public abstract IEnumerator Appeal_Cn();

                public static bool operator >(Plan p, int i) => p._weight > i;
                public static bool operator <(Plan p, int i) => p._weight < i;

                public static bool operator >=(Plan p, int i) => p._weight >= i;
                public static bool operator <=(Plan p, int i) => p._weight <= i;

                #region Nested
                // **********************************************************
                public class Dummy : Plan
                {
                    public Dummy() : base(null) { }

                    public override bool IsValid => false;

                    public override IEnumerator Appeal_Cn() { yield break; }
                }
                #endregion
            }
        }
    }
}
