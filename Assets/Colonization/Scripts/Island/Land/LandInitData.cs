//Assets\Colonization\Scripts\Island\Land\LandInitData.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class LandInitData
	{
        public Hexagon prefabHex;
        public HexagonMark prefabHexMark;
        public SurfacesScriptable surfaces;
        [Space]
        public LandMesh landMesh;

#if UNITY_EDITOR
        public void OnValidate()
        {
            
            if (prefabHex == null)
                prefabHex = VurbiriEditor.Utility.FindAnyPrefab<Hexagon>();
            if (prefabHexMark == null)
                prefabHexMark = VurbiriEditor.Utility.FindAnyPrefab<HexagonMark>();
            if (surfaces == null)
                surfaces = VurbiriEditor.Utility.FindAnyScriptable<SurfacesScriptable>();
            else
                surfaces.OnValidate();
            if (landMesh == null)
                landMesh = Object.FindAnyObjectByType<LandMesh>();
        }
#endif
    }
}
