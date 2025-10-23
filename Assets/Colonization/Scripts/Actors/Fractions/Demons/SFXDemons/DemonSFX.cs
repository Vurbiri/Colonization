using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    sealed public class DemonSFX : ActorSFX
    {
        private string _specSFX;

        public void Init(ReadOnlyArray<string> hitSFX, string specSFX)
        {
            InitInternal(hitSFX);
            _specSFX = specSFX;
        }

        public Coroutine Spec(ActorSkin target) => StartCoroutine(GameContainer.SFX.Run(_specSFX, this, target));

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetProfitSFX_Ed("DemonMainProfit", "AdvProfit");
        }
#endif
    }
}
