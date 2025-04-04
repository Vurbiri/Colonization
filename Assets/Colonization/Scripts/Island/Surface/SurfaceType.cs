//Assets\Colonization\Scripts\Island\Surface\SurfaceType.cs
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable]
	public class SurfaceType
	{
        [HideInInspector, SerializeField] private Id<SurfaceId> _id;
        [Space]
        [SerializeField] private IdFlags<CurrencyId> _profits;
        [SerializeField] private ASurface _prefabSurface;

        private IProfit _profit;

        public Id<SurfaceId> Id => _id;
        public bool IsWater => _id == SurfaceId.Water;
        public bool IsGate => _id == SurfaceId.Gate;
        public IdFlags<CurrencyId> Currencies => _profits;
        public IProfit Profit => _profit ??= _id != SurfaceId.Water ? new ProfitSingle(_profits.First()) : new ProfitArray(_profits.GetValues());

        public void Create(Transform parent)
        {
            if (_prefabSurface != null)
                UnityEngine.Object.Instantiate(_prefabSurface, parent).Init(_id == SurfaceId.Mountain);
        }

#if UNITY_EDITOR
        public void Set(int id)
        {
            _id = id;

            _prefabSurface = EUtility.FindAnyPrefab<ASurface>($"P_{IdType<SurfaceId>.Names[id]}");

            if (id < SurfaceId.Water)
                _profits = id;
            else if (id == SurfaceId.Gate)
                _profits = CurrencyId.Blood;
            else if (id == SurfaceId.Water)
                _profits = new( 0, 1, 2, 3, 4 );
        }
#endif
    }
}
