using UnityEngine;

namespace Vurbiri.Colonization
{
    public partial class Skills
    {
        [System.Serializable]
        private class MoveSkill
        {
            [Range(0.1f, 1.5f)] public float speed = 0.45f;
        }
    }
}
