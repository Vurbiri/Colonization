//Assets\Colonization\Scripts\Characteristics\Skills\SkillSettings.cs
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;
using Vurbiri.TextLocalization;
using Vurbiri.UI;

namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public class SkillSettings
    {
        [SerializeField] private TargetOfSkill _target;
        [SerializeField] private float _range;
        [SerializeField] private float _distance;
        [SerializeField] private int _cost;
        [SerializeField] private EffectsHitSettings[] _effectsHitsSettings;
        [SerializeField] private SkillUI _ui;

#if UNITY_EDITOR
        public AnimationClipSettingsScriptable clipSettings;
        public AHitScriptableSFX[] SFXHits;
#endif

        public TargetOfSkill Target => _target;
        public float Range => _range;
        public float Distance => _distance;
        public int Cost => _cost;

        public EffectsHit[] CreateEffectsHit(Actor parent, int skillId)
        {
            int countHits = _effectsHitsSettings.Length;
            EffectsHit[] effects = new EffectsHit[countHits];
            EffectsHitSettings effectsHitSettings;

            for (int i = 0, u = 0; i < countHits; i++)
            {
                effectsHitSettings = _effectsHitsSettings[i];
                effects[i] = effectsHitSettings.CreateEffectsHit(parent, skillId, u);
                u += effectsHitSettings.Count;
            }

            return effects;
        }


        public SkillUI GetSkillUI(Localization language, TextColorSettings hintTextColor)
        {
            int countHits = _effectsHitsSettings.Length;
            List<AEffectsUI> targetEffectsUI = new(countHits), selfEffectsUI = new(countHits);

            for (int i = 0; i < countHits; i++)
                _effectsHitsSettings[i].CreateEffectsHitUI(hintTextColor, targetEffectsUI, selfEffectsUI);

            _ui.Init(language, hintTextColor, targetEffectsUI.ToArray(), selfEffectsUI.ToArray());

            return _ui;
        }
    }
}
