using System;
using System.Collections.Generic;

namespace Vurbiri.FSM
{
    public class StateMachine : IDisposable
    {
        protected AState _currentState;
        protected Dictionary<TypeIdKey, AState> _states = new();

        public AState CurrentState => _currentState;

        public StateMachine() : this(new EmptyState()) { }

        public StateMachine(AState startState)
        {
            _states.Add(startState.Key, startState);

            _currentState = startState;
            _currentState.Enter();
        }

        public void Init(AState startState)
        {
            _states.Add(startState.Key, startState);

            _currentState = startState;
            _currentState.Enter();
        }

        public void AddState(AState state) => _states.Add(state.Key, state);

        public bool TryAddState(AState state) => _states.TryAdd(state.Key, state);

        public bool SetState<T>(int id = 0)
        {
            TypeIdKey key = new(typeof(T), id);

            if(key == _currentState.Key)
                return true;

            if(_states.TryGetValue(key, out AState newState))
            { 
                _currentState.Exit();
                _currentState = newState;
                _currentState.Enter();
            }

            return false;
        }

        public bool ForceSetState<T>(int id = 0)
        {
            if (_states.TryGetValue(new(typeof(T), id), out AState newState))
            {
                _currentState.Exit();
                _currentState = newState;
                _currentState.Enter();
            }

            return false;
        }

        public void SetState(AState newState)
        {
            if (newState == _currentState)
                return;

            _states.TryAdd(newState.Key, newState);

            _currentState.Exit();
            _currentState = newState;
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
