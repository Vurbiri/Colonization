using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public interface ISFXUser
	{
        public Vector3 StartPosition { get; }
        public AudioSource AudioSource { get; }
	}
}
