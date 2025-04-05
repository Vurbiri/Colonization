//Assets\Vurbiri.UI\Runtime\ColorTween\ColorTweenInfo.cs
using UnityEngine;

namespace Vurbiri.UI
{
    public struct ColorTweenInfo
	{
        public Color targetColor;
        public bool fade;
        public float speed;

        public void Set(Color targetColor, bool fade, float duration)
        {
            this.targetColor = targetColor;
            this.fade = fade;
            if (fade) speed = 1f / duration;
        }

        public void Set(bool fade, float duration)
        {
            this.fade = fade;
            if (fade) speed = 1f / duration; 
        }
    }
}
