using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "Surface_", menuName = "Vurbiri/Colonization/Surface", order = 51)]
    public class SurfaceScriptable : ScriptableObject, IValueTypeEnum<SurfaceType>, ISerializationCallbackReceiver
    {
        [SerializeField] private SurfaceType _type;
        [SerializeField] private Color32 _color;
        [Space]
        [SerializeField] private CurrencyType[] _profits;
        [SerializeField, Hide] private CurrencyType _profit;
        [Space]
        [SerializeField] private ASurface _prefabSurface;


        public SurfaceType Type => _type;
        public Color32 Color => _color;
        public bool IsWater => _type == SurfaceType.Water;
        public bool IsGate => _type == SurfaceType.Gate;
        public ASurface Prefab => _prefabSurface;

        public CurrencyType GetCurrency() => _type != SurfaceType.Water ? _profit : _profits.Rand();

        public void OnBeforeSerialize()
        {
            if(_profits.Length > 0 && !IsWater)
                _profit = _profits[0];
        }

        public void OnAfterDeserialize() { }
    }
}
