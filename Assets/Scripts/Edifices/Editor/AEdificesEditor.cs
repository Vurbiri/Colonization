using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(AEdifice), true), CanEditMultipleObjects]
public class AEdificesEditor : Editor
{
    protected SerializedProperty _isUpgradeProperty;
    protected SerializedProperty _prefabUpgradeProperty;

    protected virtual void OnEnable()
    {
        _isUpgradeProperty = serializedObject.FindProperty("_isUpgrade");
        _prefabUpgradeProperty = serializedObject.FindProperty("_prefabUpgrade");
    }

    public override void OnInspectorGUI()
    {
        AEdifice city = (AEdifice)target;
        EdificeType type = city.Type;
        GUIStyle style = new(EditorStyles.boldLabel);

        if (!Application.isPlaying)
        {
            serializedObject.Update();
            _isUpgradeProperty.boolValue = _prefabUpgradeProperty.objectReferenceValue != null;
            serializedObject.ApplyModifiedProperties();
        }

        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.TextField("Group", type.ToGroup().ToString(), style);

        PlayerType player = city.Owner;
        if (Application.isPlaying && player != PlayerType.None && Players.Instance != null)
        {
            Color prevColor = GUI.color;
            GUI.color = Players.Instance[player].Color;
            EditorGUILayout.TextField("Owner", city.Owner.ToString(), style);
            GUI.color = prevColor;
        }
    }
}
