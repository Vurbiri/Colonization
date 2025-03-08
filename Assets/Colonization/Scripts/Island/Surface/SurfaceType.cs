//Assets\Colonization\Scripts\Island\Surface\SurfaceType.cs
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [Serializable]
	public class SurfaceType
	{
        [HideInInspector, SerializeField] private Id<SurfaceId> _id;
        [Space]
        [SerializeField] private Id<CurrencyId>[] _profits;
        [Space]
        [SerializeField] private ASurface _prefabSurface;

        private IProfit _profit;

        public Id<SurfaceId> Id => _id;
        public bool IsWater => _id == SurfaceId.Water;
        public bool IsGate => _id == SurfaceId.Gate;
        public IReadOnlyList<Id<CurrencyId>> Currencies => _profits;
        public IProfit Profit => _profit ??= _profits.Length == 1 ? new ProfitSingle(_profits[0]) : new ProfitArray(_profits);

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
                _profits = new Id<CurrencyId>[] { id };
            else if (id == SurfaceId.Gate)
                _profits = new Id<CurrencyId>[] { CurrencyId.Blood };
            else if (id == SurfaceId.Water)
                _profits = new Id<CurrencyId>[] { 0, 1, 2, 3, 4 };
        }
#endif
    }
}
