using System;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;

namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public class SpecSkillSettings : ASkillSettings
    {
        [SerializeField] private int _value;
        [SerializeField] private AnimationTime _timing;
        [SerializeField] private HitSFXName _hitSFXName;

        public int Value => _value;

        public ASkillUI Init(ProjectColors colors, SeparatorEffectUI separator, int actorType, int actorId)
        {
            ASkillUI ui;

            if (actorType == ActorTypeId.Warrior)
                ui = new BlockUI(colors, separator, _cost, _value);
            else
                ui = new SpecSkillUI(colors, separator);

#if !UNITY_EDITOR
            _effectsHitsSettings = null;
#endif
            return ui;
        }

#if UNITY_EDITOR

        [SerializeField] private int _actorId_Ed;
        [SerializeField] private string _nameClip_Ed;

        public readonly static System.Collections.Generic.HashSet<int> nonClip = new() { DemonId.Imp, DemonId.Grunt };

        public void OnValidate(int type, int id)
        {
            typeActor_ed = type;
            _actorId_Ed = id;

            if (type == ActorTypeId.Warrior)
            {
                _hitSFXName = new(EmptySFX.NAME);
                if (_effectsHitsSettings.Length > 0)
                    Array.Resize(ref _effectsHitsSettings, 0);
            }
            else if(nonClip.Contains(_actorId_Ed))
            {
                if (_effectsHitsSettings.Length != 1)
                    Array.Resize(ref _effectsHitsSettings, 1);
            }
            else
            {
                SetClip();
            }
        }

        public void UpdateAnimation_Ed(AnimatorOverrideController animator)
        {
            if(typeActor_ed == ActorTypeId.Warrior || nonClip.Contains(_actorId_Ed))
            {
                clipSettings_ed = null;
                _timing = new();
            }
            else
            {
                _nameClip_Ed = animator["A_Special"].name.Insert(1, "CS");
                SetClip();

                if (clipSettings_ed != null)
                    _timing = new(clipSettings_ed);
            }
        }

        private void SetClip()
        {
            if (clipSettings_ed == null || clipSettings_ed.name != _nameClip_Ed)
                clipSettings_ed = EUtility.FindAnyScriptable<AnimationClipSettingsScriptable>(_nameClip_Ed);
        }
#endif
    }
}
