//Assets\Colonization\Scripts\Characteristics\Effects\EffectsSet.cs
using System;
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    sealed public class EffectsSet : IReactiveSet<ReactiveEffect>, IReadOnlyList<ReactiveEffect>
    {
        private readonly AbilitiesSet<ActorAbilityId> _abilities;
        private ReactiveEffect[] _values;
        private int _capacity = 4;
        private readonly RInt _count = new(0);
        private readonly Subscriber<ReactiveEffect, TypeEvent> _subscriber = new();
        
        public ReactiveEffect this[int index] => _values[index];

        public int Count => _count;
        public IReactiveValue<int> CountReactive => _count;

        public EffectsSet(AbilitiesSet<ActorAbilityId> abilities)
        {
            _values = new ReactiveEffect[_capacity];
            _abilities = abilities;
        }

        #region IReactiveCollection
        public Unsubscriber Subscribe(Action<ReactiveEffect, TypeEvent> action, bool calling = true)
        {
            if (calling)
            {
                for (int i = 0; i < _count; i++)
                    action(_values[i], TypeEvent.Subscribe);
            }

            return _subscriber.Add(action);
        }
        #endregion

        public int Add(ReactiveEffect effect)
        {
            if (effect == null)
                return 0;
            
            for (int i = 0; i < _count; i++)
                if (_values[i].Update(effect, _abilities.AddPerk, out int delta))
                    return delta;

            if (_count == _capacity)
                GrowArray();

            _values[_count] = effect;
            effect.Adding(RedirectEvents, _count);
            _count.Increment();

            return _abilities.AddPerk(effect);
        }
                
        public void Remove(EffectCode code)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_values[i].Code == code)
                {
                    _values[i].Removing();
                    return;
                }
            }
        }

        public bool Contains(EffectCode code)
        {
            for (int i = 0; i < _count; i++)
                if (_values[i].Code == code)
                    return true;

            return false;
        }

        public void Next()
        {
            for (int i = _count - 1; i >= 0; i--)
                _values[i].Next();
        }

        public void Dispose()
        {
            _subscriber.Dispose();
            for (int i = 0; i < _count; i++)
                _values[i].Dispose();
            _count.Dispose();

            _values = null;
        }

        public IEnumerator<ReactiveEffect> GetEnumerator()
        {
            for(int i = 0; i < _count; i++)
                yield return _values[i];
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void RemoveItem(ReactiveEffect effect)
        {
            _count.SilentValue--;

            ReactiveEffect temp;
            for (int i = effect.Index; i < _count; i++)
            {
                temp = _values[i + 1];
                temp.Index = i;
                _values[i] = temp;
            }

            _values[_count] = null;
            _abilities.RemovePerk(effect);

            _count.Signal();
        }

        private void RedirectEvents(ReactiveEffect effect, TypeEvent operation)
        {
            if (operation == TypeEvent.Remove)
                RemoveItem(effect);

            _subscriber.Invoke(effect, operation);
        }

        private void GrowArray()
        {
            _capacity = _capacity << 1 | 4;

            ReactiveEffect[] array = new ReactiveEffect[_capacity];
            for (int i = 0; i < _count; i++)
                array[i] = _values[i];
            _values = array;
        }
    }
}
