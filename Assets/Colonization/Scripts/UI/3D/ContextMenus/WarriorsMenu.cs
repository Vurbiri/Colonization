using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    sealed public class WarriorsMenu : AWorldMenu
    {
        [Space]
        [SerializeField] private WorldHintButton _buttonClose;
        [Space]
        [SerializeField] private WorldHintButton _buttonMovement;
        [SerializeField] private ButtonBlock _buttonBlock;
        [SerializeField] private ButtonSkill[] _buttonsSkill;
        [SerializeField] private Positions[] _buttonPositions;

        private int _countButtonsSkill;
        private Actor _currentWarrior;

        public ISubscription<IMenu, bool> Init()
        {
            _buttonClose.Init(Close);

            _buttonMovement.Init(OnMovement);
            _buttonBlock.Init(OnBlock);

            _countButtonsSkill = _buttonsSkill.Length;
            for (int i = 0; i < _countButtonsSkill; i++)
                _buttonsSkill[i].Init(this);

            base.CloseInstant();

            return _eventActive;
        }

        public void Open(Actor actor)
        {
            _currentWarrior = actor;
            var skills = GameContainer.Actors.GetSkills(actor);

            _buttonMovement.Setup(true, _currentWarrior.CanMove);
            _buttonBlock.Setup(actor, skills.SpecSkillUI);

            int index = 0; var skillsUI = skills.SkillsUI;
            for (int count = skillsUI.Count; index < count; index++)
                _buttonsSkill[index].Setup(actor, index, skillsUI[index], _buttonPositions[count][index]);

            for (; index < _countButtonsSkill; index++)
                _buttonsSkill[index].Disable();

            base.Open();
        }

        private void OnMovement()
        {
            base.Close();
            _currentWarrior.Action.Move();
        }

        public void OnBlock()
        {
            base.Close();
            _currentWarrior.Action.UseSpecSkill();
        }

        #region Nested struct Positions
        //**********************************************************
        [System.Serializable]
        private struct Positions
        {
            public Vector3[] vectors;

            public readonly Vector3 this[int index]
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => vectors[index];
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                set => vectors[index] = value;
            }

            public Positions(Vector3[] values) => vectors = values;

            public static implicit operator Positions(Vector3[] values) => new(values);
        }
        #endregion

#if UNITY_EDITOR
        public override void SetButtonPosition(float buttonDistance)
        {
            _buttonClose.transform.localPosition = Vector3.zero;

            Vector3 distance = new(0f, buttonDistance, 0f);
            _buttonMovement.transform.localPosition = -distance;
            _buttonBlock.transform.localPosition = distance;

            CreatePositionButtons(distance);

            int count = _buttonsSkill.Length;
            for (int i = 0; i < count; i++)
                _buttonsSkill[i].transform.localPosition = _buttonPositions[count][i];
        }

        private void CreatePositionButtons(Vector3 distance)
        {
            float angle;
            int countButton = _buttonsSkill.Length, right, left;
            _buttonPositions = new Positions[countButton + 1];

            for (int i = 0, j; i <= countButton; i++)
            {
                _buttonPositions[i] = new Vector3[i];
                //left = i >> 1; right = i - left;
                right = i >> 1; left = i - right;

                angle = 180f / (left + 1);
                for (j = 0; j < left; j++)
                    _buttonPositions[i][j] = Quaternion.Euler(0f, 0f, angle * (j + 1)) * distance;

                angle = -180f / (right + 1);
                for (; j < i; j++)
                    _buttonPositions[i][j] = Quaternion.Euler(0f, 0f, angle * (right--)) * distance;
            }
        }

        private void OnValidate()
        {
            if (_buttonsSkill == null || _buttonsSkill.Length == 0)
                _buttonsSkill = GetComponentsInChildren<ButtonSkill>();

            this.SetChildren(ref _buttonBlock);
        }
#endif
    }
}
