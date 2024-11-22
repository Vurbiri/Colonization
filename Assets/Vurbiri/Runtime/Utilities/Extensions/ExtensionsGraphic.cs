//Assets\Vurbiri\Runtime\Utilities\Extensions\ExtensionsGraphic.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri
{
    public static class ExtensionsGraphic
    {
        public static Coroutine Appear(this Graphic self, Color color, float duration, bool isUnscaled = false)
        {
            if (isUnscaled)
                return self.StartCoroutine(SmoothAlphaUnscaled_Coroutine(self, color, 0f, 1f, duration));
            else
                return self.StartCoroutine(SmoothAlpha_Coroutine(self, color, 0f, 1f, duration));
        }
        public static Coroutine Fade(this Graphic self, Color color, float duration, bool isUnscaled = false)
        {
            if (isUnscaled)
                return self.StartCoroutine(SmoothAlphaUnscaled_Coroutine(self, color, 1f, 0f, duration));
            else
                return self.StartCoroutine(SmoothAlpha_Coroutine(self, color, 1f, 0f, duration));
        }

        private static IEnumerator SmoothAlpha_Coroutine(Graphic graphic, Color color, float start, float end, float duration)
        {
            float currentTime = 0f;
            float alpha;
            while (currentTime < duration)
            {
                alpha = Mathf.Lerp(start, end, currentTime / duration);
                graphic.color = color.SetAlpha(alpha);
                currentTime += Time.deltaTime;
                yield return null;
            }
            graphic.color = color.SetAlpha(end);
        }

        private static IEnumerator SmoothAlphaUnscaled_Coroutine(Graphic graphic, Color color, float start, float end, float duration)
        {
            float currentTime = 0f;
            float alpha;
            while (currentTime < duration)
            {
                alpha = Mathf.Lerp(start, end, currentTime / duration);
                graphic.color = color.SetAlpha(alpha);
                currentTime += Time.unscaledDeltaTime;
                yield return null;
            }
            graphic.color = color.SetAlpha(end);
        }

    }
}
