using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{

    [CustomEditor(typeof(AEdifice), true), CanEditMultipleObjects]
    public class AEdificesEditor : Editor
    {
        protected SerializedProperty _groupIdProperty;
        protected SerializedProperty _isUpgradeProperty;
        protected SerializedProperty _isBuildWallProperty;
        protected SerializedProperty _prefabUpgradeProperty;
        protected SerializedProperty _nextIdProperty;
        protected SerializedProperty _nextGroupIdProperty;
        protected AEdifice _edifice;

        protected virtual void OnEnable()
        {
            _groupIdProperty = serializedObject.FindProperty("_groupId");
            _isUpgradeProperty = serializedObject.FindProperty("_isUpgrade");
            _isBuildWallProperty = serializedObject.FindProperty("_isBuildWall");
            _prefabUpgradeProperty = serializedObject.FindProperty("_prefabUpgrade");
            _nextIdProperty = serializedObject.FindProperty("_nextId");
            _nextGroupIdProperty = serializedObject.FindProperty("_nextGroupId");

            _edifice = (AEdifice)target;
        }

        public override void OnInspectorGUI()
        {
            GUIStyle style = new(EditorStyles.boldLabel);

            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.TextField(_groupIdProperty.displayName, EdificeGroupId.Names[_groupIdProperty.intValue], style);
            EditorGUILayout.TextField(_isUpgradeProperty.displayName, _isUpgradeProperty.boolValue.ToString(), style);
            EditorGUILayout.TextField(_isBuildWallProperty.displayName, _isBuildWallProperty.boolValue.ToString(), style);
            EditorGUILayout.Space();
            EditorGUILayout.TextField(_nextIdProperty.displayName, (EdificeId.Names[_nextIdProperty.intValue]).ToString(), style);
            EditorGUILayout.TextField(_nextGroupIdProperty.displayName, (EdificeGroupId.Names[_nextGroupIdProperty.intValue]).ToString(), style);
        }
    }
}
