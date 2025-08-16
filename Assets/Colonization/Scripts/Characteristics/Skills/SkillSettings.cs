using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public class SkillSettings
    {
        [SerializeField] private TargetOfSkill _target;
        [SerializeField] private float _range;
        [SerializeField] private float _distance;
        [SerializeField] private int _cost;
        [SerializeField] private HitEffectsSettings[] _effectsHitsSettings;
        [SerializeField] private SkillUI _ui;

        public TargetOfSkill Target => _target;
        public float Range => _range;
        public float Distance => _distance;
        public int Cost => _cost;

        public ReadOnlyArray<HitEffects> CreateEffectsHit(int actorType, int actorId, int skillId)
        {
            int countHits = _effectsHitsSettings.Length;
            HitEffects[] effects = new HitEffects[countHits];
            HitEffectsSettings effectsHitSettings;

            for (int i = 0, u = 0; i < countHits; i++)
            {
                effectsHitSettings = _effectsHitsSettings[i];
                effects[i] = effectsHitSettings.CreateEffectsHit(actorType, actorId, skillId, u);
                u += effectsHitSettings.Count;
            }

            return new(effects);
        }

        public SkillUI GetSkillUI(ProjectColors colors)
        {
            int countHits = _effectsHitsSettings.Length;
            List<AEffectsUI> targetEffectsUI = new(countHits), selfEffectsUI = new(countHits);

            for (int i = 0; i < countHits; i++)
                _effectsHitsSettings[i].CreateEffectsHitUI(colors, targetEffectsUI, selfEffectsUI);

            _ui.Init(colors, targetEffectsUI.ToArray(), selfEffectsUI.ToArray());

            return _ui;
        }

        public void RemoveSkillUI()
        {
            _ui = null;
        }

#if UNITY_EDITOR
        public AnimationClipSettingsScriptable clipSettings_ed;
        public HitSFXName[] hitSFXs;
        public int typeActor_ed;

        public bool UpdateName_Ed(string oldName, string newName)
        {
            bool output = false;
            for (int i = 0; i < hitSFXs.Length; i++)
                output |= hitSFXs[i].Update_Ed(oldName, newName);

            return output;
        }

#endif
    }
}
