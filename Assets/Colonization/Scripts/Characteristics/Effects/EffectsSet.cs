//Assets\Colonization\Scripts\Characteristics\Effects\EffectsSet.cs
using System;
using System.Collections;
using System.Collections.Generic;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    public class EffectsSet : IReactiveCollection<ReactiveEffect>
    {
        private ReactiveEffect[] _effects;
        private int _count = 0;
        private int _capacity = 4;

        private Action<ReactiveEffect, TypeEvent> actionCollectionChange;
        private readonly AbilitiesSet<ActorAbilityId> _abilities;

        public ReactiveEffect this[int index] => _effects[index];

        public int Count => _count;

        public EffectsSet(AbilitiesSet<ActorAbilityId> abilities)
        {
            _effects = new ReactiveEffect[_capacity];
            _abilities = abilities;
        }

        #region IReactiveCollection
        public IUnsubscriber Subscribe(Action<ReactiveEffect, TypeEvent> action, bool calling = true)
        {
            actionCollectionChange -= action ?? throw new ArgumentNullException("action");
            actionCollectionChange += action;

            if (calling)
            {
                for (int i = 0; i < _count; i++)
                    action(_effects[i], TypeEvent.Subscribe);
            }

            return new Unsubscriber<Action<ReactiveEffect, TypeEvent>>(this, action);
        }

        public void Unsubscribe(Action<ReactiveEffect, TypeEvent> action) => actionCollectionChange -= action ?? throw new ArgumentNullException("action");
        #endregion

        public void Add(ReactiveEffect effect)
        {
            if (_effects == null)
                return;
            
            for (int i = 0; i < _count; i++)
                if (_effects[i].UpdateDuration(effect))
                    return;

            if (_count == _capacity)
                GrowArray();

            _effects[_count++] = effect;
            _abilities.AddPerk(effect);

            effect.Subscribe(RedirectEvents, _count - 1);
        }

        public void Remove(EffectCode code)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_effects[i].Code == code)
                {
                    _effects[i].Removing();
                    return;
                }
            }
        }
        public void Remove(int index) => _effects[index].Removing();
        public void Remove(ReactiveEffect effect) => _effects[effect.Index].Removing();

        public void Next()
        {
            for (int i = 0; i < _count; i++)
                _effects[i].Next();
        }

        public IEnumerator<ReactiveEffect> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
                yield return _effects[i];
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void GrowArray()
        {
            _capacity = _capacity << 1 | 4;

            ReactiveEffect[] array = new ReactiveEffect[_capacity];
            for (int i = 0; i < _count; i++)
                array[i] = _effects[i];
            _effects = array;
        }

        private void RedirectEvents(ReactiveEffect effect, TypeEvent operation)
        {
            if (operation == TypeEvent.Remove)
            {
                _count--;
                ReactiveEffect temp;
                for (int i = effect.Index; i < _count; i++)
                {
                    temp = _effects[i + 1];
                    temp.Index = i;
                    _effects[i] = temp;
                }

                _abilities.RemovePerk(effect);
                _effects[_count] = null;
            }

            actionCollectionChange?.Invoke(effect, operation);
        }
    }
}
