using UnityEngine;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class WarriorSettings : ActorSettings
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private RuntimeAnimatorController _animatorController;
    }
}
