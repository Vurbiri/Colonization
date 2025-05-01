//Assets\Colonization\Scripts\UI\_UIGame\Panels\Button\WarriorButton.cs
using System.Collections;
using UnityEngine;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class WarriorButton : AVButton
    {
        [SerializeField] private float _speedOpen = 8f;
        [SerializeField] private float _speedClose = 10f;
        [SerializeField] private CanvasGroup _canvasGroup;
        [Space]
		[SerializeField] private VBarInt _hpBar;

        private Actor _attachActor;
        private InputController _inputController;
        private Coroutine _coroutine;
        private float _targetAlpha;
        private Unsubscribers _unsubscribers = new();

        public void Init(InputController inputController)
        {
            _inputController = inputController;
            _onClick.Add(SelectActor);
        }

        public void Setup(Actor actor)
        {
            _attachActor = actor;
            _unsubscribers.Unsubscribe();

            actor.InteractableReactive.Subscribe(SetInteractable);

            var abilities = actor.Abilities;
            _unsubscribers += abilities[ActorAbilityId.MaxHP].Subscribe(SetMaxHP);
            _unsubscribers += abilities[ActorAbilityId.CurrentHP].Subscribe(SetCurrentHP);
        }

        #region Open/Close
        public void Open()
        {
            SetOpenValue();
            _coroutine = StartCoroutine(Open_Cn());

            #region Local: Open_Cn()
            //=================================
            IEnumerator Open_Cn()
            {
                float alpha = _canvasGroup.alpha;
                while (alpha < 1f)
                {
                    _canvasGroup.alpha = alpha += Time.unscaledDeltaTime * _speedOpen;
                    yield return null;
                }

                ApplyCanvasGroupValues();
            }
            #endregion
        }
        public void Close()
        {
            SetCloseValue();
            _coroutine = StartCoroutine(Close_Cn());

            #region Local: Close_Cn()
            //=================================
            IEnumerator Close_Cn()
            {
                float alpha = _canvasGroup.alpha;
                while (alpha > 0f)
                {
                    _canvasGroup.alpha = alpha -= Time.unscaledDeltaTime * _speedClose;
                    yield return null;
                }

                ApplyCanvasGroupValues();
            }
            #endregion
        }

        public void OpenInstant()
        {
            SetOpenValue();
            ApplyCanvasGroupValues();
        }

        public void CloseInstant()
        {
            SetCloseValue();
            ApplyCanvasGroupValues();
        }

        private void SetOpenValue()
        {
            _targetAlpha = 1f;
            if (_coroutine != null)
                StopCoroutine(_coroutine);
        }
        private void SetCloseValue()
        {
            _targetAlpha = 0f;
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _canvasGroup.blocksRaycasts = false;
        }
        private void ApplyCanvasGroupValues()
        {
            _canvasGroup.alpha = _targetAlpha;
            _canvasGroup.blocksRaycasts = _targetAlpha > 0.1f;
            _coroutine = null;
        }
        #endregion

        #region On...
        private void SelectActor() => _inputController.Select(_attachActor);
        private void SetInteractable(bool value) => interactable = value;
        private void SetMaxHP(int maxHP) => _hpBar.MaxValue = maxHP;
        private void SetCurrentHP(int currentHP) => _hpBar.Value = currentHP;
        #endregion

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                ApplyCanvasGroupValues();
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
            if (_hpBar == null)
                _hpBar = GetComponentInChildren<VBarInt>();
        }
#endif
    }
}
