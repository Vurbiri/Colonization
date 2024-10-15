using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "Surface_", menuName = "Vurbiri/Colonization/Surface", order = 51)]
    public class SurfaceScriptable : ScriptableObject, IValueId<SurfaceId>, System.IDisposable
    {
        [SerializeField] private Id<SurfaceId> _id;
        [SerializeField] private Color32 _color;
        [Space]
        [SerializeField] private Id<CurrencyId>[] _profits;
        [Space]
        [SerializeField] private ASurface _prefabSurface;

        public Id<SurfaceId> Id => _id;
        public Color32 Color => _color;
        public bool IsWater => _id == SurfaceId.Water;
        public bool IsGate => _id == SurfaceId.Gate;
        public IReadOnlyList<Id<CurrencyId>> Currencies => _profits;

        public void Create(Transform parent) 
        {
            if (_prefabSurface != null)
                Instantiate(_prefabSurface, parent).Init();
        }

        public void Dispose()
        {
            Resources.UnloadAsset(this);
        }
    }
}
