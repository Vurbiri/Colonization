//Assets\Vurbiri.UI\Runtime\UIElements\Utility\ScaleBlock.cs
using System;
using UnityEngine;

namespace Vurbiri.UI
{
    [Serializable]
    public struct ScaleBlock : IEquatable<ScaleBlock>
    {
        public readonly static ScaleBlock defaultScaleBlock;

        public Vector3 normal;
		public Vector3 highlighted;
        public Vector3 pressed;
        public Vector3 selected;
        public Vector3 disabled;
        [Range(0f, 1f)]
        public float fadeDuration;
                
        static ScaleBlock()
        {
            defaultScaleBlock = new ScaleBlock()
            {
                normal       = Vector3.one,
                highlighted  = Vector3.one * 1.035f,
                pressed      = Vector3.one * 0.99f,
                selected     = Vector3.one * 1.05f,
                disabled     = Vector3.one * 0.99f,
                fadeDuration = 0.1f
            };
        }

        public readonly bool Equals(ScaleBlock other)
        {
            return normal == other.normal &&
                   highlighted == other.highlighted &&
                   pressed == other.pressed &&
                   selected == other.selected && 
                   fadeDuration == other.fadeDuration;
        }

        public override readonly bool Equals(object obj)
        {
            if (obj is ScaleBlock other)
                return Equals(other);

            return false;
        }

        public static bool operator ==(ScaleBlock block1, ScaleBlock block2) => block1.Equals(block2);
        public static bool operator !=(ScaleBlock block1, ScaleBlock block2) => !block1.Equals(block2);

        public override readonly int GetHashCode() => base.GetHashCode();

    }
}
