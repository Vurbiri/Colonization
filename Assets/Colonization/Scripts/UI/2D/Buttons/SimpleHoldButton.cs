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
        [SerializeField, MinMax(0.1f, 2.5f)] private WaitRealtime _clickPeriod = 0.4f;
        [Space]
        [SerializeField] private Image _target;
        [SerializeField] private IdArray<StateId, Color> _colors = new(Color.white);
        [SerializeField, Range(0.01f, 0.5f)] private float _fadeDuration = 0.1f;
        
        private readonly Subscription _onClick = new();
        private bool _inside, _hold;

        public float FadeDuration { get => _fadeDuration; set => _fadeDuration = value; }

        public Color ColorDisabled => _colors[StateId.Disabled];

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

        public Unsubscription AddListener(Action action) => _onClick.Add(action);
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

        public void CopyFrom(SimpleHoldButton other)
        {
            if (other == null)
                return;

            _clickPeriod = other._clickPeriod;
            _fadeDuration = other._fadeDuration;

            _colors = new(other._colors);
        }

        private IEnumerator Hold_Cn()
        {
            while (_interactable & _hold & _inside)
            {
                _onClick.Invoke();
                yield return _clickPeriod.Restart();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetColor(Color color)
        {
            if (_interactable)
                _target.CrossFadeColor(color, _fadeDuration, true, true);
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
#endif
    }
}
