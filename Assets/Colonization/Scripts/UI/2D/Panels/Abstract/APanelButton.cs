using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.UI
{
    public abstract class APanelButton : AVButton
    {
        [SerializeField] protected CanvasGroup _canvasGroup;
        [SerializeField] protected Image _icon;
        [Space]
        [SerializeField] private float _speedOpen = 6f;
        [SerializeField] private float _speedClose = 8f;

        protected IInteractable _attach;
        protected float _targetAlpha;
        protected Subscription _subscription;

        private Coroutine _activeCn;

        public Vector3 Offset { [Impl(256)] get => new(0f, _thisRectTransform.sizeDelta.y * 1.08f, 0f); }

        [Impl(256)] protected void InitInternal()
        {
            _onClick.Add(OnSelect);
            GameContainer.InputController.IsWindowMode.Subscribe(OnWindowMode, false);
        }

        [Impl(256)] protected void Attach(IInteractable attach, Sprite sprite)
        {
            _icon.sprite = sprite;
            _attach = attach;

            _subscription += attach.InteractableReactive.Subscribe(OnAttachInteractable);
        }

        #region Enable/Disable
        public Coroutine Enable()
        {
            _targetAlpha = 1f;
            if (_activeCn != null)
                StopCoroutine(_activeCn);

            return _activeCn = StartCoroutine(Enable_Cn());

            #region Local: Enable_Cn()
            //=================================
            IEnumerator Enable_Cn()
            {
                float alpha = _canvasGroup.alpha;
                while (alpha < 1f)
                {
                    _canvasGroup.alpha = alpha += Time.unscaledDeltaTime * _speedOpen;
                    yield return null;
                }

                _canvasGroup.alpha = 1f;
                _canvasGroup.blocksRaycasts = true;
                _activeCn = null;
            }
            #endregion
        }
        public Coroutine Disable()
        {
            _targetAlpha = 0f;
            _canvasGroup.blocksRaycasts = false;

            if (_activeCn != null)
                StopCoroutine(_activeCn);

            return _activeCn = StartCoroutine(Disable_Cn());

            #region Local: Disable_Cn()
            //=================================
            IEnumerator Disable_Cn()
            {
                float alpha = _canvasGroup.alpha;
                while (alpha > 0f)
                {
                    yield return null;
                    _canvasGroup.alpha = alpha -= Time.unscaledDeltaTime * _speedClose;
                }

                _canvasGroup.alpha = 0f;
                _activeCn = null;
            }
            #endregion
        }
        #endregion

        private void OnAttachInteractable(bool value) => interactable = value & !GameContainer.InputController.IsWindowMode;
        private void OnWindowMode(bool value)
        {
            if(_attach != null)
                interactable = !value & _attach.Interactable;
        } 
        private void OnSelect() => GameContainer.InputController.Select(_attach, MouseButton.Right);

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_activeCn != null)
            {
                StopCoroutine(_activeCn);
                _canvasGroup.alpha = _targetAlpha;
                _canvasGroup.blocksRaycasts = _targetAlpha > 0.1f;
                _activeCn = null;
            }
        }

        sealed protected override void OnDestroy()
        {
            base.OnDestroy();
            _subscription?.Dispose();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            this.SetComponent(ref _canvasGroup);
            this.SetChildren(ref _icon, "Icon");
        }
#endif
    }
}
