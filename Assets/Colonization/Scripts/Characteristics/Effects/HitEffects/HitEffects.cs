namespace Vurbiri.Colonization
{
    public class HitEffects
	{
		private readonly AHitEffect[] _effects;
        private readonly int _count;

        public AHitEffect this[int index] => _effects[index];
        public int Count => _count;

        public HitEffects(HitEffectSettings[] setting, int actorType, int actorId, int skillId, int startEffectId)
        {
            _count = setting.Length;
            _effects = new AHitEffect[_count];
            for (int i = 0; i < _count; i++)
                _effects[i] = setting[i].CreateEffect(new(actorType, actorId, skillId, startEffectId + i));
        }

        public void Apply(Actor self, Actor target)
        {
            for (int i = 0; i < _count; i++)
                _effects[i].Apply(self, target);
        }
    }
}
