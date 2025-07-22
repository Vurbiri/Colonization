using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.Controllers
{
    public partial class CameraController
    {
        sealed private class MoveToDefaultState : ACameraState<float>
        {
            private readonly WaitSignal _waitSignal = new();
            private readonly GameTriggerBus _eventBus;
            private readonly Zoom _zoom;
            private readonly Default _default;
            private readonly float _sqrRatioParentDistance, _maxSqrDistance, _minSqrDistance;
            private float _ratioSpeed = 1f;

            public override float InputValue { get => _ratioSpeed; set => _ratioSpeed = value; }
            public WaitSignal Signal => _waitSignal;

            public MoveToDefaultState(CameraController controller, Default settings, Zoom zoom, GameTriggerBus eventBus) : base(controller)
            {
                _default = settings;
                _zoom = zoom;
                _eventBus = eventBus;

                float widthDistance = CONST.HEX_DIAMETER_IN * CONST.MAX_CIRCLES;
                float heightDistance = settings.height - zoom.heightZoomMin - zoom.minDeltaHeight * 0.9f;
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
                    _coroutine = _controller.StartCoroutine(MoveToDefault_Cn(parentPosition, cameraPosition, speed));
                }
                else
                {
                    cameraPosition.y = _default.height;
                    _cameraTransform.SetCameraAndParentPosition(cameraPosition, Vector3.zero);
                    _fsm.ToDefaultState();
                }
            }

            private IEnumerator MoveToDefault_Cn(Vector3 startParentPosition, Vector3 cameraPosition, float maxSpeed)
            {
                float progress = 0f;
                float speed = maxSpeed, deltaSpeed = _default.minSpeed - maxSpeed;
                float startHeight = cameraPosition.y, deltaHeight = _default.height - startHeight;
                bool isShow = startHeight > _zoom.heightHexagonShow;
                
                while (progress < 1f)
                {
                    progress = Mathf.Clamp01(progress + Time.unscaledDeltaTime * speed);

                    cameraPosition.y = startHeight + deltaHeight * progress;
                    _cameraTransform.SetCameraAndParentPosition(cameraPosition, startParentPosition * (1f - progress));

                    speed = maxSpeed + deltaSpeed * progress;

                    yield return null;
  
                    if (isShow != cameraPosition.y > _zoom.heightHexagonShow)
                        _eventBus.TriggerHexagonShowDistance(isShow = !isShow);
                }

                cameraPosition.y = _default.height;
                _cameraTransform.SetCameraAndParentPosition(cameraPosition, Vector3.zero);

                _coroutine = null;
                _fsm.ToDefaultState();
            }

            public override void Exit()
            {
                base.Exit();
                _waitSignal.Send();
            }
        }
    }
}
