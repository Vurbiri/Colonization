using System;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public partial class Skills : IDisposable
    {
        [SerializeField] private float _runSpeed; // = 0.6f;
        [SerializeField] private float _walkSpeed; // = 0.6f;
        [SerializeField] private float _speed; // = 0.6f;
        [SerializeField] private SpecSkillSettings _specSkillSettings;
        [SerializeField] private SkillSettings[] _skillsSettings;

        [SerializeField] private Array<string> _hitSfxNames;
        [SerializeField] private Array<AnimationTime> _timings;

        [NonSerialized] private Array<SkillUI> _skillsUI;
        private MoveUI _moveUI;

        public ReadOnlyArray<SkillUI> SkillsUI { [Impl(256)] get => _skillsUI; }
        public MoveUI MoveUI { [Impl(256)] get => _moveUI; }

        public ReadOnlyArray<string> HitSfxNames { [Impl(256)] get => _hitSfxNames; }
        public ReadOnlyArray<AnimationTime> Timings { [Impl(256)] get => _timings; }

        public SpecSkillSettings Spec { [Impl(256)] get => _specSkillSettings; }

        public void Init(int actorType, int actorId)
        {
            int countSkills = _skillsSettings.Length;
            var colors = GameContainer.UI.Colors;
            var separator = new SeparatorEffectUI(colors);

            _skillsUI = new(countSkills);
            for (int i = 0; i < countSkills; i++)
                _skillsUI[i] = _skillsSettings[i].Init(colors, separator, actorType, actorId, i);
            _moveUI = new(separator);

            _specSkillSettings.Init(separator, actorType, actorId);
        }

        public void CreateStates<TActor, TSkin>(Actor.AStates<TActor, TSkin> states) where TActor : Actor where TSkin : ActorSkin
        {
            states.AddMoveSkillState(_walkSpeed);
            states.AddSpecSkillState(_specSkillSettings, _runSpeed, _walkSpeed);

            int countSkills = _skillsSettings.Length;
            states.MainSkillsCount = countSkills;
            for (int i = 0; i < countSkills; i++)
                states.AddSkillState(_skillsSettings[i], _runSpeed, i);
            for (int i = countSkills; i < CONST.MAIN_SKILLS_COUNT; i++)
                states.AddEmptySkillState(i);
        }

        public void Dispose()
        {
            if (_skillsUI != null)
            {
                for (int i = 0; i < _skillsUI.Count; i++)
                    _skillsUI[i].Dispose();

                _moveUI.Dispose();
                _specSkillSettings.Dispose();
            }
        }
    }
}
