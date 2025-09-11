using System.Collections;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
	sealed public class FattySFX : ActorSFX
    {
        [Space]
        [SerializeField] private MoveUsingLerp _cameraShake;
        [SerializeField] private float _shakePitch = 1.07f;

        private string _specSFX;

        public void Init(ReadOnlyArray<string> hitSFX, string specSFX)
        {
            _cameraShake.Transform = GameContainer.CameraTransform;
            InitInternal(hitSFX);
            _specSFX = specSFX;
        }

        public void Spec(ActorSkin target)
        {
            StartCoroutine(CameraShake_Cn());
            GameContainer.HitSFX.Hit(_specSFX, this, target);
        }

        private IEnumerator CameraShake_Cn()
        {
            var start = _cameraShake.Transform.localPosition;
            yield return _cameraShake.Run(new(start.x, start.y * _shakePitch, start.z));
            yield return _cameraShake.Run(start);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_cameraShake.Speed <= 0f)
                _cameraShake.SetSpeed_Ed(10f);
        }
#endif
    }
}
