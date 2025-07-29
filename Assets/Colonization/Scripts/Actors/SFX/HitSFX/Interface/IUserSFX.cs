using UnityEngine;

namespace Vurbiri.Colonization
{
    public interface IUserSFX
	{
        public Vector3 StartPosition { get; }
        public AudioSource AudioSource { get; }
	}
}
