using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.UI
{
	//[CreateAssetMenu(fileName = "SpritesOfAbilitiesScriptable", menuName = "Vurbiri/Colonization/SpritesOfAbilitiesScriptable", order = 51)]
	public class SpritesOfAbilitiesScriptable : ScriptableObject
    {
        [SerializeField] private IdArray<ActorAbilityId, Sprite> _abilities;

        public static implicit operator ReadOnlyIdArray<ActorAbilityId, Sprite>(SpritesOfAbilitiesScriptable self)
        {
            Resources.UnloadAsset(self);
            return self._abilities;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            for(int i = 0; i < ActorAbilityId.Count; i++)
                if (_abilities[i] == null)
                    _abilities[i] = EUtility.FindAnyAsset<Sprite>($"SP_Icon{ActorAbilityId.Names_Ed[i]}");

            if (_abilities[ActorAbilityId.CurrentHP] == null)
                _abilities[ActorAbilityId.CurrentHP] = EUtility.FindAnyAsset<Sprite>("SP_IconMaxHP");
            if (_abilities[ActorAbilityId.CurrentAP] == null)
                _abilities[ActorAbilityId.CurrentAP] = EUtility.FindAnyAsset<Sprite>("SP_IconMaxAP");
        }
#endif
    }
}
