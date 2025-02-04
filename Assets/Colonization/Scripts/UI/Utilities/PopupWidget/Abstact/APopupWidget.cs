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
        protected string _stringPlus, _stringMinus;
        protected WaitQueue _queue;
        
        protected void Init(Vector3 direction)
        {
            _thisTransform = transform;
            _self = gameObject;

            _positionStart = _thisTransform.localPosition;
            _positionEnd = _positionStart + direction * _distance;

            Vurbiri.UI.SettingsTextColor settings = SceneData.Get<Vurbiri.UI.SettingsTextColor>();

            _stringPlus = settings.HexColorPositive.Concat(" +{0}");
            _stringMinus = settings.HexColorNegative.Concat(" {0}");

            _scaleColorSpeed = 1f / (1f - _startHide);

            _queue = new(this, () => _self.SetActive(false));
            _self.SetActive(false);
        }


        protected IEnumerator Run_Coroutine(string text)
        {
            float lerpVector = 0f, lerpColor = 0f, delta;
            Color color = Color.white;

            _thisTMP.color = color;
            _thisTMP.text = text;

            while (lerpVector < 1f)
            {
                _thisTransform.localPosition = Vector3.Lerp(_positionStart, _positionEnd, lerpVector);

                delta = Time.deltaTime * _speed;
                lerpVector += delta;

                if (lerpVector > _startHide)
                {
                    color.a = Mathf.Lerp(1f, _minAlpha, lerpColor);
                    _thisTMP.color = color;
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
