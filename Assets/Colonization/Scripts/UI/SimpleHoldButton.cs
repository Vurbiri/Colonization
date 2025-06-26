using System;
using System.Collections;
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
        [SerializeField, MinMax(0.1f, 2.5f)] private WaitRealtime _clickPeriod = 0.4f;
        [Space]
        [SerializeField] private Image _target;
        [SerializeField] private IdArray<StateId, Color> _colors = new(Color.white);
        [SerializeField, Range(0.01f, 0.5f)] private float _fadeDuration = 0.1f;
        
        private readonly Subscription _onClick = new();
        private bool _interactable = true, _inside, _hold;
        private Coroutine _clickCoroutine;

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
                _clickCoroutine ??= StartCoroutine(Hold_Cn());
            }
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            _hold = false;
            if (_interactable)
                _target.CrossFadeColor(_colors[_inside ? StateId.Inside : StateId.Normal], _fadeDuration, true, true);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _inside = true;
            if (_interactable)
                _target.CrossFadeColor(_colors[StateId.Inside], _fadeDuration, true, true);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            _inside = false;
            if (_interactable)
                _target.CrossFadeColor(_colors[StateId.Normal], _fadeDuration, true, true);
        }

        private IEnumerator Hold_Cn()
        {
            while (_interactable & _hold & _inside)
            {
                _onClick.Invoke();
                yield return _clickPeriod.Restart();
            }
            _clickCoroutine = null;
        }

        private void OnEnable()
        {
            _target.canvasRenderer.SetColor(_colors[_interactable ? StateId.Normal : StateId.Disabled]);
        }

        private void OnDisable()
        {
            _hold = _inside = false;
        }

        #region Nested SelectionStateId
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
        private void OnValidate()
        {
            EUtility.SetComponent(ref _target, this);
        }

        public void CopyFrom_Editor(SimpleHoldButton other)
        {
            if(other == null || Equals(other))
                return;
            
            _clickPeriod = other._clickPeriod;
            _fadeDuration = other._fadeDuration;

            _colors = new(other._colors);
        }

        private bool Equals(SimpleHoldButton other)
        {
            for(int i = 0; i < StateId.Count; i++)
                if (_colors[i] != other._colors[i])
                    return false;

            return _clickPeriod == other._clickPeriod & _fadeDuration == other._fadeDuration;
        }
#endif
    }
}
