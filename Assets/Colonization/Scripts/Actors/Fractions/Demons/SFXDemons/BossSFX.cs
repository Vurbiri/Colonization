using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
    sealed public class BossSFX : ActorSFX
    {
        [Space]
        [SerializeField] private MeshRenderer _box;
        
        private string _specSFX;

        public void Init(ReadOnlyArray<string> hitSFX, string specSFX)
        {
            InitInternal(hitSFX);
            _specSFX = specSFX;
            _box.enabled = false;
        }

        public void SpecSkillStart() => _box.enabled = true;
        public void SpecSkill(ActorSkin target)
        {
            _box.enabled = false;
            StartCoroutine(GameContainer.SFX.Run(_specSFX, this, target));
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            this.SetChildren(ref _box, "Box");
            SetProfitSFX_Ed("DemonMainProfit", "AdvProfit");
        }
#endif
    }
}
