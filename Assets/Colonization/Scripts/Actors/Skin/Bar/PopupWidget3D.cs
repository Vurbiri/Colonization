using System.Collections;
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    public class PopupWidget3D : MonoBehaviour
    {
        private const int MIN_VALUE = -1000;

        [SerializeField] protected SpriteRenderer _sprite;
        [SerializeField] protected TextMeshPro _valueTMP;
        [Space]
        [SerializeField, Range(0.1f, 2f)] private float _speed = 0.75f;
        [Space]
        [SerializeField, Range(1f, 5f)] private float _distance = 3f;
        [Space]
        [SerializeField, Range(0.05f, 1f)] private float _minAlpha = 0.25f;
        [SerializeField, Range(0.05f, 1f)] private float _startHide = 0.7f;

        private Transform _thisTransform;
        private CoroutinesQueue _queue;

        private float _scaleColorSpeed;
        private Vector3 _positionStart, _positionEnd;
        private Color _colorPlusStart, _colorPlusEnd;
        private Color _colorMinusStart, _colorMinusEnd;

        private Color Color { set => _sprite.color = _valueTMP.color = value; }

        public void Init(int orderLevel)
        {
            var colors = GameContainer.UI.Colors;

            _sprite.sortingOrder += orderLevel;
            _valueTMP.sortingOrder += orderLevel;

            _thisTransform = GetComponent<Transform>();
            _queue = new(this, () => gameObject.SetActive(false));

            _colorPlusStart = _colorPlusEnd = colors.TextPositive;
            _colorMinusStart = _colorMinusEnd = colors.TextNegative;
            _colorPlusEnd.a = _colorMinusEnd.a = _minAlpha;

            _scaleColorSpeed = 1f / (1f - _startHide);

            _positionStart = _thisTransform.localPosition;
            _positionEnd = _positionStart + new Vector3(0f, _distance, 0f);

            gameObject.SetActive(false);
        }

        public void Run(int delta, Sprite sprite)
        {
            if (delta != 0 & delta > MIN_VALUE)
            {
                gameObject.SetActive(true);
                _queue.Enqueue(Run_Cn(delta, sprite));
            }
        }

        protected IEnumerator Run_Cn(int value, Sprite sprite)
        {
            float lerpVector = 0f, lerpColor = 0f, delta;
            Color start, end;

            if (value > 0)
            {
                _valueTMP.text = string.Concat("+", value.ToStr());
                start = _colorPlusStart;
                end = _colorPlusEnd;
            }
            else
            {
                _valueTMP.text = value.ToStr();
                start = _colorMinusStart;
                end = _colorMinusEnd;
            }

            _sprite.sprite = sprite;
            Color = start;

            while (lerpVector < 1f)
            {
                _thisTransform.localPosition = Vector3.Lerp(_positionStart, _positionEnd, lerpVector);

                delta = Time.unscaledDeltaTime * _speed;
                lerpVector += delta;

                if (lerpVector > _startHide)
                {
                    Color = Color.Lerp(start, end, lerpColor);
                    lerpColor += delta * _scaleColorSpeed;
                }

                yield return null;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_sprite == null)
                _sprite = GetComponent<SpriteRenderer>();
            if (_valueTMP == null)
                _valueTMP = GetComponentInChildren<TextMeshPro>();
        }
#endif
    }
}
