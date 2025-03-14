//Assets\Colonization\Scripts\Actors\Fractions\Warriors\WarriorInitializer.cs
using UnityEngine;
using Vurbiri.Colonization.Characteristics;
using Vurbiri.Colonization.Data;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorInitializer : MonoBehaviour
    {
        [SerializeField] private Warrior _warrior;
        [SerializeField] private BoxCollider _collider;
        [Space]
        [SerializeField] private WarriorsSettingsScriptable _warriorsSettings;

        public Warrior Init(Id<WarriorId> id, Id<PlayerId> owner, IReactive<IPerk>[] buffs, Material material, Hexagon startHex)
        {
            _warrior.Init(_warriorsSettings[id], _collider, owner, buffs, startHex);
            return Setup(_warrior.Skin.Mesh, material);
        }

        public Warrior Load(ActorLoadData data, Id<PlayerId> owner, IReactive<IPerk>[] buffs, Material material, Hexagon startHex)
        {
            _warrior.Load(_warriorsSettings[data.id], _collider, owner, buffs, startHex, data);
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
                    _warriorsSettings = EUtility.FindAnyScriptable<WarriorsSettingsScriptable>();
        }
#endif
        }
}
