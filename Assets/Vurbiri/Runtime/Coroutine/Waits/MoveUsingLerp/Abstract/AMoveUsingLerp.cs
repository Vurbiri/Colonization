using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
	public abstract class AMoveUsingLerp : AWait
	{
		[SerializeField] private Transform _transform;
		[SerializeField] private float _speed;

		private Vector3 _start, _end, _delta;
		private float _progress = 1f;
		private bool _isWait;

		protected abstract float DeltaTime { get; }

		sealed public override bool IsWait { [Impl(256)] get => _isWait; }
		public Transform Transform { [Impl(256)] get => _transform; [Impl(256)] set => _transform = value; }
		public float Speed { [Impl(256)] get => _speed; }

		[Impl(256)] public AMoveUsingLerp(Transform transform, float speed)
		{
			_transform = transform; _speed = speed;
		}

		sealed public override bool MoveNext()
		{
			_progress += DeltaTime * _speed;
			if (_isWait = _progress < 1f)
				_transform.localPosition = new(_start.x + _delta.x * _progress, _start.y + _delta.y * _progress, _start.z + _delta.z * _progress);
			else
				_transform.localPosition = _end;
			
			return _isWait;
		}

		public AMoveUsingLerp Run(Vector3 target)
		{
			Set(_transform.localPosition, target, _speed);
			return this;
		}
		public AMoveUsingLerp Run(Vector3 target, float speed)
		{
			Set(_transform.localPosition, target, speed);
			return this;
		}

		public AMoveUsingLerp Run(Vector3 current, Vector3 target)
		{
			Set(current, target, _speed);
			return this;
		}
		public AMoveUsingLerp Run(Vector3 current, Vector3 target, float speed)
		{
			Set(current, target, speed);
			return this;
		}

		public void Run(MonoBehaviour mono, Vector3 target)
		{
			Set(_transform.localPosition, target, _speed);
			if (!_isWait)
				mono.StartCoroutine(this);
		}

		[Impl(256)] public void Skip()
		{
			if (_isWait)
			{
				_isWait = false;
				_progress = 1f;
				_transform.localPosition = _end;
			}
		}
		[Impl(256)] public void Stop()
		{
			if (_isWait)
			{
				_isWait = false;
				_progress = 1f;
			}
		}

		[Impl(256)] private void Set(Vector3 current, Vector3 target, float speed)
		{
			_speed    = speed;
			_start    = current; 
			_end      = target; 
			_delta    = _end - _start;
			_progress = 0f;
		}

#if UNITY_EDITOR
		public void SetSpeed_Ed(float speed) => _speed = speed;
#endif
	}
}
