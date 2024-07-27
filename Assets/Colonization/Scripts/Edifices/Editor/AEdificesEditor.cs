using UnityEditor;
using UnityEngine;

namespace Vurbiri.Colonization
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
            EdificeType type = _edifice.Type;
            EdificeGroup group = type.ToGroup();
            GUIStyle style = new(EditorStyles.boldLabel);

            if (!Application.isPlaying)
            {
                serializedObject.Update();

                _groupProperty.enumValueIndex = (int)group;
                _isBuildWallProperty.boolValue = group == EdificeGroup.Urban;

                AEdifice next = _prefabUpgradeProperty.objectReferenceValue as AEdifice;
                if (_isUpgradeProperty.boolValue = next != null)
                {
                    _typeNextProperty.enumValueIndex = (int)next.Type;
                    _groupNextProperty.enumValueIndex = (int)next.Group;
                }
                else
                {
                    _typeNextProperty.enumValueIndex = 0;
                    _groupNextProperty.enumValueIndex = 0;
                }
                serializedObject.ApplyModifiedProperties();
            }

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
