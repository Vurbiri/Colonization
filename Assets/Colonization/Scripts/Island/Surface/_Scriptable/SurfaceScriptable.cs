using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "Surface_", menuName = "Vurbiri/Colonization/Surface", order = 51)]
    public class SurfaceScriptable : ScriptableObject, IValueTypeEnum<SurfaceType>
    {
        [SerializeField] private SurfaceType _type;
        [SerializeField] private Color32 _color;
        [Space]
        [SerializeField] private CurrencyType[] _profits;
        [Space]
        [SerializeField] private ASurface _prefabSurface;


        public SurfaceType Type => _type;
        public Color32 Color => _color;
        public bool IsWater => _type == SurfaceType.Water;
        public bool IsGate => _type == SurfaceType.Gate;

        public CurrencyType GetCurrency() => _profits.Rand();

        public void Create(Transform parent) 
        {
            if (_prefabSurface != null)
                Instantiate(_prefabSurface, parent).Initialize();
        }
    }
}
