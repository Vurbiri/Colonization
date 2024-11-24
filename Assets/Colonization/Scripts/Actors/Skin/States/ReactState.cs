//Assets\Colonization\Scripts\Actors\Skin\States\ReactState.cs
namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        private class ReactState : TriggerSwitchState
        {
            public ReactState(ActorSkin parent) : base(T_REACT, parent)
            {
                foreach (var behaviour in _animator.GetBehaviours<ReactBehaviour>())
                    behaviour.EventExit += () => _fsm.ToPrevState();
            }
        }
    }
}
