using UnityEngine;

[CreateAssetMenu(fileName = "ColorsSet", menuName = "Colonization/ColorsSet", order = 51)]
public class ColorsScriptable : ScriptableObject
{
    [SerializeField] private Color[] _colors;

    public int Count => _colors.Length;
    public Color this[int index] => _colors[index];
}
