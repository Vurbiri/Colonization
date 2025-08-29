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
        [SerializeField] private SkillUI.Settings _ui;

        [NonSerialized] private ReadOnlyArray<HitEffects> _hitEffects;

        public TargetOfSkill Target { [Impl(256)] get => _target; }
        public float Range { [Impl(256)] get => _range; }
        public float Distance { [Impl(256)] get => _distance; }
        public int Cost { [Impl(256)] get => _cost; }
        public ReadOnlyArray<HitEffects> HitEffects { [Impl(256)] get => _hitEffects; }

        public SkillUI Init(ProjectColors colors, SeparatorEffectUI separator, int actorType, int actorId, int skillId)
        {
            int hitsCount = _effectsHitsSettings.Length;
            var effects = new HitEffects[hitsCount];
            List<AEffectUI> targetEffectsUI = new(hitsCount), selfEffectsUI = new(hitsCount);
            int targetEffectsCount, selfEffectsCount; 
            HitEffectsSettings effectsHitSettings;

            for (int i = 0, effectsCount = 0; i < hitsCount; i++)
            {
                effectsHitSettings = _effectsHitsSettings[i];
                effects[i] = effectsHitSettings.CreateEffectsHit(actorType, actorId, skillId, effectsCount);

                targetEffectsCount = targetEffectsUI.Count; selfEffectsCount = selfEffectsUI.Count;
                effectsHitSettings.CreateEffectsHitUI(colors, targetEffectsUI, selfEffectsUI);

                if (targetEffectsCount != targetEffectsUI.Count) targetEffectsUI.Add(separator);
                if (selfEffectsCount != selfEffectsUI.Count) selfEffectsUI.Add(separator);

                effectsCount += effectsHitSettings.Count;
            }

            _hitEffects = new(effects);
            SkillUI ui = new(colors, separator, _cost, _ui, targetEffectsUI.ToArray(), selfEffectsUI.ToArray());

#if !UNITY_EDITOR
            _ui = null; 
            _effectsHitsSettings = null;
#endif

            return ui;
        }

#if UNITY_EDITOR
        public AnimationClipSettingsScriptable clipSettings_ed;
        public HitSFXName hitSFXName_ed;
        public int typeActor_ed;
#endif
    }
}
