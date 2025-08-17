using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.Characteristics
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

        public void CreateEffectsHitUI(ProjectColors colors, List<AEffectsUI> target, List<AEffectsUI> self)
		{
			int count = _effects.Length;
            HitEffectSettings effect;
            List<AEffectsUI> list;

            for (int i = 0; i < count; i++)
            {
                effect = _effects[i];
                list = target;
                if (effect.IsSelf)
                    list = self;

                list.Add(effect.CreateEffectUI(colors));
            }
        }
    }
}
