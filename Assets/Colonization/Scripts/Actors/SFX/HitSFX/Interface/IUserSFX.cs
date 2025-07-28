using UnityEngine;

namespace Vurbiri.Colonization
{
    public interface IUserSFX
	{
        public Transform StartTransform { get; }
        public AudioSource AudioSource { get; }
	}
}
