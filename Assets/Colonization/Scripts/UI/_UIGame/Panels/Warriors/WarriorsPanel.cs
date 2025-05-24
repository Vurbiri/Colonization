using System.Collections.Generic;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    sealed public class WarriorsPanel : ATogglePanel<CurrentMax, WarriorButton>
    {
        [SerializeField] private Transform _buttonRepository;
        [SerializeField] private IdArray<WarriorId, Sprite> _sprites = new();

        private Stack<WarriorButton> _buttonPool;

        public void Init(Human player, ProjectColors colors, InputController inputController, CanvasHint hint)
        {
            _inputController = inputController;

            var warriors = player.Warriors;
            var maxWarrior = player.GetAbility(HumanAbilityId.MaxWarrior);

            _buttonPool = new(maxWarrior.Value);

            maxWarrior.Subscribe(FillingPool);
            warriors.Subscribe(AddWarrior);

            InitToggle(warriors.CountReactive);
            _widget.Init(warriors.CountReactive, maxWarrior, colors, hint);
        }

        private void FillingPool(int max)
        {
            for (int i = _buttons.Count + _buttonPool.Count; i < max; i++)
            {
                _buttonPool.Push(Instantiate(_buttonPrefab, _buttonRepository, false).Init(_inputController, _buttonContainer, ToPool));
            }
        }

        private void ToPool(WarriorButton button)
        {
            _buttons.RemoveAt(button.Index);
            _buttonPool.Push(button);

            for (int i = 0; i < _buttons.Count; i++)
                _buttons[i].Index = i;
        }

        private void AddWarrior(Actor actor, TypeEvent typeEvent)
        {
            if (typeEvent == TypeEvent.Add | typeEvent == TypeEvent.Subscribe)
            {
                var button = _buttonPool.Pop();
                button.Setup(_buttons.Count, _sprites[actor.Id], actor, _toggle.IsOn);
                _buttons.Add(button);
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            EUtility.SetObject(ref _buttonRepository, "UIRepository");

            for (int i = 0; i < WarriorId.Count; i++)
                if (_sprites[i] == null)
                    _sprites[i] = EUtility.FindAnyAsset<Sprite>($"SP_Icon{WarriorId.GetName(i)}");
        }
#endif
    }
}
