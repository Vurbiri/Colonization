using UnityEngine;

namespace Vurbiri.Colonization.Actors.UI
{
    public class EffectsBarSettings
	{
        public readonly Vector3 startPosition;
        public readonly Vector3 offsetPosition;
        public readonly int maxIndex;
        public readonly int orderLevel;

        public EffectsBarSettings(Vector3 startPosition, Vector3 offsetPosition, int maxIndex, int orderLevel)
        {
            this.startPosition = startPosition;
            this.offsetPosition = offsetPosition;
            this.maxIndex = maxIndex;
            this.orderLevel = orderLevel;
        }
    }
}
