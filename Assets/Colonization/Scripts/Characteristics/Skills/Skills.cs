//Assets\Colonization\Scripts\Characteristics\Skills\Skills.cs
using System;
using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.UI;
using Vurbiri.Localization;
using Vurbiri.UI;
using static Vurbiri.Colonization.Actors.Actor;

namespace Vurbiri.Colonization.Characteristics
{
    [System.Serializable]
    public partial class Skills : IDisposable
    {
        public const int COUNT_SKILLS_MAX = 4;

        [SerializeField] private float _speedWalk = 0.45f;
        [SerializeField] private float _speedRun = 0.65f;
        [SerializeField] private int _blockCost = 2;
        [SerializeField] private int _blockValue = 100;
        [SerializeField] private SkillSettings[] _skillsSettings;
        
        [NonSerialized] private SkillUI[] _skillsUI;
        [NonSerialized] private EffectsHint[][] _effects;
        [NonSerialized] private BlockUI _blockUI;

        public IReadOnlyList<SkillUI> SkillsUI => _skillsUI;
        public BlockUI BlockUI => _blockUI ??= new(_blockCost, _blockValue);

        public MoveState GetMoveSate(Actor parent) => new(_speedWalk, parent);
        public BlockState GetBlockState(Actor parent) => new(_blockCost, _blockValue, parent);

        public List<ASkillState> GetSkillSates(Actor parent)
        {
            if(_effects == null | _skillsUI == null)
                return GetAndCreateSkills(parent);

            int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);
            List<ASkillState> skillStates = new(countSkills);

            for (int i = 0; i < countSkills; i++)
                skillStates.Add(CreateState(parent, _skillsSettings[i], i));

            return skillStates;
        }

        public void Dispose()
        {
            _blockUI?.Dispose();

            if (_skillsUI == null)
                return;

            foreach (var skillUI in _skillsUI)
                skillUI.Dispose();
        }

        private List<ASkillState> GetAndCreateSkills(Actor parent)
        {
            var hintTextColor = SceneData.Get<HintTextColor>();
            var language = SceneServices.Get<Language>();

            int countSkills = Math.Min(_skillsSettings.Length, COUNT_SKILLS_MAX);
            List<ASkillState> skillStates = new(countSkills);

            _effects = new EffectsHint[countSkills][];
            _skillsUI = new SkillUI[countSkills];

            SkillSettings skill; EffectsHintSettings packSettings;
            EffectsHint[] effectsPackets; 
            List<AEffectsUI> effectsSkillUI;
            for (int i = 0; i < countSkills; i++)
            {
                skill = _skillsSettings[i];

                int countPackets = skill.effectsPacket.Length;
                effectsPackets = new EffectsHint[countPackets];
                effectsSkillUI = new(countPackets << 1);

                for (int j = 0, u = 0; j < countPackets; j++)
                {
                    packSettings = skill.effectsPacket[j];
                    effectsPackets[j] = packSettings.CreatePacket(parent, i, u);
                    effectsSkillUI.AddRange(packSettings.CreateEffectsUI(hintTextColor));
                    u += packSettings.Count;
                }
                skill.ui.Init(language, hintTextColor, effectsSkillUI.ToArray());

                _skillsUI[i] = skill.ui;
                _effects[i] = effectsPackets;

                skillStates.Add(CreateState(parent, skill, i));

#if !UNITY_EDITOR
                skill.ui = null;
                skill.effectsPacket = null;
#endif
            }

            return skillStates;
        }

        private ASkillState CreateState(Actor parent, SkillSettings skill, int id)
        {
            if (skill.target == TargetOfSkill.Self)
                return new SelfBuffState(parent, _effects[id], skill.cost, id);

            if (skill.isMove)
                return new AttackState(parent, skill.target, _effects[id], skill.range, _speedRun, skill.cost, id);

            return new SpellState(parent, skill.target, _effects[id], skill.isTargetReact, skill.cost, id);
        }

#if UNITY_EDITOR
        public IReadOnlyList<SkillSettings> List => _skillsSettings;
#endif
    }
}
