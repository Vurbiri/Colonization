using System;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerVisualSet", menuName = "Colonization/PlayerVisualSet", order = 51)]
public class PlayerVisualSetScriptable : ScriptableObject
{
    [SerializeField] private Color[] _colors;
    [Space]
    [SerializeField] private Material _defaultMaterial;

    public int Count => _colors.Length;
    public Color[] Colors => _colors;

    public PlayerVisual Get(int id) => new(id, _colors[id], new(_defaultMaterial));

    public int[] GetIds(int count, int start = 0)
    {
        int length = _colors.Length;
        int[] ids = new int[count];
        for (int i = 0; i < count; i++)
            ids[i] = (start + i) % length;

        return ids;
    }
    
    public int[] RandIds(int count)
    {
        int length = _colors.Length;
        int[] ids = new int[length];
        for (int i = 1, j; i < length; i++)
        {
            ids[i] = i;
            j = UnityEngine.Random.Range(0, i);
            (ids[j], ids[i]) = (ids[i], ids[j]);
        }

        Array.Resize(ref ids, count);
        return ids;
    }
}


