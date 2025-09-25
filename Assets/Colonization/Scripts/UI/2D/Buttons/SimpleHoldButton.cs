using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vurbiri.Collections;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(Image))]
    public class SimpleHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private bool _interactable = true;
        [SerializeField, Range(0.1f, 1f)] private float _period = 0.4f;
        [SerializeField, Inverse(0.05f, 0.95f)] private float _acceleration = 0.87f;
        [Space]
        [SerializeField] private Image _target;
        [SerializeField] private IdArray<StateId, Color> _colors = new(Color.white);
        [SerializeField, Range(0.01f, 0.5f)] private float _fadeDuration = 0.1f;
        
        private readonly VAction _onClick = new();
        private bool _inside, _hold;
        private WaitRealtime _clickPeriod;

        public float FadeDuration { get => _fadeDuration; set => _fadeDuration = value; }

        public Color NormalColor    { get => _colors[StateId.Normal];   set => _colors[StateId.Normal]   = value; }
        public Color InsideColor    { get => _colors[StateId.Inside];   set => _colors[StateId.Inside]   = value; }
        public Color HoldColor      { get => _colors[StateId.Hold];     set => _colors[StateId.Hold]     = value; }
        public Color DisabledColor  { get => _colors[StateId.Disabled]; set => _colors[StateId.Disabled] = value; }

        public bool Interactable
        {
            get => _interactable;
            set
            {
                if (_interactable != value)
                {
                    _interactable = value;
                    if (value)
                        _target.CrossFadeColor(_colors[_inside ? StateId.Inside : StateId.Normal], _fadeDuration, true, true);
                    else
                        _target.CrossFadeColor(_colors[StateId.Disabled], _fadeDuration, true, true);
                }
            }
        }

        public Subscription AddListener(Action action) => _onClick.Add(action);
        public void RemoveListener(Action action) => _onClick.Remove(action);

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_interactable & eventData.button == PointerEventData.InputButton.Left)
            {
                _hold = true;
                _target.CrossFadeColor(_colors[StateId.Hold], _fadeDuration, true, true);
                StartCoroutine(Hold_Cn());
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            _hold = false;
            SetColor(_colors[_inside ? StateId.Inside : StateId.Normal]);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _inside = true;
            SetColor(_colors[StateId.Inside]);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            _inside = false;
            SetColor(_colors[StateId.Normal]);
        }

        private IEnumerator Hold_Cn()
        {
            float time = _period;
            while (_interactable & _hold & _inside)
            {
                _onClick.Invoke();
                yield return _clickPeriod.Restart(time);
                time *= _acceleration;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetColor(Color color)
        {
            if (_interactable)
                _target.CrossFadeColor(color, _fadeDuration, true, true);
        }

        private void Awake()
        {
            _clickPeriod = _period;
        }

        private void OnEnable()
        {
            _target.canvasRenderer.SetColor(_colors[_interactable ? StateId.Normal : StateId.Disabled]);
        }

        private void OnDisable()
        {
            _hold = _inside = false;
        }

        #region Nested StateId
        private abstract class StateId : IdType<StateId>
        {
            public const int Normal   = 0;
            public const int Inside   = 1;
            public const int Hold     = 2;
            public const int Disabled = 3;

            static StateId() => ConstructorRun();
        }
        #endregion

#if UNITY_EDITOR

        public RectTransform RectTransformE => _target.rectTransform;

        private void OnValidate()
        {
            if (!Application.isPlaying)
            {
                this.SetComponent(ref _target);
                _target.canvasRenderer.SetColor(_colors[_interactable ? StateId.Normal : StateId.Disabled]);
            }
        }

        public void CopyFrom_Editor(SimpleHoldButton other)
        {
            if (other == null)
                return;

            _period = other._period;
            _acceleration = other._acceleration;
            _fadeDuration = other._fadeDuration;

            _colors = new(other._colors);
        }
#endif
    }
}
