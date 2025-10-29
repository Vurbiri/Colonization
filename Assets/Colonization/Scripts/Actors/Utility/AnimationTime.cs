using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class AnimationTime
    {
        [SerializeField] private float[] _hitTimes;
        [SerializeField] private float _remainingTime;

        public WaitScaledTime WaitEnd { [Impl(256)] get => new(_remainingTime); }
        public WaitScaledTime[] WaitHits 
        {
            [Impl(256)] get
            {
                int count = _hitTimes.Length; var wait = new WaitScaledTime[count];
                for (int i = 0; i < count; i++)
                    wait[i] = new(_hitTimes[i]);
                
                return wait;
            }
        }

        public AnimationTime() { }


#if UNITY_EDITOR

        public AnimationTime(float time) => _remainingTime = time;

        public AnimationTime(AnimationClipSettingsScriptable clipSettings)
        {
            if (clipSettings == null || clipSettings.hitTimes == null || clipSettings.hitTimes.Length == 0)
                return;

            int count = clipSettings.hitTimes.Length;
            _hitTimes = new float[count];

            float totalTime = clipSettings.totalTime, currentTime, prevTime;
            _hitTimes[0] = prevTime = totalTime * clipSettings.hitTimes[0] / 100f;

            for (int i = 1; i < count; i++)
            {
                currentTime = totalTime * clipSettings.hitTimes[i] / 100f;
                _hitTimes[i] = currentTime - prevTime;
                prevTime = currentTime;
            }

            if(clipSettings.totalTimeRatio < 99.99f)
                totalTime *= clipSettings.totalTimeRatio / 100f;

            _remainingTime = totalTime - prevTime;
        }
#endif
    }
}
