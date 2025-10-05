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

                protected AIController Controller { [Impl(256)] get => _parent._parent; }

                public static Plan Empty { get; } = new Dummy();

                public int Weight { [Impl(256)] get => _weight; }
                public bool Done { [Impl(256)] get => _done; }
                public bool IsWeightValid { [Impl(256)] get => _weight > s_settings.minWeight; }
                public abstract bool IsValid { get; }
                
                [Impl(256)] protected Plan(Builder parent)
                {
                    _parent = parent;
                }

                [Impl(256)] public void WeightAdd(int weight) => _weight += weight;

                public abstract IEnumerator Appeal_Cn();

                public static bool operator >(Plan a, int b) => a._weight > b;
                public static bool operator <(Plan a, int b) => a._weight < b;

                public static bool operator >=(Plan a, int b) => a._weight >= b;
                public static bool operator <=(Plan a, int b) => a._weight <= b;

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
