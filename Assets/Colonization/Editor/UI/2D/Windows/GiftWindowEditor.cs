using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization.UI;

namespace VurbiriEditor.Colonization.UI
{
    [CustomEditor(typeof(GiftWindow)), CanEditMultipleObjects]
    public class GiftWindowEditor : Editor
    {
        private GiftWindow _giftWindow;

        private void OnEnable()
        {
            _giftWindow = (GiftWindow)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            if (GUILayout.Button("Setup"))
                _giftWindow.Setup_Editor();
            EditorGUILayout.Space();
            if (GUILayout.Button("Create"))
                _giftWindow.Create_Editor();
            if (GUILayout.Button("Delete"))
                _giftWindow.Delete_Editor();
        }
    }
}
