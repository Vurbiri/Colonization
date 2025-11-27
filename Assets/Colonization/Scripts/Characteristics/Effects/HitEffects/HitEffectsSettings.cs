using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    [Serializable]
	public class HitEffectsSettings
	{
        [SerializeField] private HitEffectSettings[] _effects;

        public int Count => _effects.Length;

        public HitEffects CreateEffectsHit(int actorType, int actorId, int skillId, int effectId)
        {
            return new HitEffects(_effects, actorType, actorId, skillId, effectId);
        }

        public void CreateEffectsHitUI(ProjectColors colors, List<AEffectUI> target, List<AEffectUI> self)
		{
			int count = _effects.Length;
            HitEffectSettings effect;

            for (int i = 0; i < count; i++)
            {
                effect = _effects[i];
                (effect.IsSelf ? self : target).Add(effect.CreateEffectUI(colors));
            }
        }

#if UNITY_EDITOR
        public const string effectsField = nameof(_effects);

        public SkillHit_Ed SkillHit_Ed => _effects[0].SkillHit_Ed;
        public bool IsUsedAttack_Ed() => _effects != null && _effects.Length > 0 && _effects[0].UseAttack_Ed;
#endif
    }
}
