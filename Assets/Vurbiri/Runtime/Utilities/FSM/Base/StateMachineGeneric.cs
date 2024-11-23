//Assets\Vurbiri\Runtime\Utilities\FSM\Base\StateMachineGeneric.cs
using System;
using System.Collections.Generic;

namespace Vurbiri.FSM
{
    public class StateMachine<TState> : IDisposable where TState : IState
    {
        protected TState _currentState;
        protected TState _defaultState;
        protected Dictionary<TypeIdKey, TState> _states = new();

        public TState CurrentState => _currentState;
        public bool IsDefaultState => _currentState.Equals(_defaultState);

        public StateMachine(TState startState)
        {
            _states.Add(startState.Key, startState);

            _defaultState = startState;
            _currentState = startState;
            _currentState.Enter();
        }

        public void SetDefaultState<T>(int id = 0) where T : TState
        {
            if (_states.TryGetValue(new(typeof(T), id), out TState state))
                _defaultState = state;
        }
        public void SetDefaultState(TState state)
        {
            if (state != null)
                _defaultState = state;
        }

        public void AddState(TState state) => _states.Add(state.Key, state);
        public bool TryAddState(TState state) => _states.TryAdd(state.Key, state);

        public virtual bool SetState<T>(int id = 0) where T : TState
        {
            TypeIdKey key = new(typeof(T), id);

            if (key == _currentState.Key)
                return true;

            if (_states.TryGetValue(key, out TState newState))
            {
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

            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public virtual void SetAndAddState(TState newState)
        {
            if (_currentState.Equals(newState))
                return;

            _states.Add(newState.Key, newState);

            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public virtual bool ForceSetState<T>(int id = 0) where T : TState
        {
            if (_states.TryGetValue(new(typeof(T), id), out TState newState))
            {
                _currentState.Exit();
                _currentState = newState;
                _currentState.Enter();
            }

            return false;
        }

        public void ToDefault()
        {
            if (_currentState.Equals(_defaultState))
                return;

            _currentState.Exit();
            _currentState = _defaultState;
            _currentState.Enter();
        }

        public void Update() => _currentState.Update();

        public void Dispose()
        {
            _currentState.Exit();
            foreach (var state in _states.Values)
                state.Dispose();
        }
    }
}
