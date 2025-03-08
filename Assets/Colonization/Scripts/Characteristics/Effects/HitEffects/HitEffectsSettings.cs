//Assets\Colonization\Scripts\Characteristics\Effects\HitEffects\HitEffectsSettings.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.Characteristics
{
    [Serializable]
	public class HitEffectsSettings
	{
        [SerializeField] private HitEffectSettings[] _effects;

        public int Count => _effects.Length;

        public HitEffects CreateEffectsHit(Actor parent, int skillId, int startEffectId)
        {
            return new HitEffects(_effects, parent.TypeId, parent.Id, skillId, startEffectId);
        }

        public void CreateEffectsHitUI(TextColorSettings hintTextColor, IList<AEffectsUI> target, IList<AEffectsUI> self)
		{
			int count = _effects.Length;
            HitEffectSettings effect;
            IList<AEffectsUI> list;

            for (int i = 0; i < count; i++)
            {
                effect = _effects[i];
                list = target;
                if (effect.IsSelf)
                    list = self;

                list.Add(effect.CreateEffectUI(hintTextColor));
            }
        }

		public static implicit operator HitEffectSettings[](HitEffectsSettings packetSettings) => packetSettings._effects;
    }
}
