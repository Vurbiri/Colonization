using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class SharedSFXUser : ISFXUser
    {
        public Vector3 StartPosition => Vector3.zero;
        public Transform Container => null;
        public AudioSource AudioSource => GameContainer.SharedAudioSource;
    }
}
