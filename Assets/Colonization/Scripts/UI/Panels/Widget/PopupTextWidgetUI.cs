using System.Collections;
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PopupTextWidgetUI : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI _thisTMP;
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
        private string _stringPlus, _stringMinus;
        private CoroutinesQueue _queue;

        public void Init(ProjectColors settings, Vector3 direction)
        {
            _thisTransform = transform;
            _thisGameObject = gameObject;

            _positionStart = _thisTransform.localPosition;
            _positionEnd = _positionStart + direction * _distance;

            _stringPlus = settings.TextPositiveTag.Concat(" +{0}");
            _stringMinus = settings.TextNegativeTag.Concat(" {0}");

            _scaleColorSpeed = 1f / (1f - _startHide);

            _queue = new(this, () => _thisGameObject.SetActive(false));
            _thisGameObject.SetActive(false);
        }

        public void Run(int delta)
        {
            if (delta != 0)
            {
                _thisGameObject.SetActive(true);
                _queue.Enqueue(Run_Cn(string.Format(delta > 0 ? _stringPlus : _stringMinus, delta)));
            }
        }

        private IEnumerator Run_Cn(string text)
        {
            float lerpVector = 0f, lerpColor = 0f, delta;
            Color color = Color.white;

            _thisTMP.canvasRenderer.SetColor(color);
            _thisTMP.text = text;

            while (lerpVector < 1f)
            {
                _thisTransform.localPosition = Vector3.Lerp(_positionStart, _positionEnd, lerpVector);

                delta = Time.unscaledDeltaTime * _speed;
                lerpVector += delta;

                if (lerpVector > _startHide)
                {
                    color.a = Mathf.Lerp(1f, _minAlpha, lerpColor);
                    _thisTMP.canvasRenderer.SetColor(color);
                    lerpColor += delta * _scaleColorSpeed;
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
