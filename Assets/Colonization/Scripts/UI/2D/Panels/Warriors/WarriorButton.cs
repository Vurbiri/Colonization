using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

        private Transform _container, _repository;
        private readonly VAction<WarriorButton> _eventRemove = new();

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

        public WarriorButton Init(Transform container, Action<WarriorButton> toPool)
        {
            _container = container;
            _repository = _move.Transform.parent;

            _offset = Offset;

            _canvasGroup.alpha = _targetAlpha = 0f;
            _canvasGroup.blocksRaycasts = false;

            _eventRemove.Add(toPool);

            _maxAP_TMP.color = _colorOff;
            _currentAP_TMP.color = _colorOn;

            _hpBar.Init();

            InitInternal();

            return this;
        }

        public void Setup(int index, Sprite sprite, Actor actor, bool isOn)
        {
            _index = index;

            _subscription += actor.Subscribe(OnChangeActor, false);

            var abilities = actor.Abilities;
            _subscription += abilities[ActorAbilityId.MaxHP].Subscribe(_hpBar.SetMaxHP);
            _subscription += abilities[ActorAbilityId.CurrentHP].Subscribe(_hpBar.SetCurrentHP);
            _subscription += abilities[ActorAbilityId.MaxAP].Subscribe(maxAP => _maxAP_TMP.text = new(AP_CHAR, maxAP));
            _subscription += abilities[ActorAbilityId.CurrentAP].Subscribe(currentAP => _currentAP_TMP.text = new(AP_CHAR, currentAP));
            _subscription += abilities[ActorAbilityId.IsMove].Subscribe(move => _moveIcon.color = move > 0 ? _colorOn : _colorOff);

            _move.Transform.SetParent(_container);
            _move.Transform.localPosition = _offset * index;

            Attach(actor, sprite);

            if (isOn) Enable();
        }

        private void OnChangeActor(Actor actor, TypeEvent typeEvent)
        {
            if (typeEvent == TypeEvent.Remove)
            {
                _subscription.Dispose();
                _attach = null;
                _canvasGroup.alpha = _targetAlpha = 0f;
                _canvasGroup.blocksRaycasts = false;
                _move.Transform.SetParent(_repository);
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
