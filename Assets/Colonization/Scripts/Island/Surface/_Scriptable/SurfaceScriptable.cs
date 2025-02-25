//Assets\Colonization\Scripts\Island\Surface\_Scriptable\SurfaceScriptable.cs
using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "Surface_", menuName = "Vurbiri/Colonization/Surface", order = 51)]
    public class SurfaceScriptable : ScriptableObjectDisposable, IValueId<SurfaceId>
    {
        [SerializeField] private Id<SurfaceId> _id;
        [SerializeField] private Color32 _color;
        [Space]
        [SerializeField] private Id<CurrencyId>[] _profits;
        [Space]
        [SerializeField] private ASurface _prefabSurface;

        private IProfit _profit;

        public Id<SurfaceId> Id => _id;
        public Color32 Color => _color;
        public bool IsWater => _id == SurfaceId.Water;
        public bool IsGate => _id == SurfaceId.Gate;
        public IReadOnlyList<Id<CurrencyId>> Currencies => _profits;
        public IProfit Profit => _profit ??= _profits.Length == 1 ? new ProfitSingle(_profits[0]) : new ProfitArray(_profits);

        public void Create(Transform parent) 
        {
            if (_prefabSurface != null)
                Instantiate(_prefabSurface, parent).Init(_id == SurfaceId.Mountain);
        }
    }
}
