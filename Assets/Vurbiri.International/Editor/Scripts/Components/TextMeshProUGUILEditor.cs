using TMPro.EditorUtilities;
using UnityEditor;
using Vurbiri.International.UI;

namespace Vurbiri.International.Editor
{
	[CustomEditor(typeof(TextMeshProUGUIL), true), CanEditMultipleObjects]
	public class TextMeshProUGUILEditor : TMP_EditorPanelUI
    {
        private TextMeshLocalizationDraw _localization;

        protected override void OnEnable()
		{
            base.OnEnable();
            _localization = new(serializedObject.FindProperty(TextMeshProUGUIL.getTextField), serializedObject.FindProperty(TextMeshProUGUIL.extractField));
        }

        public override void OnInspectorGUI()
        {
            if (IsMixSelectionTypes()) return;

            _localization.Draw(serializedObject);

            base.OnInspectorGUI();
        }
    }
}