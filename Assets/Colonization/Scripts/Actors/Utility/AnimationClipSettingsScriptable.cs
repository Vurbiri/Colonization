//Assets\Colonization\Scripts\Actors\Utility\AnimationClipSettingsScriptable.cs
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = "ACS_", menuName = "Vurbiri/Colonization/AnimationClipSettings", order = 51)]
    public class AnimationClipSettingsScriptable : ScriptableObject
    {
        public AnimationClip clip;
        public float totalTime;
        public float damageTime;
        public float range;
    }
}
