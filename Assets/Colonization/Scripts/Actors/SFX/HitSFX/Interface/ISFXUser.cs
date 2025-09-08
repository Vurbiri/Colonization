using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public interface ISFXUser
	{
        public Vector3 StartPosition { get; }
        public Transform Container { get; }
        public AudioSource AudioSource { get; }
	}
}
