//Assets\Colonization\Scripts\Actors\Skin\ActorSkin_Editor.cs
#if UNITY_EDITOR

using System;
using UnityEngine;

namespace Vurbiri.Colonization.Actors
{
    public partial class ActorSkin
    {
        private const string A_DEATH = "A_Death";

        public void SetAnimationClip(AnimationClipSettingsScriptable clipSettings, int id)
        {
            if (clipSettings.hitTimes == null || clipSettings.hitTimes.Length == 0)
                return;

            var timing = _timings[id];
            int count = clipSettings.hitTimes.Length;
            timing.hitTimes = new float[count];
            float totalTime = clipSettings.totalTime;
            float current, prev = timing.hitTimes[0] = totalTime * clipSettings.hitTimes[0] / 100f;
            for (int i = 1; i < count; i++)
            {
                current = totalTime * clipSettings.hitTimes[i] / 100f;
                timing.hitTimes[i] = current - prev;
                prev = current;
            }

            timing.remainingTime = clipSettings.totalTime - prev;
        }

        public void SetCountAnimationClips(int count)
        {
            _timings ??= new TimingSkillSettings[count];
            if (_timings.Length != count)
                Array.Resize(ref _timings, count);
        }

        private void OnValidate()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            if (_sfx == null)
                _sfx = GetComponent<AActorSFX>();

            if (_animator != null)
                _durationDeath = ((AnimatorOverrideController)_animator.runtimeAnimatorController)[A_DEATH].length;
        }
    }
}
#endif
