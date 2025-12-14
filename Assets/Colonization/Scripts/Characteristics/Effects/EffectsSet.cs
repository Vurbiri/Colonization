using System;
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Collections;
using Vurbiri.Reactive.Collections;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    public abstract class ReactiveEffects : IEnumerable<ReactiveEffect>
    {
        protected readonly AbilitiesSet<ActorAbilityId> _abilities;
        protected ReactiveEffect[] _values;
        protected int _count, _capacity = 3;
        protected readonly ReactiveVersion<ReactiveEffect, TypeEvent> _version = new();
        
        public int Count { [Impl(256)] get => _count; }

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

            return _version.Add(action);
        }

        public bool Contains<T>(T code) where T : IEquatable<EffectCode>
        {
            int i = _count;
            while (i --> 0 && !code.Equals(_values[i].Code));
            return i >= 0;
        }

        public bool ContainsNegative()
        {
            bool result = false;
            for (int i = 0; !result & i < _count; ++i)
                result = _values[i].IsNegative;
            return result;
        }

        [Impl(256)] public ArrayEnumerator<ReactiveEffect> GetEnumerator() => new(_values, _count, _version);
        IEnumerator<ReactiveEffect> IEnumerable<ReactiveEffect>.GetEnumerator() => _count == 0 ? EmptyEnumerator<ReactiveEffect>.Instance : GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<ReactiveEffect>)this).GetEnumerator();
    }

    //**************************************************************************************************

    sealed public class EffectsSet : ReactiveEffects, IReadOnlyList<ReactiveEffect>, IDisposable
    {
        public ReactiveEffect this[int index] { [Impl(256)] get => _values[index]; }

        [Impl(256)] public EffectsSet(AbilitiesSet<ActorAbilityId> abilities) : base(abilities) { }

        public int Add(ReactiveEffect effect)
        {
            if (_values != null && effect != null)
            {
                for (int i = 0; i < _count; ++i)
                    if (_values[i].TryUpdate(effect, _abilities.AddPerk, out int delta))
                        return delta;

                if (_count == _capacity)
                    Array.Resize(ref _values, _capacity = _capacity << 1 | 3);

                _values[_count] = effect;
                effect.Adding(RedirectEvents, _count++);

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
            _version.Clear();
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

            _version.Next(effect, operation);
        }
    }
}
