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

            _switcher?.OnValidate(this);
        }

        //public virtual void UpdateVisuals_Ed(float pixelsPerUnit, ProjectColors project, SceneColorsEd scene)
        //{
        //    GetComponent<UnityEngine.UI.Image>().SetImageFields(scene.panelBack, pixelsPerUnit);
        //}
#endif
    }
}
