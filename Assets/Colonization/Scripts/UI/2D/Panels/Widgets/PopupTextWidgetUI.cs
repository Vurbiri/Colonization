using System.Collections;
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    sealed public class PopupTextWidgetUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _thisTMP;
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
        private GameObject _thisGameObject;
        private Color _colorPlus, _colorMinus;
        private CoroutinesQueue _queue;

        public void Init(ProjectColors settings, Vector3 direction)
        {
            _thisTransform = transform;
            _thisGameObject = gameObject;

            _positionStart = _thisTransform.localPosition;
            _positionEnd = _positionStart + direction * _distance;

            _colorPlus = settings.TextPositive;
            _colorMinus = settings.TextNegative;

            _scaleColorSpeed = 1f / (1f - _startHide);

            _queue = new(this, () => _thisGameObject.SetActive(false));
            _thisGameObject.SetActive(false);
        }

        public void Run(int delta)
        {
            if (delta != 0)
            {
                _thisGameObject.SetActive(true);
                _queue.Enqueue(Run_Cn(delta > 0 ? _colorPlus : _colorMinus, delta.ToString("+#;-#;0")));
            }
        }

        private IEnumerator Run_Cn(Color textColor, string text)
        {
            float lerpVector = 0f, lerpAlpha = 0f, deltaTime;
            Color color = Color.white;

            _thisTMP.canvasRenderer.SetColor(color);
            _thisTMP.text = text;
            _thisTMP.color = textColor;

            while (lerpVector < 1f)
            {
                _thisTransform.localPosition = Vector3.Lerp(_positionStart, _positionEnd, lerpVector);

                deltaTime = Time.unscaledDeltaTime * _speed;
                lerpVector += deltaTime;

                if (lerpVector > _startHide)
                {
                    color.a = Mathf.Lerp(1f, _minAlpha, lerpAlpha);
                    _thisTMP.canvasRenderer.SetColor(color);
                    lerpAlpha += deltaTime * _scaleColorSpeed;
                }

                yield return null;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_thisTMP == null)
                _thisTMP = GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
