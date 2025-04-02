//Assets\Vurbiri\Runtime\Utilities\Extensions\ExtensionsGraphic.cs
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Vurbiri
{
    public static class ExtensionsGraphic
    {

        public static Coroutine FadeAlpha(this Graphic self, float alpha, float speed)
        {
             return self.StartCoroutine(FadeAlpha_Cn(self, alpha, speed));
        }

        private static IEnumerator FadeAlpha_Cn(Graphic graphic, float alpha, float speed)
        {
            Color color = graphic.color;
            float current = color.a;
            float progress = 0f;

            while (progress <= 1f)
            {
                yield return null;
                progress += speed * Time.unscaledDeltaTime;
                current = Mathf.Lerp(current, alpha, progress);
                graphic.color = color.SetAlpha(current);
            }
            graphic.color = color.SetAlpha(alpha);
        }
    }
}
