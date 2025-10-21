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
                private readonly Builder _builder;
                protected bool _done;

                #region Parent Properties
                protected AIController Human { [Impl(256)] get => _builder._parent; }
                protected int HumanId { [Impl(256)] get => _builder._parent._id; }
                protected Currencies Resources { [Impl(256)] get => _builder._parent._resources; }
                protected int FreeRoadCount { [Impl(256)] get => _builder.FreeRoadCount; }
                #endregion

                public static Plan Empty { get; } = new Dummy();

                public bool Done { [Impl(256)] get => _done; }
                public abstract bool IsValid { get; }
                
                [Impl(256)] protected Plan(Builder parent) => _builder = parent;

                public abstract IEnumerator Execution_Cn();

                sealed public override string ToString() => GetType().Name;

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
