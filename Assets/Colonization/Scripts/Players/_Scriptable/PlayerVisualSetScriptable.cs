//Assets\Colonization\Scripts\Players\_Scriptable\PlayerVisualSetScriptable.cs
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
        [SerializeField] private Material _defaultMaterialActor;

        public PlayerColors Colors => _colors;

        public void Init(ProjectStorage storage, DIContainer container)
        {
            _colors.Init(storage, container);

            Resources.UnloadAsset(this);
        }
    }
}


