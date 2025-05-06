//Assets\Colonization\Scripts\UI\_UIGame\Panels\Button\WarriorButton.cs
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class WarriorButton : AVButton
    {
        private const char AP_CHAR = '+';

        [SerializeField] private float _speedOpen = 6f;
        [SerializeField] private float _speedClose = 8f;
        [SerializeField] private float _speedMove = 4f;
        [SerializeField] private CanvasGroup _canvasGroup;
        [Space]
        [SerializeField] private Image _icon;
        [SerializeField] private VBarInt _hpBar;
        [SerializeField] private TMP_Text _maxAP_TMP;
        [SerializeField] private TMP_Text _currentAP_TMP;
        [SerializeField] private Graphic _moveIcon;
        [SerializeField] private Color _colorOn = Color.yellow;
        [SerializeField] private Color _colorOff = Color.gray;

        private int _index;
        private Vector3 _offset;

        private Transform _thisTransform;
        private Transform _container, _repository;
        private InputController _inputController;
        private Actor _attachActor;
        
        private Coroutine _activeCn, _moveCn;
        private float _targetAlpha;
        private Vector3 _targetPosition;
        private Unsubscribers _unsubscribers = new();
        private readonly Signer<WarriorButton> _eventRemove = new();

        public RectTransform RectTransform => (RectTransform)transform;
        public int Index
        { 
            get => _index; 
            set
            {
                if (value != _index)
                {
                    _index = value;

                    if (_moveCn != null)
                        StopCoroutine(_moveCn);
                    _moveCn = StartCoroutine(Move_Cn(_offset * value));
                }
            }
        }

        public WarriorButton Init(InputController inputController, Transform container, Action<WarriorButton> action)
        {
            _thisTransform = transform;
            _container = container; _repository = _thisTransform.parent;

            _offset = new(0f, RectTransform.sizeDelta.y * 1.08f, 0f);

            _inputController = inputController;
            _onClick.Add(OnSelectActor);
            _eventRemove.Add(action);

            _maxAP_TMP.color = _colorOff;
            _currentAP_TMP.color = _colorOn;

            _canvasGroup.alpha = _targetAlpha = 0f;
            _canvasGroup.blocksRaycasts = false;

            return this;
        }

        public void Setup(int index, Sprite sprite, Actor actor, bool isOn)
        {
            _index = index;
            _icon.sprite = sprite;
            _attachActor = actor;

            _unsubscribers += actor.Subscribe(OnChangeActor, false);
            _unsubscribers += actor.InteractableReactive.Subscribe((value) => interactable = value);

            var abilities = actor.Abilities;
            _unsubscribers += abilities[ActorAbilityId.MaxHP].Subscribe((maxHP) => _hpBar.MaxValue = maxHP);
            _unsubscribers += abilities[ActorAbilityId.CurrentHP].Subscribe((currentHP) => _hpBar.Value = currentHP);
            _unsubscribers += abilities[ActorAbilityId.MaxAP].Subscribe(maxAP => _maxAP_TMP.text = new(AP_CHAR, maxAP));
            _unsubscribers += abilities[ActorAbilityId.CurrentAP].Subscribe(currentAP => _currentAP_TMP.text = new(AP_CHAR, currentAP));
            _unsubscribers += abilities[ActorAbilityId.IsMove].Subscribe(move => _moveIcon.canvasRenderer.SetColor(move > 0 ? _colorOn : _colorOff));

            _thisTransform.SetParent(_container);
            _thisTransform.localPosition = _offset * index;
            
            if (isOn) Enable();
        }

        #region Enable/Disable/Move
        public Coroutine Enable()
        {
            _targetAlpha = 1f;
            if (_activeCn != null)
                StopCoroutine(_activeCn);

            return _activeCn = StartCoroutine(Enable_Cn());

            #region Local: Enable_Cn()
            //=================================
            IEnumerator Enable_Cn()
            {
                float alpha = _canvasGroup.alpha;
                while (alpha < 1f)
                {
                    _canvasGroup.alpha = alpha += Time.unscaledDeltaTime * _speedOpen;
                    yield return null;
                }

                _canvasGroup.alpha = 1f;
                _canvasGroup.blocksRaycasts = true;
                _activeCn = null;
            }
            #endregion
        }
        public Coroutine Disable()
        {
            _targetAlpha = 0f;
            _canvasGroup.blocksRaycasts = false;

            if (_activeCn != null)
                StopCoroutine(_activeCn);

            return _activeCn = StartCoroutine(Disable_Cn());

            #region Local: Disable_Cn()
            //=================================
            IEnumerator Disable_Cn()
            {
                float alpha = _canvasGroup.alpha;
                while (alpha > 0f)
                {
                    yield return null;
                    _canvasGroup.alpha = alpha -= Time.unscaledDeltaTime * _speedClose;
                }

                _canvasGroup.alpha = 0f;
                _activeCn = null;
            }
            #endregion
        }
        public IEnumerator Move_Cn(Vector3 targetPosition)
        {
            Vector3 startPosition = _thisTransform.localPosition;
            _targetPosition = targetPosition;

            float progress = 0f;

            while (progress < 1f)
            {
                progress += Time.unscaledDeltaTime * _speedMove;
                _thisTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);
                yield return null;
            }

            _thisTransform.localPosition = targetPosition;
            _moveCn = null;
        }
        #endregion

        #region On...
        private void OnSelectActor()
        {
            if(_attachActor != null && _attachActor.Interactable)
                _inputController.Select(_attachActor);
        }
        private void OnChangeActor(Actor actor, TypeEvent typeEvent)
        {
            if (typeEvent == TypeEvent.Remove)
            {
                _unsubscribers.Unsubscribe();
                _attachActor = null;
                _canvasGroup.alpha = _targetAlpha = 0f;
                _canvasGroup.blocksRaycasts = false;
                _thisTransform.SetParent(_repository);
                _eventRemove.Invoke(this);
            }
        }
        #endregion

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_activeCn != null)
            {
                StopCoroutine(_activeCn);
                _canvasGroup.alpha = _targetAlpha;
                _canvasGroup.blocksRaycasts = _targetAlpha > 0.1f;
                _activeCn = null;
            }

            if (_moveCn != null)
            {
                StopCoroutine(_moveCn);
                _thisTransform.localPosition = _targetPosition;
                _moveCn = null;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _unsubscribers.Unsubscribe();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();
            if (_hpBar == null)
                _hpBar = GetComponentInChildren<VBarInt>();
            if (_icon == null)
                _icon = EUtility.GetComponentInChildren<Image>(this, "Icon");
            if (_maxAP_TMP == null)
                _maxAP_TMP = EUtility.GetComponentInChildren<TMP_Text>(this, "MaxAP_TMP");
            if (_currentAP_TMP == null)
                _currentAP_TMP = EUtility.GetComponentInChildren<TMP_Text>(this, "AP_TMP");
            if (_moveIcon == null)
                _moveIcon = EUtility.GetComponentInChildren<Image>(this, "MoveIcon");
        }
#endif
    }
}
