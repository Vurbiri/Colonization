//Assets\Colonization\Scripts\Characteristics\Effects\EffectsPacket\EffectsPacketSettings.cs
using System;
using UnityEngine;
using Vurbiri.Colonization.UI;
using Vurbiri.UI;

namespace Vurbiri.Colonization.Characteristics
{
    [Serializable]
	public class EffectsPacketSettings
	{
		[SerializeField] private EffectSettings[] _effects;

		public int Count => _effects.Length;

		public EffectsPacket CreateEffectsPacket(bool isTargetReact, int sourceId, int actorId, int skillId, int startEffectId)
		{
			return new(_effects, isTargetReact, sourceId, actorId, skillId, startEffectId);
		}

        public AEffectsUI[] CreateEffectsUI(HintTextColor hintTextColor)
		{
			int count = _effects.Length;
            AEffectsUI[] effectsUIs = new AEffectsUI[count];

            for (int i = 0; i < Count; i++)
				effectsUIs[i] = _effects[i].CreateEffectUI(hintTextColor);
			
			return effectsUIs;

        }
    }
}
