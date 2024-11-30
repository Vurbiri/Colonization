//Assets\Colonization\Scripts\Characteristics\Effects\EffectsPacket\EffectsHintSettings.cs
using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.Characteristics
{
    [Serializable]
	public class EffectsHintSettings
	{
		[SerializeField] private EffectSettings[] _effects;

        public int Count => _effects.Length;

        public EffectsHint CreatePacket(Actor parent, int skillId, int startEffectId)
        {
            return new EffectsHint(_effects, parent.TypeId, parent.Id, skillId, startEffectId);
        }

        public AEffectsUI[] CreateEffectsUI(HintTextColor hintTextColor)
		{
			int count = _effects.Length;
            AEffectsUI[] effectsUIs = new AEffectsUI[count];

            for (int i = 0; i < count; i++)
				effectsUIs[i] = _effects[i].CreateEffectUI(hintTextColor);
			
			return effectsUIs;
        }

		public static implicit operator EffectSettings[](EffectsHintSettings packetSettings) => packetSettings._effects;
    }
}
