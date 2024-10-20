using System;
using System.Collections.Generic;

namespace Vurbiri.FSM
{
    public class StateMachine : IDisposable
    {
        protected AState _currentState;
        protected Dictionary<Type, AState> _states = new();

        public AState CurrentState => _currentState;

        public StateMachine() : this(new EmptyState()) { }

        public StateMachine(AState startState)
        {
            _states.Add(startState.GetType(), startState);

            _currentState = startState;
            _currentState.Enter();
        }

        public void Init(AState startState)
        {
            _states.Add(startState.GetType(), startState);

            _currentState = startState;
            _currentState.Enter();
        }

        public void AddState(AState state) => _states.Add(state.GetType(), state);

        public bool TryAddState(AState state) => _states.TryAdd(state.GetType(), state);

        public bool SetState<T>()
        {
            Type type = typeof(T);

            if(type == _currentState.GetType())
                return true;

            if(_states.TryGetValue(type, out AState newState))
            { 
                _currentState.Exit();
                _currentState = newState;
                _currentState.Enter();
            }

            return false;
        }

        public bool ForceSetState<T>()
        {
            if (_states.TryGetValue(typeof(T), out AState newState))
            {
                _currentState.Exit();
                _currentState = newState;
                _currentState.Enter();
            }

            return false;
        }

        public void SetState(AState newState)
        {
            if (newState.GetType() == _currentState.GetType())
                return;

            _states.TryAdd(newState.GetType(), newState);

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
