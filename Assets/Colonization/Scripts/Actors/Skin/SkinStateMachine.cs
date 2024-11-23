//Assets\Colonization\Scripts\Actors\Skin\SkinStateMachine.cs
using Vurbiri.FSM;
using static Vurbiri.Colonization.Actors.ActorSkin;

namespace Vurbiri.Colonization.Actors
{
    public class SkinStateMachine : StateMachine<ASkinState>
    {
        protected ASkinState _prevState;

        public SkinStateMachine() : base(new EmptyState())
        {
            _prevState = _currentState;
        }

        public override bool SetState<T>(int id = 0)
        {
            TypeIdKey key = new(typeof(T), id);

            if (key == _currentState.Key)
                return true;

            if (_states.TryGetValue(key, out ASkinState newState))
            {
                _prevState = _currentState;
                _currentState.Exit();
                _currentState = newState;
                _currentState.Enter();
            }

            return false;
        }
        public override void SetState(ASkinState newState)
        {
            if (_currentState.Equals(newState))
                return;

            _prevState = _currentState;
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void ToPrev()
        {
            if (_currentState.Equals(_prevState))
                return;

            _currentState.Exit();
            _currentState = _prevState;
            _currentState.Enter();
        }

        public override void SetAndAddState(ASkinState newState)
        {
            if (_currentState.Equals(newState))
                return;

            _states.Add(newState.Key, newState);

            _prevState = _currentState;
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public override bool ForceSetState<T>(int id = 0)
        {
            if (_states.TryGetValue(new(typeof(T), id), out ASkinState newState))
            {
                _prevState = _currentState;
                _currentState.Exit();
                _currentState = newState;
                _currentState.Enter();
            }

            return false;
        }

        #region Nested: EmptyState
        private class EmptyState : ASkinState
        {
            public EmptyState() : base() { }
        }
        #endregion
    }
}
