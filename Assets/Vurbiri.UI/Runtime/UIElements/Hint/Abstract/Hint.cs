using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
    public abstract class Hint : MonoBehaviour
    {
        internal readonly static Hint[] s_instances = new Hint[HintId.Count];
        
        [SerializeField, ReadOnly] private Id<HintId> _type;
        [Space]
        [SerializeField] private Image _backImage;
        [SerializeField] private TextMeshProUGUI _hintTMP;
        [Space]
        [SerializeField] private Vector2 _maxSize;
        [SerializeField] private Vector2 _padding;
        [Space]
        [SerializeField, MinMax(0f, 5f)] private WaitRealtime _timeDelay;
        [SerializeField] private CanvasGroupSwitcher _switcher;

        protected RectTransform _backTransform, _hintTransform;
        private Coroutine _coroutineShow, _coroutineHide;

        [Impl(256)] public static Hint GetInstance(Id<HintId> id) => s_instances[id];
        [Impl(256)] public static THint GetInstance<THint>() where THint : Hint => (THint)s_instances[HintId.Get<THint>()];

        public static THint FindInstance<THint>() where THint : Hint
        {
            int id = HintId.Get<THint>();
            if (s_instances[id] == null)
            {
                var instances = FindObjectsOfType<THint>();
                if (instances != null && instances.Length > 0)
                {
                    s_instances[id] = instances[0];
                    for (int i = instances.Length - 1; i > 0; --i)
                        Destroy(instances[i].gameObject);
                }
            }
            return (THint)s_instances[id];
        }

        private void Awake() => Init();

        public virtual bool Init()
        {
            var instance = s_instances[_type];

            if (instance == null)
            {
                s_instances[_type] = this;

                _switcher.Disable();

                _backTransform = _backImage.rectTransform;
                _hintTransform = _hintTMP.rectTransform;

                _hintTMP.enableWordWrapping = true;
                _hintTMP.overflowMode = TextOverflowModes.Overflow;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return false;
            }
            return true;
        }

        [Impl(256)] public bool Show(string text, Transform transform, Vector3 offset)
        {
            bool result;
            if (result = !string.IsNullOrEmpty(text) & gameObject.activeInHierarchy)
            {
                StopCoroutine(ref _coroutineShow);
                _coroutineShow = StartCoroutine(Show_Cn(text, transform, offset));
            }
            return result;
        }
        [Impl(256)] public bool Hide()
        {
            StopCoroutine(ref _coroutineShow);
            _coroutineHide ??= StartCoroutine(Hide_Cn());

            return true;
        }

        [Impl(256)] public void SetColors(Color backColor, Color textColor)
        {
            _backImage.color = backColor;
            _hintTMP.color = textColor;
        }

        protected abstract void SetPosition(Transform transform, Vector3 offset);

        private IEnumerator Show_Cn(string text, Transform transform, Vector3 offset)
        {
            yield return _timeDelay.Restart();

            StopCoroutine(ref _coroutineHide);

            _hintTransform.sizeDelta = _maxSize;
            _hintTMP.text = text;
            _hintTMP.ForceMeshUpdate();

            Vector2 size = _hintTMP.textBounds.size;

            _hintTransform.sizeDelta = size;
            _backTransform.sizeDelta = size + _padding;

            yield return null;
            SetPosition(transform, offset);

            yield return _switcher.Show();
            _coroutineShow = null;
        }

        private IEnumerator Hide_Cn()
        {
            yield return _switcher.Hide();
            _coroutineHide = null;
        }

        [Impl(256)] private void StopCoroutine(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private void OnDisable()
        {
            _switcher.Disable();
            _coroutineHide = null;
            _coroutineShow = null;
        }

        private void OnDestroy()
        {
            if (s_instances[_type] == this)
                s_instances[_type] = null;
        }

#if UNITY_EDITOR
        public virtual void UpdateVisuals_Ed(Color backColor, Color textColor)
        {
            SetColors(backColor, textColor);

            _hintTMP.rectTransform.sizeDelta = _maxSize;
            _backImage.rectTransform.sizeDelta = _maxSize + _padding;
        }

        protected virtual void OnValidate()
        {
            _type = HintId.Get(GetType());

            this.SetChildren(ref _backImage);
            this.SetChildren(ref _hintTMP);

            _switcher.OnValidate(this, 8);
        }
#endif
    }
}
