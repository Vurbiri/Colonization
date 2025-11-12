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
            private Vector3 _oldPosition, _targetPosition;
            private float _oldHeight, _targetHeight;
            private bool _isReturn;

            public bool Return { [Impl(256)] set => _isReturn = value; }
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

                if (_isReturn)
                {
                    _targetPosition = _oldPosition;
                    _targetHeight = _oldHeight;
                }
                else
                {
                    _oldPosition = parentPosition;
                    _oldHeight = cameraPosition.y;
                    _targetPosition = Vector3.zero;
                    _targetHeight = _default.height;
                }

                float height = cameraPosition.y - _targetHeight;
                float sqrDistance = Mathf.Max((parentPosition - _targetPosition).sqrMagnitude * _sqrRatioParentDistance, height * height);

                if(sqrDistance > _minSqrDistance)
                {
                    float speed = Mathf.Sqrt(_maxSqrDistance / sqrDistance) * _default.maxTime * _ratioSpeed;
                    _coroutine = StartCoroutine(MoveToDefault_Cn(parentPosition, cameraPosition, speed));
                }
                else
                {
                    cameraPosition.y = _targetHeight;
                    _cameraTransform.SetCameraAndParentPosition(cameraPosition, _targetPosition);
                    GetOutOfThisState();
                }
            }

            private IEnumerator MoveToDefault_Cn(Vector3 parentPosition, Vector3 cameraPosition, float maxSpeed)
            {
                float progress = 0f;
                float speed = maxSpeed, deltaSpeed = _default.minSpeed - maxSpeed;
                float startX = parentPosition.x, startY = cameraPosition.y, startZ = parentPosition.z;
                float deltaX = _targetPosition.x - startX, deltaY = _targetHeight - startY, deltaZ = _targetPosition.z - startZ;

                while (progress < 1f)
                {
                    progress = Mathf.Clamp01(progress + Time.unscaledDeltaTime * speed);

                    parentPosition.x = startX + deltaX * progress;
                    cameraPosition.y = startY + deltaY * progress;
                    parentPosition.z = startZ + deltaZ * progress;
                    _cameraTransform.SetCameraAndParentPosition(cameraPosition, parentPosition);

                    speed = maxSpeed + deltaSpeed * progress;
                    yield return null;
                }

                cameraPosition.y = _targetHeight;
                _cameraTransform.SetCameraAndParentPosition(cameraPosition, _targetPosition);

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
