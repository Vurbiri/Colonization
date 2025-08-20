using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

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

        [NonSerialized] private ReadOnlyArray<HitEffects> _hitEffects;

        public TargetOfSkill Target { [Impl(256)] get => _target; }
        public float Range { [Impl(256)] get => _range; }
        public float Distance { [Impl(256)] get => _distance; }
        public int Cost { [Impl(256)] get => _cost; }
        public ReadOnlyArray<HitEffects> HitEffects { [Impl(256)] get => _hitEffects; }

        public void Init(int actorType, int actorId, int skillId)
        {
            int countHits = _effectsHitsSettings.Length;
            var effects = new HitEffects[countHits];
            HitEffectsSettings effectsHitSettings;

            for (int i = 0, u = 0; i < countHits; i++)
            {
                effectsHitSettings = _effectsHitsSettings[i];
                effects[i] = effectsHitSettings.CreateEffectsHit(actorType, actorId, skillId, u);
                u += effectsHitSettings.Count;
            }

            _hitEffects = new(effects);
            _ui = null; _effectsHitsSettings = null;
        }

        public SkillUI Init(ProjectColors colors, SeparatorEffectUI separator, int actorType, int actorId, int skillId)
        {
            int countHits = _effectsHitsSettings.Length;
            var effects = new HitEffects[countHits];
            List<AEffectsUI> targetEffectsUI = new(countHits), selfEffectsUI = new(countHits);
            int targetEffectsCount, selfEffectsCount; 
            HitEffectsSettings effectsHitSettings;

            for (int i = 0, u = 0; i < countHits; i++)
            {
                effectsHitSettings = _effectsHitsSettings[i];
                effects[i] = effectsHitSettings.CreateEffectsHit(actorType, actorId, skillId, u);

                targetEffectsCount = targetEffectsUI.Count; selfEffectsCount = selfEffectsUI.Count;
                effectsHitSettings.CreateEffectsHitUI(colors, targetEffectsUI, selfEffectsUI);

                if (targetEffectsCount != targetEffectsUI.Count) targetEffectsUI.Add(separator);
                if (selfEffectsCount != selfEffectsUI.Count) selfEffectsUI.Add(separator);

                u += effectsHitSettings.Count;
            }

            var ui = _ui.Init(colors, targetEffectsUI.ToArray(), selfEffectsUI.ToArray(), separator);
            _hitEffects = new(effects);

            _ui = null; _effectsHitsSettings = null;
            return ui;
        }

#if UNITY_EDITOR
        public AnimationClipSettingsScriptable clipSettings_ed;
        public HitSFXName hitSFXName_ed;
        public int typeActor_ed;
#endif
    }
}
