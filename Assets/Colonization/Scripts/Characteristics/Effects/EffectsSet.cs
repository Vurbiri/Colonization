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

        public int AddEffect(ReactiveEffect effect)
        {
            if (_values == null)
                return 0;
            
            for (int i = 0; i < _count; i++)
                if (_values[i].UpdateDuration(effect))
                    return 0;

            base.Add(effect);

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
        
        public void Next()
        {
            for (int i = _count - 1; i >= 0; i--)
                _values[i].Next();
        }

        protected override void RemoveItem(ReactiveEffect effect)
        {
            base.RemoveItem(effect);
            _abilities.RemovePerk(effect);
        }
    }
}
