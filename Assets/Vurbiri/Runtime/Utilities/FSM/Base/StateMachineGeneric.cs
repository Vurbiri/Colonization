//Assets\Vurbiri\Runtime\Utilities\FSM\Base\StateMachineGeneric.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.FSM
{
    public class StateMachine<TState> : IDisposable where TState : IState
    {
        protected TState _currentState;
        protected TState _previousState;
        protected TState _defaultState;
        
        protected Dictionary<TypeIdKey, TState> _states = new();

        public TState CurrentState => _currentState;
        public bool IsDefaultState => _currentState.Equals(_defaultState);

        public StateMachine(TState startState)
        {
            _states.Add(startState.Key, startState);

            _defaultState = startState;
            _previousState = startState;
            _currentState = startState;
            _currentState.Enter();
        }

        public bool IsCurrentState<T>(int id = 0) => _currentState.Key == new TypeIdKey(typeof(T), id);

        public void AddState(TState state) => _states.Add(state.Key, state);
        public void AddStates(IEnumerable<TState> states)
        {
            foreach (var state in states)
                _states.Add(state.Key, state);
        }

        public virtual bool SetState<T>(int id = 0) where T : TState
        {
            TypeIdKey key = new(typeof(T), id);

            if (key == _currentState.Key)
                return true;

            if (_states.TryGetValue(key, out TState newState))
            {
                _previousState = _currentState;
                _currentState.Exit();
                _currentState = newState;
                _currentState.Enter();
            }

            return false;
        }
        public virtual void SetState(TState newState)
        {
            if (_currentState.Equals(newState))
                return;

            _previousState = _currentState;
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void ToPrevState()
        {
            if (_currentState.Equals(_previousState))
                return;

            _currentState.Exit();
            _currentState = _previousState;
            _currentState.Enter();
        }

        public void ToDefaultState()
        {
            if (_currentState.Equals(_defaultState))
                return;

            _previousState = _currentState;
            _currentState.Exit();
            _currentState = _defaultState;
            _currentState.Enter();
        }

        public void SetDefaultState<T>(int id = 0) where T : TState
        {
            if (_states.TryGetValue(new(typeof(T), id), out TState state))
                _defaultState = state;
        }
        public void SetDefaultState(TState state) => _defaultState = state;

        public void Dispose()
        {
            _currentState.Exit();
            foreach (var state in _states.Values)
                state.Dispose();
        }
    }
}
