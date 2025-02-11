//Assets\Colonization\Scripts\Actors\Warriors\WarriorInitializer.cs
using UnityEngine;
using Vurbiri.Colonization.Data;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorInitializer : MonoBehaviour
    {
        [SerializeField] private Warrior _warrior;
        [SerializeField] private BoxCollider _collider;
        [Space]
        [SerializeField] private WarriorsSettingsScriptable _warriorsSettings;

        public Warrior Init(int id, int owner, Material material, Hexagon startHex)
        {
            _warrior.Init(_warriorsSettings[id], _collider, owner, startHex);
            Setup(_warrior.Skin.Mesh, material);

            Destroy(this);

            return _warrior;
        }

        public Warrior Init(ActorLoadData data, int owner, Material material, Hexagon startHex)
        {
            _warrior.Init(_warriorsSettings[data.id], _collider, owner, startHex, data);
            Setup(_warrior.Skin.Mesh, material);

            Destroy(this);

            return _warrior;
        }

        private void Setup(SkinnedMeshRenderer mesh, Material material)
        {
            mesh.sharedMaterial = material;

            _collider.enabled = false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_warrior == null)
                _warrior = GetComponent<Warrior>();
            if (_collider == null)
                _collider = GetComponent<BoxCollider>();
            if (_warriorsSettings == null)
                    _warriorsSettings = VurbiriEditor.Utility.FindAnyScriptable<WarriorsSettingsScriptable>();
        }
#endif
        }
}
