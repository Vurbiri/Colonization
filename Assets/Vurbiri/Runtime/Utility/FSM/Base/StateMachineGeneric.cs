using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.FSM
{
    public class StateMachine<TState> where TState : IState, System.IEquatable<TState>
    {
        protected TState _currentState;
        protected TState _previousState;
        protected TState _defaultState;

        private bool _block;

        public TState CurrentState => _currentState;
        public TState PrevState => _previousState;
        public bool IsDefaultState => _currentState.Equals(_defaultState);

        public StateMachine(TState startState)
        {
            _defaultState = startState;
            _previousState = startState;
            _currentState = startState;
            _currentState.Enter();
        }

        [Impl(256)] public bool IsSet(TState state) => _currentState.Equals(state);
        [Impl(256)] public bool IsSetOrDefault(TState state) => _currentState.Equals(state) | _currentState.Equals(_defaultState);

        [Impl(256)] public void Block() => _block = true;
        [Impl(256)] public void Unblock() => _block = false;

        [Impl(256)] public void AssignDefaultState(TState state) => _defaultState = state;

        public bool SetState(TState newState, bool block = false)
        {
            bool isSet = !_block;
            if (isSet)
                ForceSetState(newState, block);
            return isSet;
        }

        public void ForceSetState(TState newState, bool block = false)
        {
            _block = block;
            if (!_currentState.Equals(newState))
                SetStateAndSavePrev(newState);
        }

        public bool GetOutState(TState currentState)
        {
            bool output = _currentState.Equals(currentState);
            if (output)
            {
                _block = false;
                SetStateAndSavePrev(_defaultState);
            }
            return output;
        }
        public bool GetOutToPrevState(TState currentState)
        {
            bool output = _currentState.Equals(currentState);
            if (output)
            {
                _block = false;
                SetStateInternal(_previousState);
            }
            return output;
        }

        [Impl(256)] public void ToDefaultState()
        {
            if (!(_block | _currentState.Equals(_defaultState)))
                SetStateAndSavePrev(_defaultState);
        }
        [Impl(256)] public void ToPrevState()
        {
            if (!(_block | _currentState.Equals(_previousState)))
                SetStateInternal(_previousState);
        }

        [Impl(256)] private void SetStateAndSavePrev(TState state)
        {
            _previousState = _currentState;
            SetStateInternal(state);
        }
        [Impl(256)]private void SetStateInternal(TState state)
        {
            _currentState.Exit();
            _currentState = state;
            state.Enter();
        }
    }
}
