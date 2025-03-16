//Assets\Colonization\Scripts\Players\_Scriptable\PlayerVisualSetScriptable.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    //[CreateAssetMenu(fileName = "PlayerVisualSet", menuName = "Vurbiri/Colonization/PlayerVisualSet", order = 51)]
    public class PlayerVisualSetScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private Color _colorDemons = Color.red;
        [Space]
        [SerializeField] private Color[] _colors;
        [Space]
        [SerializeField] private Material _defaultMaterialLit;
        [SerializeField] private Material _defaultMaterialUnlit;
        [Space]
        [SerializeField] private Material _defaultMaterialActor;

        public int Count => _colors.Length;
        public Color[] Colors => _colors;

        public PlayersVisual Get(IReadOnlyList<int> ids)
        {
            int count = ids.Count;

            Color[] colors = new Color[count + 1];
            int i = 0;
            for (; i < count; i++)
                colors[i] = _colors[ids[i]];
            colors[i] = _colorDemons;

            return new(colors, _defaultMaterialLit, _defaultMaterialUnlit, _defaultMaterialActor);
        }

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
}


