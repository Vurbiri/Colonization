using UnityEngine;
using Vurbiri.Colonization.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
	public abstract class ASwitchableWindow : MonoBehaviour
	{
        [SerializeField] protected Switcher _switcher;

        public Switcher Switcher { [Impl(256)] get => _switcher; }

        public abstract Switcher Init();

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            if(_switcher == null)
                _switcher = new(this);
            else
                _switcher.OnValidate(this);
        }

        public virtual void UpdateVisuals_Ed(float pixelsPerUnit, ProjectColors colors)
        {
            var color = colors.PanelBack.SetAlpha(1f);
            var mainImage = GetComponent<UnityEngine.UI.Image>();

            mainImage.color = color;
            mainImage.pixelsPerUnitMultiplier = pixelsPerUnit;
        }
#endif
    }
}
