//Assets\Colonization\Scripts\Island\Hexagon\HexagonMark\HexagonMarkFactory.cs
using System;
using UnityEngine;

namespace Vurbiri.Colonization
{
    [RequireComponent(typeof(MeshRenderer))]
    public class HexagonMarkFactory : MonoBehaviour
	{
        public Material greenMaterial;
        public Material redMaterial;

        public HexagonMark Create(Transform repository, Action<HexagonMark, bool> callback) => new(Instantiate(this, repository), callback);

#if UNITY_EDITOR
        public void OnValidate()
        {
            EUtility.SetAsset(ref greenMaterial, "M_HexagonMarkGreen");
            EUtility.SetAsset(ref redMaterial, "M_HexagonMarkRed");
        }
#endif
    }
}
