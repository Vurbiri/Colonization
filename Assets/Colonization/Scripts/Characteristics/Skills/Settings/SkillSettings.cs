using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class SkillSettings : ASkillSettings
    {
        [SerializeField] protected SkillUI.Settings _ui;

        public SkillUI.Settings UI => _ui;

        public SkillUI Init(ProjectColors colors, SeparatorEffectUI separator, int actorType, int actorId, int skillId)
        {
            int hitsCount = _effectsHitsSettings.Length;
            _hitEffects = new (hitsCount);

            List<AEffectUI> targetEffectsUI = new(hitsCount), selfEffectsUI = new(hitsCount);
            int targetEffectsCount, selfEffectsCount; 
            HitEffectsSettings effectsHitSettings;

            for (int i = 0, effectsCount = 0; i < hitsCount; i++)
            {
                effectsHitSettings = _effectsHitsSettings[i];
                _hitEffects[i] = effectsHitSettings.CreateEffectsHit(actorType, actorId, skillId, effectsCount);

                targetEffectsCount = targetEffectsUI.Count; selfEffectsCount = selfEffectsUI.Count;
                effectsHitSettings.CreateEffectsHitUI(colors, targetEffectsUI, selfEffectsUI);

                if (targetEffectsCount != targetEffectsUI.Count) targetEffectsUI.Add(separator);
                if (selfEffectsCount != selfEffectsUI.Count) selfEffectsUI.Add(separator);

                effectsCount += effectsHitSettings.Count;
            }

            SkillUI ui = new(separator, _cost, _ui, targetEffectsUI.ToArray(), selfEffectsUI.ToArray());

#if !UNITY_EDITOR
            _ui = null; 
            _effectsHitsSettings = null;
#endif
            return ui;
        }

#if UNITY_EDITOR
        public HitSFXName hitSFXName_ed;

        public string GetName_Ed()
        {
            return Vurbiri.International.Localization.ForEditor(CONST_UI.FILE).GetText(CONST_UI.FILE, _ui.keySkillName).Delete("<b>", "</b>");
        }

        public bool IsHeal_Ed()
        {
            return _target == TargetOfSkill.Friend && _effectsHitsSettings != null && _effectsHitsSettings.Length > 0 && _effectsHitsSettings[0].IsUsedAttack_Ed();
        }
#endif
    }
}
