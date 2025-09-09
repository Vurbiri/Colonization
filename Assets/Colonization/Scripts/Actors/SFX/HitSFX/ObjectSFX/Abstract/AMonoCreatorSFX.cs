using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public abstract class AMonoCreatorSFX : MonoBehaviour
	{
        public abstract APooledSFX Create(Action<APooledSFX> deactivate);

#if UNITY_EDITOR
        public abstract TargetForSFX_Ed Target_Ed { get; }
#endif
    }
}
