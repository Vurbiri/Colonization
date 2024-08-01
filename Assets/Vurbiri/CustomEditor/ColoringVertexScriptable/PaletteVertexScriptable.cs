using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri
{
    [CreateAssetMenu(fileName = "Palette_", menuName = "Vurbiri/Editor/Palette Vertex", order = 51)]
    public class PaletteVertexScriptable : ScriptableObject
    {
        [SerializeField] private List<Color> colors;

        public Color this[int index] { get => colors[index]; set => colors[index] = value; }
        public int Count => colors.Count;

        public void Initialize(string name, int count)
        {
            this.name = name;
            colors = new(count);
        }

        public void Add(Color color) => colors.Add(color);
    }
}
