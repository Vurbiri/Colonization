//Assets\Vurbiri.EntryPoint\Runtime\Loading\Step\LoadSceneStep.cs
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vurbiri.EntryPoint
{
    sealed internal class LoadSceneStep : AEnumeratorStep
    {
        private readonly AsyncOperation _operation;

        public LoadSceneStep(int sceneBuildIndex, string desc) : base(0.9f, desc)
        {
            _operation = SceneManager.LoadSceneAsync(sceneBuildIndex);
        }

        public override bool MoveNext() => !_operation.isDone;
    }
}
