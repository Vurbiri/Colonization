using System.Collections;
using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class MoveToDefaultState : ACameraState<float>
        {
            private readonly WaitSignal _waitSignal = new();
            private readonly Zoom _zoom;
            private readonly Default _default;
            private readonly float _sqrRatioParentDistance, _maxSqrDistance, _minSqrDistance;
            private float _ratioSpeed = 1f;

            public override float InputValue { [Impl(256)] get => _ratioSpeed; [Impl(256)] set => _ratioSpeed = value; }
            public WaitSignal Signal { [Impl(256)] get => _waitSignal; }

            public MoveToDefaultState(CameraController controller) : base(controller)
            {
                _default = controller._default;
                _zoom = controller._zoom;

                float widthDistance = HEX.DIAMETER_IN * CONST.MAX_CIRCLES;
                float heightDistance = _default.height - _zoom.heightZoomMin - _zoom.minDeltaHeight * 0.9f;
                _maxSqrDistance = heightDistance * heightDistance;
                _sqrRatioParentDistance = _maxSqrDistance / (widthDistance * widthDistance);
                _minSqrDistance = _maxSqrDistance * 0.001f;
            }

            public override void Enter()
            {
                _waitSignal.Reset();

                Vector3 parentPosition = _cameraTransform.ParentPosition;
                Vector3 cameraPosition = _cameraTransform.CameraPosition;
                float height = cameraPosition.y - _default.height;
                float sqrDistance = Mathf.Max(parentPosition.sqrMagnitude * _sqrRatioParentDistance, height * height);

                if(sqrDistance > _minSqrDistance)
                {
                    float speed = Mathf.Sqrt(_maxSqrDistance / sqrDistance) * _default.maxTime * _ratioSpeed;
                    _coroutine = StartCoroutine(MoveToDefault_Cn(parentPosition, cameraPosition, speed));
                }
                else
                {
                    cameraPosition.y = _default.height;
                    _cameraTransform.SetCameraAndParentPosition(cameraPosition, Vector3.zero);
                    GetOutOfThisState();
                }
            }

            private IEnumerator MoveToDefault_Cn(Vector3 startParentPosition, Vector3 cameraPosition, float maxSpeed)
            {
                float progress = 0f;
                float speed = maxSpeed, deltaSpeed = _default.minSpeed - maxSpeed;
                float startHeight = cameraPosition.y, deltaHeight = _default.height - startHeight;
                
                while (progress < 1f)
                {
                    progress = Mathf.Clamp01(progress + Time.unscaledDeltaTime * speed);

                    cameraPosition.y = startHeight + deltaHeight * progress;
                    _cameraTransform.SetCameraAndParentPosition(cameraPosition, startParentPosition * (1f - progress));

                    speed = maxSpeed + deltaSpeed * progress;

                    yield return null;
                }

                cameraPosition.y = _default.height;
                _cameraTransform.SetCameraAndParentPosition(cameraPosition, Vector3.zero);

                _coroutine = null;
                GetOutOfThisState();
            }

            public override void Exit()
            {
                base.Exit();
                _waitSignal.Send();
            }
        }
    }
}
