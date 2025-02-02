//Assets\Colonization\Scripts\UI\Utilities\PopupWidget\Abstact\APopupWidget.cs
using System.Collections;
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public abstract class APopupWidget<T> : MonoBehaviour where T : TMP_Text
    {
        [SerializeField] protected T _thisTMP;
        [Space]
        [SerializeField, Range(0.1f, 2f)] private float _speed = 0.6f;
        [Space]
        [SerializeField] private float _distance = 100f;
        [Space]
        [SerializeField, Range(0.05f, 1f)] private float _minAlpha = 0.25f;
        [SerializeField, Range(0.05f, 1f)] private float _startHide = 0.67f;

        private Transform _thisTransform;
        private float _scaleColorSpeed;
        private Vector3 _positionStart, _positionEnd;

        protected GameObject _self;
        protected Color _colorPlusStart, _colorMinusStart;
        protected Color _colorPlusEnd, _colorMinusEnd;
        protected WaitQueue _queue;
        

        protected void Init(Vector3 direction)
        {
            _thisTransform = transform;
            _self = gameObject;

            _positionStart = _thisTransform.localPosition;
            _positionEnd = _positionStart + direction * _distance;

            Vurbiri.UI.SettingsTextColor settings = SceneData.Get<Vurbiri.UI.SettingsTextColor>();

            _colorPlusEnd = _colorPlusStart = settings.ColorPositive;
            _colorMinusEnd = _colorMinusStart = settings.ColorNegative;
            _colorPlusEnd.a = _colorMinusEnd.a = _minAlpha;

            _scaleColorSpeed = 1f / (1f - _startHide);

            _queue = new(this, () => _self.SetActive(false));
            _self.SetActive(false);
        }


        protected IEnumerator Run_Coroutine(string text, Color start, Color end)
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

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_thisTMP == null)
                _thisTMP = GetComponent<T>();
        }
#endif
    }
}
