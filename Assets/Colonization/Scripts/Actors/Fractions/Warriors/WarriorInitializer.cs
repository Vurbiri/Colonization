//Assets\Colonization\Scripts\Actors\Fractions\Warriors\WarriorInitializer.cs
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

        public Warrior Init(int id, Id<PlayerId> owner, Material material, Hexagon startHex)
        {
            _warrior.Init(_warriorsSettings[id], _collider, owner, startHex);
            return Setup(_warrior.Skin.Mesh, material);
        }

        public Warrior Load(ActorLoadData data, Id<PlayerId> owner, Material material, Hexagon startHex)
        {
            _warrior.Load(_warriorsSettings[data.id], _collider, owner, startHex, data);
            return Setup(_warrior.Skin.Mesh, material);
        }

        private Warrior Setup(SkinnedMeshRenderer mesh, Material material)
        {
            mesh.sharedMaterial = material;

            _collider.enabled = false;

            Destroy(this);

            return _warrior;
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
