//Assets\Colonization\Scripts\Characteristics\Effects\EffectsPacket\EffectsPacket.cs
using Vurbiri.Colonization.Actors;

namespace Vurbiri.Colonization.Characteristics
{
    public class EffectsPacket
	{
		private readonly AEffect[] _effects;
        private readonly bool _isTargetReact = true;
        private readonly int _count;

        public EffectsPacket(EffectSettings[] setting, bool isTargetReact, int sourceId, int actorId, int skillId, int startEffectId)
        {
            _isTargetReact = isTargetReact;
            _count = setting.Length;
            _effects = new AEffect[_count];
            for (int i = 0; i < _count; i++)
                _effects[i] = setting[i].CreateEffect(new(sourceId, actorId, skillId, startEffectId + i));
        }

        public bool Apply(Actor self, Actor target)
        {
            for (int i = 0; i < _count; i++)
                _effects[i].Apply(self, target);
            return target.ReactionToAttack(_isTargetReact); ;
        }
    }
}
