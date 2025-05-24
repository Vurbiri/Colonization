using UnityEngine;

namespace Vurbiri.Colonization
{
    public interface IDataSFX
	{
		public Transform Main { get; }
        public Transform RightHand { get; }
        public AudioSource AudioSource { get; }
	}
}
