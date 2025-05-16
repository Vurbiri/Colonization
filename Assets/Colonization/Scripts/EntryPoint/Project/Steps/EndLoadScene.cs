//Assets\Colonization\Scripts\EntryPoint\Project\Steps\EndLoadScene.cs
using System.Collections;
using UnityEngine;
using Vurbiri.EntryPoint;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class EndLoadScene : ALoadingStep
    {
        private readonly AsyncOperation _operation;

        public EndLoadScene(AsyncOperation operation) : base(string.Empty)
        {
            _operation = operation;
        }

        public override IEnumerator GetEnumerator()
        {
            _operation.allowSceneActivation = true;

            while (!_operation.isDone)
                yield return null;

            yield return null;
        }
    }
}
