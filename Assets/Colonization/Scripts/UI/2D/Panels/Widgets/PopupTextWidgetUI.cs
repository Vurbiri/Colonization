using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PopupTextWidgetUI : MonoBehaviour
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
        private CoroutinesQueue _queue;

        public void Init(Vector3 direction)
        {
            _thisTransform = transform;
            _thisGameObject = gameObject;

            _positionStart = _thisTransform.localPosition;
            _positionEnd = _positionStart + direction * _distance;

            _scaleColorSpeed = 1f / (1f - _startHide);

            _queue = new(() => _thisGameObject.SetActive(false), this);
            _thisGameObject.SetActive(false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Run(int delta)
        {
            if (delta != 0)
                ForceRun(delta);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ForceRun(int delta)
        {
            _thisGameObject.SetActive(true);
            _queue.Enqueue(Run_Cn(GameContainer.UI.Colors.GetTextColor(delta > 0), delta.ToString("+#;-#;0")));
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
            this.SetComponent(ref _thisTMP);
        }
#endif
    }
}
