using UnityEngine;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorInitializer : MonoBehaviour
    {
        [SerializeField] private Warrior _warrior;
        [SerializeField] private BoxCollider _collider;
        [Space]
        [SerializeField] private WarriorsSettingsScriptable _warriorsSettings;

        public Warrior Init(Id<WarriorId> id, ActorInitData initData, Material material, Hexagon startHex)
        {
            _warrior.Init(_warriorsSettings[id], initData, _collider, startHex);
            return Setup(_warrior.Skin.Mesh, material);
        }

        public Warrior Load(ActorLoadData data, ActorInitData initData, Material material, Hexagon startHex)
        {
            _warrior.Load(_warriorsSettings[data.state.id], initData, _collider, startHex, data);
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

            EUtility.SetScriptable(ref _warriorsSettings);
        }
#endif
        }
}
