using System;
using UnityEngine;
using Vurbiri.Collections;
using Vurbiri.Colonization.Actors;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Characteristics
{
	public abstract class ASkillSettings
	{
        [SerializeField] protected TargetOfSkill _target;
        [SerializeField] protected float _range;
        [SerializeField] protected float _distance;
        [SerializeField] protected int _cost;
        [SerializeField] protected HitEffectsSettings[] _effectsHitsSettings;

        [NonSerialized] protected Array<HitEffects> _hitEffects;

        public TargetOfSkill Target { [Impl(256)] get => _target; }
        public float Range { [Impl(256)] get => _range; }
        public float Distance { [Impl(256)] get => _distance; }
        public int Cost { [Impl(256)] get => _cost; }
        public ReadOnlyArray<HitEffects> HitEffects { [Impl(256)] get => _hitEffects; }


#if UNITY_EDITOR
        public AnimationClipSettingsScriptable clipSettings_ed;
        public int typeActor_ed;
#endif
    }
}
