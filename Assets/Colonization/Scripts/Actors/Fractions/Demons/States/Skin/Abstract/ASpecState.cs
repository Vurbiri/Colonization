using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        protected abstract class ASpecState : ASkinState
        {
            private readonly DemonSFX _sfx;

            public readonly WaitSignal signal = new();

            protected new DemonSFX SFX { [Impl(256)] get => _sfx; }

            public ASpecState(string stateName, ActorSkin parent, DemonSFX sfx) : base(stateName, parent) => _sfx = sfx;

            sealed public override void Exit() => DisableAnimation();
        }
    }
}
