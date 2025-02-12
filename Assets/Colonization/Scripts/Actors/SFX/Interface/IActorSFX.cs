//Assets\Colonization\Scripts\Actors\SFX\Interface\IActorSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    public interface IActorSFX
	{
		public Transform Main { get; }
        public Transform RightHand { get; }
        public AudioSource AudioSource { get; }
	}
}
