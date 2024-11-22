//Assets\Vurbiri\Runtime\Types\SceneId.cs
using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public struct SceneId
    {
        [SerializeField] private int _scene;

        public static implicit operator int(SceneId scene) => scene._scene;
    }
}
