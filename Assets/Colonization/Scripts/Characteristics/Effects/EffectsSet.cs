using System;
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization
{
    public abstract class ReactiveEffects : IEnumerable<ReactiveEffect>
    {
        protected readonly AbilitiesSet<ActorAbilityId> _abilities;
        protected ReactiveEffect[] _values;
        protected int _count, _capacity = 3;
        protected readonly VAction<ReactiveEffect, TypeEvent> _eventChanged = new();
        
        public int Count => _count;

        protected ReactiveEffects(AbilitiesSet<ActorAbilityId> abilities)
        {
            _values = new ReactiveEffect[_capacity];
            _abilities = abilities;
        }

        public Subscription Subscribe(Action<ReactiveEffect, TypeEvent> action, bool instantGetValue = true)
        {
            if (instantGetValue)
            {
                for (int i = 0; i < _count; i++)
                    action(_values[i], TypeEvent.Subscribe);
            }

            return _eventChanged.Add(action);
        }

        public bool Contains<T>(T code) where T : IEquatable<EffectCode>
        {
            int i = _count;
            while (i --> 0 && !code.Equals(_values[i].Code)) ;
            return i >= 0;
        }

        public IEnumerator<ReactiveEffect> GetEnumerator() => new ArrayEnumerator<ReactiveEffect>(_values, _count);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    //**************************************************************************************************

    sealed public class EffectsSet : ReactiveEffects, IReadOnlyList<ReactiveEffect>, IDisposable
    {
       
        public ReactiveEffect this[int index] => _values[index];

        public EffectsSet(AbilitiesSet<ActorAbilityId> abilities) : base(abilities) { }

        public int Add(ReactiveEffect effect)
        {
            if (_values != null & effect != null)
            {
                for (int i = 0; i < _count; i++)
                    if (_values[i].TryUpdate(effect, _abilities.AddPerk, out int delta))
                        return delta;

                if (_count == _capacity)
                    GrowArray();

                _values[_count] = effect;
                effect.Adding(RedirectEvents, _count);
                _count++;

                return _abilities.AddPerk(effect);
            }

            return 0;
        }
                
        public void Remove(EffectCode code)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_values[i] == code)
                {
                    _values[i].Removing();
                    return;
                }
            }
        }

        public void Next()
        {
            for (int i = _count - 1; i >= 0; i--)
                _values[i].Next();
        }

        public void Degrade(int duration, Id<ClearEffectsId> type)
        {
            if(type == ClearEffectsId.All)
            {
                for (int i = _count - 1; i >= 0; i--)
                    _values[i].Degrade(duration);
            }
            else
            {
                bool isPositive = type == ClearEffectsId.Positive;
                for (int i = _count - 1; i >= 0; i--)
                    _values[i].Degrade(duration, isPositive);
            }
        }

        public void Dispose()
        {
            _eventChanged.Clear();
            for (int i = 0; i < _count; i++)
                _values[i].Dispose();
            
            _values = null;
        }

        private void RemoveItem(ReactiveEffect effect)
        {
            ReactiveEffect temp;

            _count--;
            for (int i = effect.Index; i < _count; i++)
            {
                temp = _values[i + 1];
                temp.Index = i;
                _values[i] = temp;
            }

            _values[_count] = null;
            _abilities.RemovePerk(effect);
        }

        private void RedirectEvents(ReactiveEffect effect, TypeEvent operation)
        {
            if (operation == TypeEvent.Remove)
                RemoveItem(effect);

            _eventChanged.Invoke(effect, operation);
        }

        private void GrowArray()
        {
            _capacity = _capacity << 1 | 3;

            ReactiveEffect[] array = new ReactiveEffect[_capacity];
            for (int i = 0; i < _count; i++)
                array[i] = _values[i];
            _values = array;
        }
    }
}
