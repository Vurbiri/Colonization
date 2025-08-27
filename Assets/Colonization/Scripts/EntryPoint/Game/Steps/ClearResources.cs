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
            Resources.UnloadUnusedAssets();
            yield return null;
            GC.Collect();
        }
    }
}
