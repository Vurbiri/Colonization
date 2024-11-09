using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class WarriorInitializer : MonoBehaviour
    {
        [SerializeField] private Warrior _warrior;
        [Space]
        [SerializeField] private WarriorsSettingsScriptable _warriorsSettings;

        public Warrior Init(int id, int owner, Material material, Hexagon startHex, GameplayEventBus eventBus)
        {
            WarriorSettings settings = _warriorsSettings[id];

            _warrior.Init(settings, owner, startHex, eventBus);
            GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial = material;

            Destroy(this);

            return _warrior;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_warrior == null)
                _warrior = GetComponent<Warrior>();
            if (_warriorsSettings == null)
                _warriorsSettings = VurbiriEditor.Utility.FindAnyScriptable<WarriorsSettingsScriptable>();
        }
#endif
    }
}
