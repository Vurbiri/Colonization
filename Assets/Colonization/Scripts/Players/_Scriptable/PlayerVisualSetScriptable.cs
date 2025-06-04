using UnityEngine;
using Vurbiri.Colonization.Storage;

namespace Vurbiri.Colonization
{
    //[CreateAssetMenu(fileName = "PlayerVisualSet", menuName = "Vurbiri/Colonization/PlayerVisualSet", order = 51)]
    public class PlayerVisualSetScriptable : ScriptableObject
    {
        [SerializeField] private PlayerColors _colors;
        [Space]
        [SerializeField] private Material _defaultMaterialLit;
        [SerializeField] private Material _defaultMaterialUnlit;
        [Space]
        [SerializeField] private Material _defaultMaterialWarrior;
        [Space]
        [SerializeField] private PlayerNames _nameKeys;

        public void Init(ProjectStorage storage, DIContainer container)
        {
            storage.SetAndBindPlayerColors(_colors);

            container.AddInstance(_colors);
            container.AddInstance(new HumansMaterials(_colors, _defaultMaterialLit, _defaultMaterialUnlit, _defaultMaterialWarrior));
            container.AddInstance(_nameKeys.Init(storage, container.Get<YandexSDK>()));

            Resources.UnloadAsset(this);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _colors.OnValidate();

            EUtility.SetAsset(ref _defaultMaterialLit, "M_BasePlayersLit");
            EUtility.SetAsset(ref _defaultMaterialUnlit, "M_BasePlayersUnlit");
            EUtility.SetAsset(ref _defaultMaterialWarrior, "M_Warrior_Base");

            _nameKeys.OnValidate();
        }
#endif
    }
}


