using UnityEngine;

namespace Vurbiri
{
    [System.Serializable]
    public struct SceneId
    {
        [SerializeField] private int _scene;

        public SceneId(int scene)
        {
            _scene = scene;
        }

        public static implicit operator SceneId(int scene) => new(scene);
        public static implicit operator int(SceneId scene) => scene._scene;
    }
}
