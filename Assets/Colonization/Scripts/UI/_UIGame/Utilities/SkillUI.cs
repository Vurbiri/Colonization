using UnityEngine;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.UI
{
    [System.Serializable]
    public class SkillUI
    {
        public string keyName = "Attack";
        public Sprite sprite;
        public int cost;
        public int percentDamage;
        public EffectSettingsUI[] effects;

        public void SetEffects(EffectSettings[] effectsSettings)
        {
            int count = effectsSettings.Length;
            effects = new EffectSettingsUI[count];
            for (int i = 0; i < count; i++)
                effects[i] = new(effectsSettings[i]);
        }
    }
}
