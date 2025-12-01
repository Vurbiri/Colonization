using TMPro;
using UnityEngine;
using Vurbiri.International;
using Vurbiri.UI;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(UnityEngine.UI.Graphic))]
    public abstract class AHintWidget : AHintElement
    {
        [SerializeField] protected TextMeshProUGUI _valueTMP;
        [Space]
        [SerializeField] protected FileIdAndKey _getText;
        [SerializeField] protected bool _extract;

        protected Subscription _subscription;

        protected void Init()
        {
            base.InternalInit(GameContainer.UI.CanvasHint);

            _subscription += Localization.Instance.Subscribe(SetLocalizationText);
        }

        protected virtual void OnDestroy()
        {
            _subscription?.Dispose();
        }

        protected abstract void SetLocalizationText(Localization localization);

#if UNITY_EDITOR
        public Vector2 Size => ((RectTransform)transform).rect.size;
        public void Init_Editor(ProjectColors colors)
        {
            _valueTMP.color = colors.PanelText;
        }

        protected virtual void OnValidate()
        {
            this.SetChildren(ref _valueTMP, "TextTMP");
        }
#endif
    }
}
