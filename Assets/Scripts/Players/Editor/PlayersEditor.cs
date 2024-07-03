using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Players), true)]
public class PlayersEditor : Editor
{
    //private readonly bool[] _opens = new bool[4];
    private readonly string[] _names = Enum<Resource>.GetNames();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (!Application.isPlaying)
            return;

        Players players = (Players)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        bool open;
        Color prev = GUI.color;
        foreach(Player player in players)
        {
            if (open = players.Current == player)
                GUI.color = player.Color;

            if (EditorGUILayout.Foldout(open, player.Type.ToString()))
            {
              
                GUI.color = prev;
                EditorGUI.indentLevel++; 
                int j = 0;
                foreach (int value in player.Resources)
                    EditorGUILayout.IntField(_names[j++], value);
                EditorGUI.indentLevel--;
                GUI.color = player.Color;
                EditorGUILayout.LabelField($"Amount: {player.Resources.Amount}");

                Repaint();
            }

            GUI.color = prev;
        }
    }
}
