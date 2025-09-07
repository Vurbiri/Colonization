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

            public ASpecState(int idParam, ActorSkin parent, DemonSFX sfx) : base(idParam, parent) => _sfx = sfx;

            sealed public override void Exit() => DisableAnimation();
        }
    }
}
