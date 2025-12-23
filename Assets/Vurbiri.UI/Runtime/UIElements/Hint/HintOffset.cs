using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
	public readonly struct HintOffset
	{
		private readonly Vector2 _center;
		private readonly float _deltaY;

		public HintOffset(RectTransform rectTransform, float heightRatio)
		{
			var pivot = rectTransform.pivot;
			var size = rectTransform.rect.size;

			_center = new (size.x * (0.5f - pivot.x), size.y * (0.5f - pivot.y));
			_deltaY = size.y * heightRatio;
		}

		[Impl(256)] public readonly float GetDeltaY(float deltaY) => _deltaY + deltaY;

		[Impl(256)] public readonly Vector3 GetCenterPosition(Vector3 position)
		{
			return new(position.x + _center.x, position.y + _center.y, position.z);
		}

		[Impl(256)] public readonly Vector3 GetOffsetPosition(Vector3 position, float deltaY)
		{
			return new(position.x + _center.x, position.y + _center.y + _deltaY + deltaY, position.z);
		}
	}
}
