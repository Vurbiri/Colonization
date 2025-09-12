using System;
using UnityEngine;

namespace Vurbiri.Reactive
{
	public enum TransformChange
    {
        Position, Rotation, Scale, PositionAndRotation
    }


    public class RTransform : IReactive<Vector3>, IReactive<Quaternion>, IReactive<Transform>
	{
		protected readonly Transform _transform;
        protected readonly Subscription<Vector3> _changedPosition = new();
        protected readonly Subscription<Quaternion> _changedRotation = new();
        protected readonly Subscription<Transform> _changedTransform = new();

        public Vector3 position
        {
            get => _transform.position;
            set
            {
                if (value != _transform.position)
                {
                    _transform.position = value;
                    _changedPosition.Invoke(value);
                    _changedTransform.Invoke(_transform);
                }
            }
        }

        public RTransform(Transform transform)
        {
            _transform = transform;
        }

        public Unsubscription Subscribe(Action<Vector3> action, bool instantGetValue = true)
        {
            return _changedPosition.Add(action, instantGetValue, _transform.position);
        }
        public Unsubscription Subscribe(Action<Quaternion> action, bool instantGetValue = true)
        {
            return _changedRotation.Add(action, instantGetValue, _transform.rotation);
        }
        public Unsubscription Subscribe(Action<Transform> action, bool instantGetValue = true)
        {
            return _changedTransform.Add(action, instantGetValue, _transform);
        }
    }
}
