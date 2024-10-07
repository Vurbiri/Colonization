using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "Surface_", menuName = "Vurbiri/Colonization/Surface", order = 51)]
    public class SurfaceScriptable : ScriptableObject, IValueId<SurfaceType>
    {
        [SerializeField] private Id<SurfaceType> _id;
        [SerializeField] private Color32 _color;
        [Space]
        [SerializeField] private Id<CurrencyId>[] _profits;
        [Space]
        [SerializeField] private ASurface _prefabSurface;

        public Id<SurfaceType> Id => _id;
        public Color32 Color => _color;
        public bool IsWater => _id == SurfaceType.Water;
        public bool IsGate => _id == SurfaceType.Gate;
        public IReadOnlyList<Id<CurrencyId>> Currencies => _profits;

        public int GetCurrency()
        {
            if(_profits.Length == 1)
                return _profits[0].ToInt;

            return _profits.Rand().ToInt;
        }

        public void Create(Transform parent) 
        {
            if (_prefabSurface != null)
                Instantiate(_prefabSurface, parent).Initialize();
        }
    }
}
