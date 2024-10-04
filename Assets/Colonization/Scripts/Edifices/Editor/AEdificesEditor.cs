using UnityEditor;
using UnityEngine;
using Vurbiri.Colonization;

namespace VurbiriEditor.Colonization
{

    [CustomEditor(typeof(AEdifice), true), CanEditMultipleObjects]
    public class AEdificesEditor : Editor
    {
        protected SerializedProperty _idGroupProperty;
        protected SerializedProperty _isUpgradeProperty;
        protected SerializedProperty _isBuildWallProperty;
        protected SerializedProperty _prefabUpgradeProperty;
        protected SerializedProperty _idNextProperty;
        protected SerializedProperty _idGroupNextProperty;
        protected AEdifice _edifice;

        protected virtual void OnEnable()
        {
            _idGroupProperty = serializedObject.FindProperty("_idGroup");
            _isUpgradeProperty = serializedObject.FindProperty("_isUpgrade");
            _isBuildWallProperty = serializedObject.FindProperty("_isBuildWall");
            _prefabUpgradeProperty = serializedObject.FindProperty("_prefabUpgrade");
            _idNextProperty = serializedObject.FindProperty("_idNext");
            _idGroupNextProperty = serializedObject.FindProperty("_idGroupNext");

            _edifice = (AEdifice)target;
        }

        public override void OnInspectorGUI()
        {
            GUIStyle style = new(EditorStyles.boldLabel);

            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            EditorGUILayout.TextField(_idGroupProperty.displayName, IdEdificeGroup.Names[_idGroupProperty.intValue], style);
            EditorGUILayout.TextField(_isUpgradeProperty.displayName, _isUpgradeProperty.boolValue.ToString(), style);
            EditorGUILayout.TextField(_isBuildWallProperty.displayName, _isBuildWallProperty.boolValue.ToString(), style);
            EditorGUILayout.Space();
            EditorGUILayout.TextField(_idNextProperty.displayName, (IdEdifice.Names[_idNextProperty.intValue]).ToString(), style);
            EditorGUILayout.TextField(_idGroupNextProperty.displayName, (IdEdificeGroup.Names[_idGroupNextProperty.intValue]).ToString(), style);

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
