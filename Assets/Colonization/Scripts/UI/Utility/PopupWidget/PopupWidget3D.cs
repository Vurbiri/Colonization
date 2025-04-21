//Assets\Colonization\Scripts\UI\Utility\PopupWidget\PopupWidget3D.cs
using System.Collections;
using TMPro;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Characteristics;

namespace Vurbiri.Colonization.UI
{
    public class PopupWidget3D : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer _sprite;
        [SerializeField] protected TextMeshPro _valueTMP;
        [Space]
        [SerializeField, Range(0.1f, 2f)] private float _speed = 0.75f;
        [Space]
        [SerializeField, Range(1f, 5f)] private float _distance = 3f;
        [Space]
        [SerializeField, Range(0.05f, 1f)] private float _minAlpha = 0.25f;
        [SerializeField, Range(0.05f, 1f)] private float _startHide = 0.7f;
        [Space]
        [SerializeField] private Direction2 _directionPopup;

        private Transform _thisTransform;
        private GameObject _thisGameObject;
        private CoroutinesQueue _queue;
        private IdArray<ActorAbilityId, Sprite> _sprites;

        private float _scaleColorSpeed;
        private Vector3 _positionStart, _positionEnd;
        private Color _colorPlusStart, _colorPlusEnd;
        private Color _colorMinusStart, _colorMinusEnd;

        private Color Color { set => _sprite.color = _valueTMP.color = value; }

        public void Init(IdArray<ActorAbilityId, Sprite> sprites, int orderLevel)
        {
            _sprite.sortingOrder += orderLevel;
            _valueTMP.sortingOrder += orderLevel;

            _thisTransform = transform;
            _thisGameObject = gameObject;
            _sprites = sprites;
            _queue = new(this, () => _thisGameObject.SetActive(false));

            Vurbiri.UI.TextColorSettings settings = SceneContainer.Get<Vurbiri.UI.TextColorSettings>();

            _colorPlusStart = _colorPlusEnd = settings.ColorPositive;
            _colorMinusStart = _colorMinusEnd = settings.ColorNegative;
            _colorPlusEnd.a = _colorMinusEnd.a = _minAlpha;

            _scaleColorSpeed = 1f / (1f - _startHide);

            _positionStart = _thisTransform.localPosition;
            _positionEnd = _positionStart + ((Vector3)_directionPopup) * _distance;
            
            _thisGameObject.SetActive(false);
        }

        public void Run(int delta, Id<ActorAbilityId> id)
        {
            if (delta == 0)
                return;

            _thisGameObject.SetActive(true);
            _queue.Enqueue(Run_Cn(delta, id));
        }

        protected IEnumerator Run_Cn(int value, Id<ActorAbilityId> id)
        {
            float lerpVector = 0f, lerpColor = 0f, delta;
            Color start, end;

            if(value > 0)
            {
                _valueTMP.text = $"+{value}";
                start = _colorPlusStart;
                end = _colorPlusEnd;
            }
            else
            {
                _valueTMP.text = value.ToString();
                start = _colorMinusStart;
                end = _colorMinusEnd;
            }

            _sprite.sprite = _sprites[id];
            Color = start;

            while (lerpVector < 1f)
            {
                _thisTransform.localPosition = Vector3.Lerp(_positionStart, _positionEnd, lerpVector);

                delta = Time.deltaTime * _speed;
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
