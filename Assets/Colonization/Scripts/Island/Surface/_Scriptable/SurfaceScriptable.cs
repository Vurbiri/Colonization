using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "Surface_", menuName = "Vurbiri/Colonization/Surface", order = 51)]
    public class SurfaceScriptable : ScriptableObject, IValueId<SurfaceType>
    {
        [SerializeField] private Id<SurfaceType> _id;
        [SerializeField] private Color32 _color;
        [Space]
        [SerializeField] private bool _isProfit = true;
        [SerializeField] private Id<CurrencyId>[] _profits;
        [Space]
        [SerializeField] private ASurface _prefabSurface;

        public Id<SurfaceType> Id => _id;
        public Color32 Color => _color;
        public bool IsWater => _id == SurfaceType.Water;
        public bool IsGate => _id == SurfaceType.Gate;
        public bool IsProfit => _isProfit;

        public Id<CurrencyId> GetCurrency() => _profits.Rand();

        public void Create(Transform parent) 
        {
            if (_prefabSurface != null)
                Instantiate(_prefabSurface, parent).Initialize();
        }
    }
}
