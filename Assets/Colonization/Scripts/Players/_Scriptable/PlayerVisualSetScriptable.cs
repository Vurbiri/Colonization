using UnityEngine;
using Vurbiri.Colonization.EntryPoint;

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

        public void Init(ProjectContent content)
        {
            content.projectStorage.SetAndBindPlayerColors(_colors);

            content.playerColors = _colors;
            content.playerNames = _nameKeys.Init(content.projectStorage);
            content.playerUINames = new(_colors, _nameKeys);
            content.humansMaterials = new(_colors, _defaultMaterialLit, _defaultMaterialUnlit, _defaultMaterialWarrior);

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


