using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri
{
    public static class TransformExtensions
    {
        [Impl(256)] public static void SetLocalPositionAndRotation(this Transform self, Transform other)
        {
            self.SetLocalPositionAndRotation(other.localPosition, other.localRotation);
        }
        
        [Impl(256)] public static void SetPositionAndRotation(this Transform self, Transform other)
        {
            self.SetPositionAndRotation(other.position, other.rotation);
        }

        #region Get Rect Positions
        [Impl(256)] public static Vector3 GetBottomLeftLocalPosition(this RectTransform self)
        {
            var rect = self.rect;
            return new(rect.x, rect.y, 0f);
        }
        [Impl(256)] public static Vector3 GetBottomLeftWorldPosition(this RectTransform self)
        {
            return self.localToWorldMatrix.MultiplyPoint(GetBottomLeftLocalPosition(self));
        }

        [Impl(256)] public static Vector3 GetTopLeftLocalPosition(this RectTransform self)
        {
            var rect = self.rect;
            return new(rect.x, rect.yMax, 0f);
        }
        [Impl(256)] public static Vector3 GetTopLeftWorldPosition(this RectTransform self)
        {
            return self.localToWorldMatrix.MultiplyPoint(GetTopLeftLocalPosition(self));
        }

        [Impl(256)] public static Vector3 GetTopRightLocalPosition(this RectTransform self)
        {
            var rect = self.rect;
            return new(rect.xMax, rect.yMax, 0f);
        }
        [Impl(256)] public static Vector3 GetTopRightWorldPosition(this RectTransform self)
        {
            return self.localToWorldMatrix.MultiplyPoint(GetTopRightLocalPosition(self));
        }

        [Impl(256)] public static Vector3 GetBottomRightLocalPosition(this RectTransform self)
        {
            var rect = self.rect;
            return new(rect.xMax, rect.y, 0f);
        }
        [Impl(256)] public static Vector3 GetBottomRightWorldPosition(this RectTransform self)
        {
            return self.localToWorldMatrix.MultiplyPoint(GetBottomRightLocalPosition(self));
        }

        [Impl(256)] public static Vector3 GetBottomLocalPosition(this RectTransform self)
        {
            var rect = self.rect;
            return new(rect.x + rect.width * 0.5f, rect.y, 0f);
        }
        [Impl(256)] public static Vector3 GetBottomWorldPosition(this RectTransform self)
        {
            return self.localToWorldMatrix.MultiplyPoint(GetBottomLocalPosition(self));
        }

        [Impl(256)] public static Vector3 GetTopLocalPosition(this RectTransform self)
        {
            var rect = self.rect;
            return new(rect.x + rect.width * 0.5f, rect.yMax, 0f);
        }
        [Impl(256)] public static Vector3 GetTopWorldPosition(this RectTransform self)
        {
            return self.localToWorldMatrix.MultiplyPoint(GetTopLocalPosition(self));
        }
        #endregion
    }
}
