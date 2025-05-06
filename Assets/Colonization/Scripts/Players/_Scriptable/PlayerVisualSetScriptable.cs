//Assets\Colonization\Scripts\Players\_Scriptable\PlayerVisualSetScriptable.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    //[CreateAssetMenu(fileName = "PlayerVisualSet", menuName = "Vurbiri/Colonization/PlayerVisualSet", order = 51)]
    public class PlayerVisualSetScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private Color _colorSatan = Color.red;
        [Space]
        [SerializeField] private Color[] _colorHumans;
        [Space]
        [SerializeField] private Material _defaultMaterialLit;
        [SerializeField] private Material _defaultMaterialUnlit;
        [Space]
        [SerializeField] private Material _defaultMaterialActor;

        public int Count => _colorHumans.Length;
        public Color[] ColorHumans => _colorHumans;

        public PlayersVisual Get(int[] colorIds)
        {
            Color[] colors = new Color[PlayerId.Count];

            for (int i = 0; i < PlayerId.HumansCount; i++)
                colors[i] = _colorHumans[colorIds[i]];
            colors[PlayerId.Satan] = _colorSatan;

            return new(colors, _defaultMaterialLit, _defaultMaterialUnlit, _defaultMaterialActor);
        }

        public int[] GetIds(int playerColorId)
        {
            int length = _colorHumans.Length, step = length / (PlayerId.HumansCount - 1);
            int[] ids = new int[PlayerId.HumansCount];
            ids[0] = playerColorId;
            for (int i = 1; i < PlayerId.HumansCount; i++)
                ids[i] = (playerColorId + Random.Range(1, step)) % length;

            return ids;
        }

        public int[] GetIds() => GetIds(Random.Range(0, _colorHumans.Length));

    }
}


