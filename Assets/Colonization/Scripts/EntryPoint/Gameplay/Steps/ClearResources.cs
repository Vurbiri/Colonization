//Assets\Colonization\Scripts\EntryPoint\Gameplay\Steps\ClearResources.cs
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
            yield return new WaitFrames(15);

            Resources.UnloadUnusedAssets();
            yield return null;

            GC.Collect();
            yield return null;
        }
    }
}
