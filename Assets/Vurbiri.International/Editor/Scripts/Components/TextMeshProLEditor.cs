using TMPro.EditorUtilities;
using UnityEditor;
using Vurbiri.International.UI;

namespace VurbiriEditor.International
{
	[CustomEditor(typeof(TextMeshProL), true), CanEditMultipleObjects]
	public class TextMeshProLEditor : TMP_EditorPanel
    {
        private TextMeshLocalizationDraw _localization;

        protected override void OnEnable()
        {
            base.OnEnable();
            _localization = new(serializedObject.FindProperty(TextMeshProL.getTextField), serializedObject.FindProperty(TextMeshProL.extractField));
        }

        public override void OnInspectorGUI()
        {
            if (IsMixSelectionTypes()) return;

            _localization.Draw(serializedObject);

            base.OnInspectorGUI();
        }
    }
}