using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
    public static class HintEx
    {
        [Impl(256)] public static Hint Get(this Id<HintId> id) => Hint.s_instances[id];
        [Impl(256)] public static bool Show(this Id<HintId> id, string text, Transform transform, Vector3 offset) => Hint.s_instances[id].Show(text, transform, offset);
        [Impl(256)] public static bool Hide(this Id<HintId> id) => Hint.s_instances[id].Hide();

        [Impl(256)]
        public static Vector3 GetOffsetHint(this RectTransform rectTransform, float heightRatio)
        {
            var pivot = rectTransform.pivot;
            var size = rectTransform.rect.size;

            return new(size.x * (0.5f - pivot.x), size.y * (Mathf.Abs(0.5f - pivot.y) + heightRatio), 0f);
        }
    }
}
