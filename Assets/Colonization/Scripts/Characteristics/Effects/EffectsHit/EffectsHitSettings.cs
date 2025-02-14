//Assets\Colonization\Scripts\Characteristics\Effects\EffectsHit\EffectsHitSettings.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.Characteristics
{
    [Serializable]
	public class EffectsHitSettings
	{
        [SerializeField] private EffectHitSettings[] _effects;

        public int Count => _effects.Length;

        public EffectsHit CreateEffectsHit(Actor parent, int skillId, int startEffectId)
        {
            return new EffectsHit(_effects, parent.TypeId, parent.Id, skillId, startEffectId);
        }

        public void CreateEffectsHitUI(SettingsTextColor hintTextColor, IList<AEffectsUI> target, IList<AEffectsUI> self)
		{
			int count = _effects.Length;
            EffectHitSettings effect;
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

		public static implicit operator EffectHitSettings[](EffectsHitSettings packetSettings) => packetSettings._effects;
    }
}
