using System;
using System.Collections;
using UnityEngine;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class ClearResources : ALocalizationLoadingStep
    {
        public ClearResources() : base(0.2f, "ResourceClearingStep") { }

        public override IEnumerator GetEnumerator()
        {
            yield return new WaitFrames(13);

            Resources.UnloadUnusedAssets();
            yield return new WaitFrames(3);

            GC.Collect();
            yield return null;
        }
    }
}
