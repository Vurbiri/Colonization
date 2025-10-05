using UnityEngine;
using Vurbiri.Collections;

namespace Vurbiri.Colonization
{
    [System.Serializable]
    public class BuilderSettings
	{
        public int minBreach; // = 4;
        public int minWeight;
        [Space]
        public int resWeight;
        public ReadOnlyIdArray<EdificeId, int> edificeWeight;
        public int wallWeight;
    }
}
