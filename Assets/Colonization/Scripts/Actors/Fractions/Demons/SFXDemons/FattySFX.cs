using System.Collections;
using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization.Actors
{
	sealed public class FattySFX : ActorSFX
    {
        [Space]
        [SerializeField] private Transform _mouth;
        [Space]
        [SerializeField] private MoveUsingLerp _cameraShake;
        [SerializeField] private float _shakePitch = 1.07f;

        private string _specSFX;

        public override Transform TargetTransform => _mouth;

        public void Init(ReadOnlyArray<string> hitSFX, string specSFX)
        {
            _cameraShake.Transform = GameContainer.CameraTransform;
            InitInternal(hitSFX);
            _specSFX = specSFX;
        }

        public void Spec(ActorSkin target)
        {
            StartCoroutine(CameraShake_Cn());
            StartCoroutine(GameContainer.SFX.Run(_specSFX, this, target));
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
            this.SetChildren(ref _mouth, "Mouth");
            if (_cameraShake.Speed <= 0f)
                _cameraShake.SetSpeed_Ed(10f);

            SetProfitSFX_Ed("DemonMainProfit", "AdvProfit");
        }
#endif
    }
}
