//Assets\Colonization\Scripts\UI\_UIGame\Panels\Abstract\ASinglyPanel.cs
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    public abstract class ASinglyPanel<TWidget> : MonoBehaviour where TWidget : AHintWidget
    {
        [SerializeField] protected TWidget _widget;

#if UNITY_EDITOR
        public virtual RectTransform UpdateVisuals_Editor(float pixelsPerUnit, Vector2 padding, ProjectColors colors)
        {
            Image image = GetComponent<Image>();
            image.color = colors.PanelBack;
            image.pixelsPerUnitMultiplier = pixelsPerUnit;

            RectTransform thisRectTransform = (RectTransform)transform;
            thisRectTransform.sizeDelta = _widget.Size + padding * 2f;

            _widget.Init_Editor(colors);
            return thisRectTransform;
        }

        protected virtual void OnValidate()
        {
            if (_widget == null)
                _widget = GetComponentInChildren<TWidget>();
        }
#endif
    }
}
