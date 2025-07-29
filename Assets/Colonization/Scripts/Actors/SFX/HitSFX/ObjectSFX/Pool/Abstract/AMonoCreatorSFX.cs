using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
	public abstract class AMonoCreatorSFX : MonoBehaviour
	{
        public abstract APooledSFX Create(Action<APooledSFX> deactivate);
	}
}
