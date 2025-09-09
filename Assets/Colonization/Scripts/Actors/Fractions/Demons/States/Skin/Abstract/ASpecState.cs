using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public partial class ADemonSkin
    {
        protected abstract class ASpecState : ASkinState
        {
            private readonly DemonSFX _sfx;

            public readonly WaitSignal signal = new();

            protected new DemonSFX SFX { [Impl(256)] get => _sfx; }

            [Impl(256)] public ASpecState(ActorSkin parent, DemonSFX sfx) : base(parent) => _sfx = sfx;
        }
    }
}
