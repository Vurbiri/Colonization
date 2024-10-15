using System.Collections.Generic;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [CreateAssetMenu(fileName = "Surfaces", menuName = "Vurbiri/Colonization/Surfaces Array", order = 51)]
    public class SurfacesScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private IdHashSet<SurfaceId, SurfaceScriptable> _surfaces;

        public SurfaceScriptable this[int index] => _surfaces[index];
        public SurfaceScriptable this[Id<SurfaceId> id] => _surfaces[id];

        public List<SurfaceScriptable> GetRange(int start, int end) => _surfaces.GetRange(start, end);

        public override void Dispose()
        {
            foreach (var surface in _surfaces)
                surface.Dispose();
            _surfaces = null;

            Resources.UnloadAsset(this);
        }
    }
}
