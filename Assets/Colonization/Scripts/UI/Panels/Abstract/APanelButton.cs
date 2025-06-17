using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Vurbiri.Colonization.Controllers;
using Vurbiri.Reactive;
using Vurbiri.UI;

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
        protected Unsubscriptions _unsubscribers = new();
        
        private InputController _inputController;
        private Coroutine _activeCn;

        public RectTransform RectTransform => (RectTransform)transform;
        public Vector3 Offset => new(0f, RectTransform.sizeDelta.y * 1.08f, 0f);

        protected void InitClick(InputController inputController)
        {
            _inputController = inputController;
            _onClick.Add(OnSelect);
        }

        protected void Attach(IInteractable attach, Sprite sprite)
        {
            _icon.sprite = sprite;
            _attach = attach;

            _unsubscribers += attach.InteractableReactive.Subscribe((value) => interactable = value);
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

        private void OnSelect()
        {
            if (_attach != null && _attach.Interactable)
                _inputController.Select(_attach);
        }

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
            _unsubscribers.Unsubscribe();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            EUtility.SetComponent(ref _canvasGroup, this);
            EUtility.SetChildren(ref _icon, this, "Icon");
        }
#endif
    }
}
