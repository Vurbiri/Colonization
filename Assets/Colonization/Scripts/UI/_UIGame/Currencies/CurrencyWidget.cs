using System.Collections;
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class CurrencyWidget : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 2f)] private float _speed = 0.6f;
        [Space]
        [SerializeField] private Color _colorPlus = Color.green;
        [SerializeField] private Color _colorMinus = Color.red;
        [Space]
        [SerializeField] private float _distance = 100f;
        [Space]
        [SerializeField, Range(0.05f, 1f)] private float _minAlpha = 0.25f;
        [SerializeField, Range(0.05f, 1f)] private float _startHide = 0.67f;

        private TMP_Text _thisTMP;
        private Transform _thisTransform;
        private Color _colorPlusEnd, _colorMinusEnd;
        private Vector3 _positionStart, _positionEnd;
        private int _prevValue = -1;
        private float _scaleColorSpeed;
        private WaitQueue _queue;
        private GameObject _self;

        public void Init(Vector3 direction)
        {
            _thisTMP = GetComponent<TMP_Text>();
            _thisTransform = transform;
            _self = gameObject;

            _positionStart = _thisTransform.localPosition;
            _positionEnd = _positionStart + direction * _distance;

            _colorPlusEnd = _colorPlus;
            _colorMinusEnd = _colorMinus;
            _colorPlusEnd.a = _colorMinusEnd.a = _minAlpha;

            _scaleColorSpeed = 1f / (1f - _startHide);

            _queue = new(this, () => _self.SetActive(false));
            _self.SetActive(false);
        }

        public void Run(int value)
        {
            int delta = value - _prevValue;

            if (_prevValue < 0 || delta == 0)
            {
               _prevValue = value;
                return;
            }

            _self.SetActive(true);
            if (delta > 0)
                _queue.Add(Run_Coroutine($"+{delta}", _colorPlus, _colorPlusEnd));
            else
                _queue.Add(Run_Coroutine(delta.ToString(), _colorMinus, _colorMinusEnd));

            _prevValue = value;
        }

        private IEnumerator Run_Coroutine(string text, Color start, Color end)
        {
            float lerpVector = 0f, lerpColor = 0f, delta;
            _thisTMP.text = text;
            _thisTMP.color = start;
            while (lerpVector < 1f)
            {
                delta = Time.deltaTime * _speed;

                lerpVector += delta;
                _thisTransform.localPosition = Vector3.Lerp(_positionStart, _positionEnd, lerpVector);
                
                if (lerpVector > _startHide)
                {
                    _thisTMP.color = Color.Lerp(start, end, lerpColor);
                    lerpColor += delta * _scaleColorSpeed;
                }

                yield return null;
            }
        }
    }
}
