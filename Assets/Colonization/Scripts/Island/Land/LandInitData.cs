//Assets\Colonization\Scripts\Island\Land\LandInitData.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class LandInitData
	{
        public Hexagon prefabHex;
        public HexagonMarkFactory prefabHexMark;
        public SurfacesScriptable surfaces;
        [Space]
        public LandMesh landMesh;

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetPrefab(ref prefabHex);
            EUtility.SetPrefab(ref prefabHexMark);
            EUtility.SetScriptable(ref surfaces);
            EUtility.SetObject(ref landMesh);
        }
#endif
    }
}
