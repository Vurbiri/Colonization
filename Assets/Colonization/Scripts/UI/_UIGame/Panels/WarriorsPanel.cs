//Assets\Colonization\Scripts\UI\_UIGame\Panels\WarriorsPanel.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Colonization.UI;
using Vurbiri.Reactive.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization
{
    public class WarriorsPanel : MonoBehaviour
    {
        [SerializeField] private VToggle _toggle;
        [SerializeField] private CurrentMax _widget;
        [Space]
        [SerializeField] private WarriorButton _warriorButtonPrefab;
        [SerializeField] private Transform _buttonContainer, _buttonRepository;
        [SerializeField] private IdArray<WarriorId, ButtonView> _buttonViews = new();

        private readonly List<WarriorButton> _buttons = new();
        private Stack<WarriorButton> _buttonPool;
        private InputController _inputController;
        private Coroutine _coroutine;

        public void Init(Human player, ProjectColors colors, InputController inputController)
        {
            _inputController = inputController;

            var warriors = player.Warriors;
            var maxWarrior = player.GetAbility(HumanAbilityId.MaxWarrior);

            _buttonPool = new(maxWarrior.Value);

            maxWarrior.Subscribe(FillingPool);
            warriors.Subscribe(AddWarrior);

            warriors.CountReactive.Subscribe(count => _toggle.interactable = count > 0);
            _toggle.AddListener(OnToggle);

            _widget.Init(warriors.CountReactive, maxWarrior, colors);
        }

        private void FillingPool(int max)
        {
            for (int i = _buttons.Count + _buttonPool.Count; i < max; i++)
            {
                _buttonPool.Push(Instantiate(_warriorButtonPrefab, _buttonRepository, false).Init(_inputController, _buttonContainer, ToPool));
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
                button.Setup(_buttons.Count, _buttonViews[actor.Id], actor, _toggle.IsOn);
                _buttons.Add(button);
            }
        }

        private void OnToggle(bool isOn)
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            if (isOn)
                _coroutine = StartCoroutine(Enable_Cn());
            else
                _coroutine = StartCoroutine(Disable_Cn());


            #region Local: Enable_Cn(), Disable_Cn()
            //=================================
            IEnumerator Enable_Cn()
            {
                for (int i = 0; i < _buttons.Count; i++)
                    yield return _buttons[i].Enable();

                _coroutine = null;
            }
            //=================================
            IEnumerator Disable_Cn()
            {
                for (int i = _buttons.Count - 1; i >= 0; i--)
                    yield return _buttons[i].Disable();

                _coroutine = null;
            }
            #endregion
        }


#if UNITY_EDITOR

        public RectTransform UpdateVisuals_Editor(float pixelsPerUnit, Vector2 padding, ProjectColors colors)
        {
            Image image = GetComponent<Image>();
            image.color = colors.BackgroundPanel;
            image.pixelsPerUnitMultiplier = pixelsPerUnit;

            _toggle.CheckmarkOn.color = colors.BackgroundPanel;
            _toggle.CheckmarkOff.color = colors.BackgroundPanel;

            Vector2 size = _widget.Size + padding * 2f;

            RectTransform thisRectTransform = (RectTransform)transform;

            thisRectTransform.sizeDelta = size;
            _warriorButtonPrefab.RectTransform.sizeDelta = new(size.x, size.x);
            ((RectTransform)_buttonContainer).anchoredPosition = new(0f, (size.x + size.y) * 0.5f + 20f);

            return thisRectTransform;
        }

        private void OnValidate()
        {
            if (_toggle == null)
                _toggle = GetComponent<VToggle>();
            if (_widget == null)
                _widget = GetComponentInChildren<CurrentMax>();

            if (_buttonContainer == null)
                _buttonContainer = EUtility.GetComponentInChildren<Transform>(this, "ButtonContainer");
            EUtility.SetObject<Transform>(ref _buttonRepository, "CameraUIRepository");
            EUtility.SetPrefab(ref _warriorButtonPrefab);

            for (int i = 0; i < WarriorId.Count; i++)
            {
                if (_buttonViews[i].sprite == null)
                    _buttonViews[i].sprite = EUtility.FindAnyAsset<Sprite>($"SP_Icon{WarriorId.GetName(i)}");

                if (string.IsNullOrEmpty(_buttonViews[i].keyName))
                    _buttonViews[i].keyName = WarriorId.GetName(i);
            }
        }
#endif
    }
}
