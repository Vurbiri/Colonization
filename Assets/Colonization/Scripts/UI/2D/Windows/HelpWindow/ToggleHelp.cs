using UnityEngine;
using Vurbiri.UI;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    sealed public class ToggleHelp : VToggleBase<ToggleHelp>
    {
        [SerializeField] private RectTransform _targetHelp;

        public RectTransform TargetTransform { [Impl(256)] get => _targetHelp; }

        protected override void UpdateVisual()
        {
#if UNITY_EDITOR
            if (_targetHelp == null) return;
#endif
            _targetHelp.gameObject.SetActive(_isOn);
            StartColorTween(0, Vector3.one, colors.normalColor, 0f);
        }

        protected override void UpdateVisualInstant() => UpdateVisual();

        protected override void StartColorTween(int intState, Vector3 targetScale, Color targetColor, float duration)
        {
            if (_isOn) targetColor = targetColor.Brightness(0.80f);
            base.StartColorTween(intState, targetScale, targetColor, duration);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode || _group != null || !isActiveAndEnabled || UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
                return;

            var so = new UnityEditor.SerializedObject(this);
            so.FindProperty(nameof(_group)).objectReferenceValue = GetComponentInParent<HelpWindow>();
            so.ApplyModifiedProperties();
        }
#endif
    }
}
