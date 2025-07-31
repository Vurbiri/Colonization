using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public class SharedSFXUser : ISFXUser
    {
        public Vector3 StartPosition => Vector3.zero;
        public AudioSource AudioSource => GameContainer.AudioSource;
    }
}
