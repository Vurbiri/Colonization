//Assets\Colonization\Scripts\Characteristics\Effects\EffectsSet.cs
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.Characteristics
{
    public class EffectsSet : ListReactiveItems<ReactiveEffect>
    {
        private readonly AbilitiesSet<ActorAbilityId> _abilities;

        public EffectsSet(AbilitiesSet<ActorAbilityId> abilities)
        {
            _values = new ReactiveEffect[_capacity];
            _abilities = abilities;
        }

        public bool Contains(EffectCode code)
        {
            for (int i = 0; i < _count; i++)
                if (_values[i].Code == code)
                    return true;

            return false;
        }

        public override void Add(ReactiveEffect effect)
        {
            if (_values == null)
                return;
            
            for (int i = 0; i < _count; i++)
                if (_values[i].UpdateDuration(effect))
                    return;

            base.Add(effect);

            _abilities.AddPerk(effect);
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
        
        public void Next()
        {
            for (int i = 0; i < _count; i++)
                _values[i].Next();
        }

        protected override void RemoveElement(ReactiveEffect effect)
        {
            base.RemoveElement(effect);
            _abilities.RemovePerk(effect);
        }
    }
}
