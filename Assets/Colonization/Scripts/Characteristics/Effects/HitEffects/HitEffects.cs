//Assets\Colonization\Scripts\Characteristics\Effects\HitEffects\HitEffects.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class HitEffects
	{
		protected readonly AHitEffect[] _effects;
        protected readonly int _count;

        public HitEffects(HitEffectSettings[] setting, int sourceId, int actorId, int skillId, int startEffectId)
        {
            _count = setting.Length;
            _effects = new AHitEffect[_count];
            for (int i = 0; i < _count; i++)
                _effects[i] = setting[i].CreateEffect(new(sourceId, actorId, skillId, startEffectId + i));
        }

        public void Apply(Actor self, Actor target)
        {
            for (int i = 0; i < _count; i++)
                _effects[i].Apply(self, target);
        }
    }
}
