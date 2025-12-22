using UnityEngine;
using Impl = System.Runtime.CompilerServices.MethodImplAttribute;

namespace Vurbiri.UI
{
    public static class HintEx
    {
        [Impl(256)] public static Hint Get(this Id<HintId> id) => Hint.s_instances[id];
        [Impl(256)] public static bool Show(this Id<HintId> id, string text, RectTransform transform, HintOffset offset) => Hint.s_instances[id].Show(text, transform, offset);
        [Impl(256)] public static bool Hide(this Id<HintId> id) => Hint.s_instances[id].Hide();

        [Impl(256)] public static HintOffset GetHintOffset(this RectTransform rectTransform, float heightRatio) => new(rectTransform, heightRatio);
    }
}
