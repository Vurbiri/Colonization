using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[CustomEditor(typeof(City), true), CanEditMultipleObjects]
public class CityEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        City city = (City)target;
        GUIStyle style = new(EditorStyles.boldLabel);

        EditorGUILayout.Space();
        EditorGUILayout.TextField("Group", city.Type.ToGroup().ToString(), style);

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
