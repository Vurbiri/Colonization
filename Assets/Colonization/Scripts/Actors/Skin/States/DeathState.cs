//Assets\Colonization\Scripts\Actors\Skin\States\DeathState.cs
using System.Collections;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        protected class DeathState : ATriggerSwitchState
        {
            public DeathState(ActorSkin parent) : base(T_DEATH, parent)
            {
                _animator.GetBehaviour<DeathBehaviour>().EventExit += () => _parent.StartCoroutine(Death_Coroutine());
            }

            private IEnumerator Death_Coroutine()
            {
                yield return _sfx.Death_Coroutine();
                waitActivate.Activate();
            }
        }
    }
}
