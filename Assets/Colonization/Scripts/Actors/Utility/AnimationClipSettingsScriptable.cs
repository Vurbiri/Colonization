//Assets\Colonization\Scripts\Actors\Utility\AnimationClipSettingsScriptable.cs
#if UNITY_EDITOR

using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    [CreateAssetMenu(fileName = "ACS_", menuName = "Vurbiri/Colonization/AnimationClipSettings", order = 51)]
    public class AnimationClipSettingsScriptable : ScriptableObject
    {
        public AnimationClip clip;
        public float totalTime;
        public float[] hitTimes = new float[1];
        public float range;
        public float distance = -1;
    }
}
#endif