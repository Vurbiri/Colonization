using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{

    [CustomEditor(typeof(AEdifice), true), CanEditMultipleObjects]
    public class AEdificesEditor : Editor
    {
        protected SerializedProperty _groupProperty;
        protected SerializedProperty _isUpgradeProperty;
        protected SerializedProperty _isBuildWallProperty;
        protected SerializedProperty _prefabUpgradeProperty;
        protected SerializedProperty _typeNextProperty;
        protected SerializedProperty _groupNextProperty;
        protected AEdifice _edifice;

        protected virtual void OnEnable()
        {
            _groupProperty = serializedObject.FindProperty("_group");
            _isUpgradeProperty = serializedObject.FindProperty("_isUpgrade");
            _isBuildWallProperty = serializedObject.FindProperty("_isBuildWall");
            _prefabUpgradeProperty = serializedObject.FindProperty("_prefabUpgrade");
            _typeNextProperty = serializedObject.FindProperty("_typeNext");
            _groupNextProperty = serializedObject.FindProperty("_groupNext");

            _edifice = (AEdifice)target;
        }

        public override void OnInspectorGUI()
        {
            EdificeGroup group = _edifice.Type.ToGroup();
            GUIStyle style = new(EditorStyles.boldLabel);

            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.TextField(_groupProperty.displayName, group.ToString(), style);
            EditorGUILayout.TextField(_isUpgradeProperty.displayName, _isUpgradeProperty.boolValue.ToString(), style);
            EditorGUILayout.TextField(_isBuildWallProperty.displayName, _isBuildWallProperty.boolValue.ToString(), style);
            EditorGUILayout.Space();
            EditorGUILayout.TextField(_typeNextProperty.displayName, ((EdificeType)_typeNextProperty.enumValueIndex).ToString(), style);
            EditorGUILayout.TextField(_groupNextProperty.displayName, ((EdificeGroup)_groupNextProperty.enumValueIndex).ToString(), style);

            PlayerType player = _edifice.Owner;
            if (Application.isPlaying && player != PlayerType.None && Players.Instance != null)
            {
                Color prevColor = GUI.color;
                GUI.color = Players.Instance[player].Color;
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                EditorGUILayout.TextField("Owner", _edifice.Owner.ToString(), style);
                GUI.color = prevColor;
            }
        }
    }
}
