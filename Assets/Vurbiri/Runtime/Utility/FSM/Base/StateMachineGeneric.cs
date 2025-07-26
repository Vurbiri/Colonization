using System;

namespace Vurbiri.FSM
{
    public class StateMachine<TState> : IDisposable where TState : IState
    {
        protected TState _currentState;
        protected TState _previousState;
        protected TState _defaultState;

        public TState CurrentState => _currentState;
        public TState PrevState => _previousState;

        public bool IsDefaultState => _currentState.Equals(_defaultState);
        public bool IsCurrentState(TState state) => _currentState.Equals(state);
        public bool IsCurrentOrDefaultState(TState state) => _currentState.Equals(_defaultState) | _currentState.Equals(state);

        public StateMachine(TState startState)
        {
            _defaultState = startState;
            _previousState = startState;
            _currentState = startState;
            _currentState.Enter();
        }

        public void SetState(TState newState)
        {
            if (!_currentState.Equals(newState))
            {
                _previousState = _currentState;
                _currentState.Exit();
                _currentState = newState;
                _currentState.Enter();
            }
        }

        public void ToPrevState()
        {
            if (!_currentState.Equals(_previousState))
            {
                _currentState.Exit();
                _currentState = _previousState;
                _currentState.Enter();
            }
        }

        public void ToDefaultState()
        {
            if (!_currentState.Equals(_defaultState))
            {
                _previousState = _currentState;
                _currentState.Exit();
                _currentState = _defaultState;
                _currentState.Enter();
            }
        }
         
        public void SetDefaultState(TState state) => _defaultState = state;

        public void Dispose()
        {
            _currentState.Exit();
        }
    }
}
