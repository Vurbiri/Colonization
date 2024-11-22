//Assets\Colonization\Scripts\Island\Road\RoadsFactory.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class RoadsFactory
    {
        public Roads prefab;
        public Transform container;

        public Roads Create() => Object.Instantiate(prefab, container.transform, false);
    }
}
