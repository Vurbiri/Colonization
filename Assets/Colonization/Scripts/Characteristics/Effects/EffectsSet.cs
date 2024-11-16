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

        private Action<ReactiveEffect, Operation> actionCollectionChange;
        private readonly AbilitiesSet<ActorAbilityId> _abilities;

        public ReactiveEffect this[int index] => _effects[index];

        public int Count => _count;

        public EffectsSet(AbilitiesSet<ActorAbilityId> abilities)
        {
            _effects = new ReactiveEffect[_capacity];
            _abilities = abilities;
        }

        #region IReactiveCollection
        public IUnsubscriber Subscribe(Action<ReactiveEffect, Operation> action)
        {
            actionCollectionChange -= action ?? throw new ArgumentNullException("action");
            actionCollectionChange += action;

            for (int i = 0; i < _count; i++)
                action(_effects[i], Operation.Subscribe);

            return new UnsubscriberCollection<ReactiveEffect>(this, action);
        }

        public void Unsubscribe(Action<ReactiveEffect, Operation> action) => actionCollectionChange -= action ?? throw new ArgumentNullException("action");
        #endregion

        public int Add(ReactiveEffect effect)
        {
            if (_count == _capacity)
                GrowArray();

            _effects[_count++] = effect;
            int delta = _abilities.AddPerk(effect);

            effect.Subscribe(RedirectEvents, _count - 1);

            return delta;
        }

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

        private void RedirectEvents(ReactiveEffect effect, Operation operation)
        {
            if (operation == Operation.Remove)
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
