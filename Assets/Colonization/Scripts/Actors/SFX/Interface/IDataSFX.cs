using UnityEngine;

namespace Vurbiri.Colonization
{
    public interface IDataSFX
	{
		public Transform Main { get; }
        public Transform Adv { get; }
        public AudioSource AudioSource { get; }
	}
}
