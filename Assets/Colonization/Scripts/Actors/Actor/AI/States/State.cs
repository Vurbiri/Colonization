using System.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public partial class Actor
    {
        public abstract partial class AI<TSettings, TActorId, TStateId>
        {
            protected abstract class State
            {
                protected readonly AI<TSettings, TActorId, TStateId> _parent;

                #region Parent Properties
                protected Actor Actor { [Impl(256)] get => _parent._actor; }
                protected Hexagon Hexagon { [Impl(256)] get => _parent._actor._currentHex; }
                protected Id<PlayerId> OwnerId { [Impl(256)] get => _parent._actor._owner; }
                protected States Action { [Impl(256)] get => _parent._actor._states; }
                protected Goals Goals { [Impl(256)] get => _parent._goals; }
                protected Status Status { [Impl(256)] get => _parent._status; }
                protected ActorAISettings Settings { [Impl(256)] get => _parent._aISettings; }
                protected bool IsInCombat { [Impl(256)] get => _parent._status.nearEnemies.IsForce; }
                protected bool IsEnemyComing { [Impl(256)] get => _parent._status.nighEnemies.IsForce; }
                #endregion

                [Impl(256)] protected State(AI<TSettings, TActorId, TStateId> parent) => _parent = parent;

                public abstract bool TryEnter();
                public abstract void Dispose();
                public abstract IEnumerator Execution_Cn(Out<bool> isContinue);

                sealed public override string ToString() => GetType().Name;

                [Impl(256)] protected void Exit()
                {
                    Dispose();
                    _parent._current = _parent._goalSetting;
                }
            }
        }
    }
}

