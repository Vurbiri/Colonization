using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    //[CreateAssetMenu(fileName = "Surfaces", menuName = "Vurbiri/Colonization/Surfaces Array", order = 51)]
    public class SurfacesScriptable : ScriptableObjectDisposable
    {
        [SerializeField] private IdArray<SurfaceId, SurfaceType> _surfaces = new();

        public SurfaceType this[int index] => _surfaces[index];
        public SurfaceType this[Id<SurfaceId> id] => _surfaces[id];


#if UNITY_EDITOR
        public void Set()
        {
            for (int i = 0; i < _surfaces.Count; i++)
                _surfaces[i].Set(i);
        }
#endif
    }
}
