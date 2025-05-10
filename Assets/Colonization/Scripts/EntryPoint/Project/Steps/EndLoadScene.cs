//Assets\Colonization\Scripts\EntryPoint\Project\Steps\EndLoadScene.cs
using UnityEngine;

namespace Vurbiri.Colonization.EntryPoint
{
    sealed internal class EndLoadScene : ALoadingStep
    {
        private readonly AsyncOperation _operation;

        private bool _allowSceneActivation;

        public EndLoadScene(AsyncOperation operation) : base("EndLoadScene")
        {
            _operation = operation;
            _allowSceneActivation = operation.allowSceneActivation;
        }

        public override bool MoveNext()
        {
            if (!_allowSceneActivation)
                _operation.allowSceneActivation = _allowSceneActivation = true;

            if (!_operation.isDone)
                return true;

            Message.Log("End Init Project");
            return false;
        }
    }
}
