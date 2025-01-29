//Assets\Colonization\Scripts\Actors\SFX\Interface\IActorSFX.cs
using UnityEngine;

namespace Vurbiri.Colonization
{
    public interface IActorSFX
	{
		public Transform Container { get; }
		public AudioSource AudioSource { get; }
	}
}
