//Assets\Colonization\Scripts\Characteristics\Effects\EffectsHit\EffectsHitSettings.cs
using System;
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

        public AEffectsUI[] CreateEffectsHitUI(SettingsTextColor hintTextColor)
		{
			int count = _effects.Length;
            AEffectsUI[] effectsUIs = new AEffectsUI[count];

            for (int i = 0; i < count; i++)
				effectsUIs[i] = _effects[i].CreateEffectUI(hintTextColor);
			
			return effectsUIs;
        }

		public static implicit operator EffectHitSettings[](EffectsHitSettings packetSettings) => packetSettings._effects;
    }
}
