using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Reactive;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.Colonization.Controllers
{
    public class CameraTransform : IReactive<Transform>
    {
        private const float MIN_SQR_MAGNITUDE = 1E-06f;

        private readonly Camera _camera;
        private readonly Transform _cameraTransform, _parentTransform;
        private readonly SphereBounds _bounds = new(HEX.DIAMETER_IN * CONST.MAX_CIRCLES);
        private readonly VAction<Transform> _changedTransform = new();
        private Vector3 _velocity = Vector3.zero;

        public Camera Camera => _camera;
        public Transform Transform => _cameraTransform;

        public Vector3 CameraPosition
        {
            [Impl(256)] get => _cameraTransform.localPosition;
            [Impl(256)] set
            {
                _cameraTransform.localPosition = value;
                _cameraTransform.LookAt(_parentTransform);
                _changedTransform.Invoke(_cameraTransform);
            }
        }
        public Vector3 ParentPosition { [Impl(256)] get => _parentTransform.position; }

        public CameraTransform(Camera camera)
        {
            _camera = camera;

            _cameraTransform = camera.transform;
            _parentTransform = _cameraTransform.parent;

            _cameraTransform.LookAt(_parentTransform);
        }

        [Impl(256)] public void Move(Vector3 offset)
        {
            _parentTransform.position = _bounds.ClosestPoint(_parentTransform.position + offset);
            _changedTransform.Invoke(_cameraTransform);
        }

        [Impl(256)]public bool MoveToTarget(Vector3 target, float smoothTime, float maxSqrVelocity)
        {
            _parentTransform.position = Vector3.SmoothDamp(_parentTransform.position, target, ref _velocity, smoothTime, float.PositiveInfinity, Time.unscaledDeltaTime);
            if (_velocity.sqrMagnitude > maxSqrVelocity)
            {
                _changedTransform.Invoke(_cameraTransform);
                return true;
            }

            return false;
        }

        [Impl(256)] public void Rotate(float angleY)
        {
            _parentTransform.rotation *= Quaternion.Euler(0f, angleY, 0f);
            _changedTransform.Invoke(_cameraTransform);
        }

        [Impl(256)] public void SetCameraAndParentPosition(Vector3 cameraPosition, Vector3 parentPosition)
        {
            _parentTransform.position = parentPosition;
            _cameraTransform.localPosition = cameraPosition;

            _cameraTransform.LookAt(_parentTransform);

            _changedTransform.Invoke(_cameraTransform);
        }

        public Vector3 ToRelatively(Vector2 direction)
        {
            Quaternion rotation = _cameraTransform.rotation;

            float rotX = rotation.x * 2f;
            float rotY = rotation.y * 2f;
            float rotZ = rotation.z * 2f;

            Vector2 right = Normalize((1f - (rotation.y * rotY + rotation.z * rotZ)), rotation.x * rotZ - rotation.w * rotY);
            Vector2 forward = Normalize(rotation.x * rotZ + rotation.w * rotY, (1f - (rotation.x * rotX + rotation.y * rotY)));

            return new(direction.x * right.x + direction.y * forward.x, 0f, direction.x * right.y + direction.y * forward.y);

            #region Local Normalize(..)
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static Vector2 Normalize(float x, float y)
            {
                float magnitude = x * x + y * y;
                if (magnitude > MIN_SQR_MAGNITUDE)
                {
                    magnitude = (float)Math.Sqrt(magnitude);
                    return new(x / magnitude, y / magnitude);
                }

                return Vector2.zero;
            }
            #endregion
        }

        [Impl(256)] public void TransformToLocalPosition(RectTransform target, RectTransform canvas, Vector3 worldPosition)
        {
            Vector3 screenPosition = _camera.WorldToScreenPoint(worldPosition);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPosition, _camera, out Vector2 localPoint))
                target.anchoredPosition = localPoint;

            target.rotation = Quaternion.LookRotation(_cameraTransform.forward);
        }

        public Subscription Subscribe(Action<Transform> action, bool instantGetValue = true) => _changedTransform.Add(action, instantGetValue, _cameraTransform);
        public void Unsubscribe(Action<Transform> action) => _changedTransform.Remove(action);

        public static implicit operator Transform(CameraTransform self) => self._cameraTransform;
    }
}
