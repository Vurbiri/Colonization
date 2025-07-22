using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Actors;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Reactive;
using Vurbiri.Reactive.Collections;

namespace Vurbiri.Colonization.UI
{
    sealed public class WarriorButton : APanelButton
    {
        private const char AP_CHAR = '+';

        [Space]
        [SerializeField] private MoveUsingLerp _move;
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
        private readonly Subscription<WarriorButton> _eventRemove = new();

        public int Index
        { 
            get => _index;
            set
            {
                if (value != _index)
                {
                    _index = value;
                    _move.Run(this, _offset * value);
                }
            }
        }

        public WarriorButton Init(Transform container, Action<WarriorButton> action)
        {
            _thisTransform = _move.Transform;
            _container = container;

            _offset = Offset;

            _canvasGroup.alpha = _targetAlpha = 0f;
            _canvasGroup.blocksRaycasts = false;

            _repository = _thisTransform.parent;

            _eventRemove.Add(action);

            _maxAP_TMP.color = _colorOff;
            _currentAP_TMP.color = _colorOn;

            _hpBar.Init();

            InitClick();

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

        protected override void OnDisable()
        {
            base.OnDisable();

            _move.Skip();
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

            public void Init() => _fill.anchorMin = Vector2.zero;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private void Set(float currentHP, float maxHP)
            {
                _currentHP = currentHP; _maxHP = maxHP;
                _fill.anchorMax = new(currentHP / maxHP, 1f);
            }

#if UNITY_EDITOR
            public void OnValidate(Component parent) => parent.SetChildren(ref _fill, "Fill");
#endif
        }
        #endregion

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            _hpBar.OnValidate(this);

            this.SetChildren(ref _icon, "Icon");
            this.SetChildren(ref _maxAP_TMP, "MaxAP_TMP");
            this.SetChildren(ref _currentAP_TMP, "AP_TMP");
            this.SetChildren(ref _moveIcon, "MoveIcon");
        }
#endif
    }
}
