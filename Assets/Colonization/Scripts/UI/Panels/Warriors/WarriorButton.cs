using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.UI
{
    sealed public class WarriorButton : APanelButton
    {
        private const char AP_CHAR = '+';

        [SerializeField] private float _speedMove = 4f;
        [Space]
        [SerializeField] private HPBar _hpBar;
        [SerializeField] private TextMeshProUGUI _maxAP_TMP;
        [SerializeField] private TextMeshProUGUI _currentAP_TMP;
        [SerializeField] private Graphic _moveIcon;
        [SerializeField] private Color _colorOn = Color.yellow;
        [SerializeField] private Color _colorOff = Color.gray;

        private int _index;
        private Vector3 _offset;

        private Transform _thisTransform, _container, _repository;
       
        private Coroutine _moveCn;
        private Vector3 _targetPosition;
        private readonly Subscription<WarriorButton> _eventRemove = new();

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
            _container = container;

            _offset = Offset;

            _canvasGroup.alpha = _targetAlpha = 0f;
            _canvasGroup.blocksRaycasts = false;

            _repository = _thisTransform.parent;

            _eventRemove.Add(action);

            _maxAP_TMP.color = _colorOff;
            _currentAP_TMP.color = _colorOn;

            InitClick(inputController);

            return this;
        }

        public void Setup(int index, Sprite sprite, Actor actor, bool isOn)
        {
            _index = index;

            _unsubscribers += actor.Subscribe(OnChangeActor, false);

            var abilities = actor.Abilities;
            _unsubscribers += abilities[ActorAbilityId.MaxHP].Subscribe(_hpBar.SetMaxHP);
            _unsubscribers += abilities[ActorAbilityId.CurrentHP].Subscribe(_hpBar.SetCurrentHP);
            _unsubscribers += abilities[ActorAbilityId.MaxAP].Subscribe(maxAP => _maxAP_TMP.text = new(AP_CHAR, maxAP));
            _unsubscribers += abilities[ActorAbilityId.CurrentAP].Subscribe(currentAP => _currentAP_TMP.text = new(AP_CHAR, currentAP));
            _unsubscribers += abilities[ActorAbilityId.IsMove].Subscribe(move => _moveIcon.color = move > 0 ? _colorOn : _colorOff);
            
            _thisTransform.SetParent(_container);
            _thisTransform.localPosition = _offset * index;

            Attach(actor, sprite);

            if (isOn) Enable();
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

        private void OnChangeActor(Actor actor, TypeEvent typeEvent)
        {
            if (typeEvent == TypeEvent.Remove)
            {
                _unsubscribers.Unsubscribe();
                _attach = null;
                _canvasGroup.alpha = _targetAlpha = 0f;
                _canvasGroup.blocksRaycasts = false;
                _thisTransform.SetParent(_repository);
                _eventRemove.Invoke(this);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _hpBar.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_moveCn != null)
            {
                StopCoroutine(_moveCn);
                _thisTransform.localPosition = _targetPosition;
                _moveCn = null;
            }
        }

        #region Nested class HPBar
        [Serializable]
        private class HPBar
        {
            [SerializeField] private RectTransform _fill;
            private float _currentHP;
            private float _maxHP = 100;

            public void SetMaxHP(int maxHP) => Set(_currentHP, maxHP);
            public void SetCurrentHP(int currentHP) => Set(currentHP, _maxHP);

            public void OnEnable() => _fill.anchorMin = Vector2.zero;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void Set(float currentHP, float maxHP)
            {
                _currentHP = currentHP;
                _maxHP = maxHP; 
                _fill.anchorMax = new(currentHP / maxHP, 1f);
            }

#if UNITY_EDITOR
            public void OnValidate(Component parent)
            {
                if (_fill == null)
                    _fill = EUtility.GetComponentInChildren<RectTransform>(parent, "Fill");
            }
#endif
        }
        #endregion

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_icon == null)
                _icon = EUtility.GetComponentInChildren<Image>(this, "Icon");
            if (_maxAP_TMP == null)
                _maxAP_TMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(this, "MaxAP_TMP");
            if (_currentAP_TMP == null)
                _currentAP_TMP = EUtility.GetComponentInChildren<TextMeshProUGUI>(this, "AP_TMP");
            if (_moveIcon == null)
                _moveIcon = EUtility.GetComponentInChildren<Image>(this, "MoveIcon");

            _hpBar.OnValidate(this);
        }
#endif
    }
}
