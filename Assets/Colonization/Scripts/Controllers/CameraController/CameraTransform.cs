using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Vurbiri.Reactive;

namespace Vurbiri.Colonization.Controllers
{
	public class CameraTransform : IReactive<Transform>
    {
        private const float MIN_SQR_MAGNITUDE = 1E-05f;

        private readonly Camera _camera;
        private readonly Transform _cameraTransform, _parentTransform;
        private readonly SphereBounds _bounds = new(CONST.HEX_DIAMETER_IN * CONST.MAX_CIRCLES);
        private readonly Subscription<Transform> _changedTransform = new();
        private Vector3 _velocity = Vector3.zero;

        public Camera Camera => _camera;
        public Transform Transform => _cameraTransform;

        public Vector3 CameraPosition
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _cameraTransform.localPosition;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                _cameraTransform.localPosition = value;
                _cameraTransform.LookAt(_parentTransform);
                _changedTransform.Invoke(_cameraTransform);
            }
        }
        public Vector3 ParentPosition => _parentTransform.position;


        public CameraTransform(Camera camera)
        {
            _camera = camera;

            _cameraTransform = camera.transform;
            _parentTransform = _cameraTransform.parent;

            _cameraTransform.LookAt(_parentTransform);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Move(Vector3 offset)
        {
            _parentTransform.position = _bounds.ClosestPoint(_parentTransform.position + offset);
            _changedTransform.Invoke(_cameraTransform);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveToTarget(Vector3 target, float smoothTime, float maxSqrVelocity)
        {
            _parentTransform.position = Vector3.SmoothDamp(_parentTransform.position, target, ref _velocity, smoothTime, float.PositiveInfinity, Time.unscaledDeltaTime);
            if (_velocity.sqrMagnitude > maxSqrVelocity)
            {
                _changedTransform.Invoke(_cameraTransform);
                return true;
            }

            _velocity = Vector3.zero;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Rotate(float angleY)
        {
            _parentTransform.rotation *= Quaternion.Euler(0f, angleY, 0f);
            _changedTransform.Invoke(_cameraTransform);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetCameraAndParentPosition(Vector3 cameraPosition, Vector3 parentPosition)
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

            float rightX = (1f - (rotation.y * rotY + rotation.z * rotZ));
            float rightZ = rotation.x * rotZ - rotation.w * rotY;

            float magnitude = rightX * rightX + rightZ * rightZ; 
            if (magnitude > MIN_SQR_MAGNITUDE)
            {
                magnitude = (float)Math.Sqrt(magnitude);
                rightX /= magnitude; rightZ /= magnitude;  
            }
            else
            {
                rightX = rightZ = 0f;
            }

            float forwardX = rotation.x * rotZ + rotation.w * rotY;
            float forwardZ = (1f - (rotation.x * rotX + rotation.y * rotY));

            magnitude = forwardX * forwardX + forwardZ * forwardZ;
            if (magnitude > MIN_SQR_MAGNITUDE)
            {
                magnitude = (float)Math.Sqrt(magnitude);
                forwardX /= magnitude; forwardZ /= magnitude;
            }
            else
            {
                forwardX = forwardZ = 0f;
            }

            return new(direction.x * rightX + direction.y * forwardX, 0f, direction.x * rightZ + direction.y * forwardZ);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TransformToLocalPosition(RectTransform target, RectTransform canvas, Vector3 worldPosition)
        {
            Vector3 screenPosition = _camera.WorldToScreenPoint(worldPosition);
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPosition, _camera, out Vector2 localPoint))
                target.anchoredPosition = localPoint;

            target.rotation = Quaternion.LookRotation(_cameraTransform.forward);
        }


        public Unsubscription Subscribe(Action<Transform> action, bool instantGetValue = true) => _changedTransform.Add(action, instantGetValue, _cameraTransform);
        public void Unsubscribe(Action<Transform> action) => _changedTransform.Remove(action);

    }
}
