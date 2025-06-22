using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri.Colonization.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    sealed public class RaycastTargetObject : Graphic
    {
        protected override void Start()
        {
            base.Start();
            canvasRenderer.SetAlpha(0f);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            color = new(0f, 0f, 0f, 0f);
            raycastTarget = true;
        }

        [UnityEditor.CustomEditor(typeof(RaycastTargetObject))]
        private class RaycastTargetObjectEditor : UnityEditor.Editor 
        {
            public override void OnInspectorGUI() { }
        }
#endif
    }
}
